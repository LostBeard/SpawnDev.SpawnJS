using System.Collections.Concurrent;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls string
    /// </summary>
    public class StringMarshaller : JSMarshaller<string>
    {
        /// <summary>
        /// Javascript slot ids for strings that have already crossed, keyed by the .Net string.
        /// <br/><br/>
        /// A string argument is the expensive one: measured at about 1us each, where five numbers
        /// together cost 1.07us. It cannot be carried by address either - pinning saved only 1.2x,
        /// because Javascript still has to materialise a string in its own heap and a Javascript string
        /// cannot point into WebAssembly memory.
        /// <br/><br/>
        /// But an INTERNED string is a slot id, and a slot id is a number - so the second and every later
        /// use costs what a number costs, which is nothing. That fits how interop strings actually occur:
        /// they are overwhelmingly repeated literals from fixed call sites - method names, property names,
        /// "rgba8unorm".
        /// <br/><br/>
        /// A string is interned on its SECOND sighting, never its first. Interning on first sight looks
        /// free and is not: a string that never recurs takes a slot that nothing will ever read again and
        /// nothing frees, so the cache fills with exactly the strings it cannot help. That is not
        /// hypothetical - <c>Callback</c> names itself <c>cb_{n}</c>, unique per instance, and awaiting a
        /// Javascript promise makes two of them. Every await interned two dead strings, measured at 402
        /// slots over 200 awaits. Requiring a repeat first costs a one-shot string nothing beyond what it
        /// cost before interning existed, and delays a real literal's benefit by exactly one use.
        /// <br/><br/>
        /// Bounded on purpose. An unbounded cache keyed on strings an application generates would be a
        /// leak wearing an optimisation's clothes: every distinct string would hold a Javascript slot for
        /// the life of the process. Past the limit, strings simply take the ordinary path.
        /// </summary>
        static readonly ConcurrentDictionary<string, double> _interned = new(StringComparer.Ordinal);

        /// <summary>
        /// Strings seen once and not yet interned. Holds no Javascript slot - it only remembers that the
        /// string has been across before, which is what makes the second sighting cheap to detect.
        /// </summary>
        static readonly ConcurrentDictionary<string, byte> _seenOnce = new(StringComparer.Ordinal);

        /// <summary>
        /// How many distinct strings may be interned. Repeated literals are few; an application's data
        /// strings are not, and they are the ones that must not accumulate.
        /// </summary>
        public static int InternLimit { get; set; } = 2048;

        /// <summary>
        /// Longest string worth interning. A long string is unlikely to be a repeated literal, and it
        /// holds its bytes Javascript side for the life of the process.
        /// </summary>
        public static int InternMaxLength { get; set; } = 128;

        /// <summary>Number of strings currently interned. Diagnostics.</summary>
        public static int InternedCount => _interned.Count;

        /// <summary>Drops every interned string and releases their slots.</summary>
        public static void ClearInterned()
        {
            foreach (var slot in _interned.Values) SlotInterop.Free(slot);
            _interned.Clear();
            _seenOnce.Clear();
        }

        /// <inheritdoc/>
        public override object? JSToNet(Type type, SpawnJSHandle jsHandle) => jsHandle.AsString();
        /// <inheritdoc/>
        public override void NetToJS(Type? type, SpawnJSHandle jsParent, object jsKey, object? value) => jsParent.SetProperty(jsKey, (string)value!);

        /// <inheritdoc/>
        /// <remarks>
        /// Interned strings cross once and are a number ever after. A miss pays one crossing to intern -
        /// the same crossing the ordinary path would have paid anyway - so the first use is not a loss
        /// either.
        /// </remarks>
        public override bool TryWriteArg(Type? typeToConvert, object value, out byte tag, out double payload)
        {
            tag = ArgTag.Slot;
            payload = 0;
            var text = (string)value;
            if (_interned.TryGetValue(text, out payload)) return true;
            if (text.Length > InternMaxLength || _interned.Count >= InternLimit) return false;
            // first sighting: remember it and take the ordinary path. Only a string that comes back is
            // worth a slot - see the note on _seenOnce.
            if (_seenOnce.TryAdd(text, 0))
            {
                // the candidate set is bounded too, or it becomes the leak it exists to prevent
                if (_seenOnce.Count > InternLimit * 4) _seenOnce.Clear();
                return false;
            }
            payload = SlotInterop.AllocString(text);
            _seenOnce.TryRemove(text, out _);
            // a racing writer would leave the loser's slot unreachable, so keep whichever landed first
            // and release ours rather than leak it. Single threaded today; correct if that changes.
            if (!_interned.TryAdd(text, payload))
            {
                SlotInterop.Free(payload);
                payload = _interned[text];
            }
            return true;
        }
    }
}
