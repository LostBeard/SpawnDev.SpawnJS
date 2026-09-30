using SpawnDev.SpawnJS.Marshaller;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// A POCO's tape codec, generated at compile time by SpawnDev.SpawnJS.Generators: its members read and set directly,
    /// with [JsonIgnore] / [JsonPropertyName] / [JsonInclude] resolved in the generated code - no reflection to build it,
    /// nothing for the trimmer to remove. Each value still goes through the member type's own marshaller, exactly as the
    /// reflection plan does, so the Javascript it produces is identical.
    /// <para>
    /// A type the generator does not handle (see its rules) keeps the reflection plan. Generated code only - not an API
    /// to implement by hand.
    /// </para>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class JSPocoCodec<T> : IJSPocoCodec
    {
        Type IJSPocoCodec.Type => typeof(T);
        /// <param name="writeNames">The Javascript names written, in order - one value per name in <see cref="WriteMembers"/>.</param>
        /// <param name="readNames">The Javascript names read, in order; null when the generator left reading to the reflection plan.</param>
        protected JSPocoCodec(string[] writeNames, string[]? readNames)
        {
            WriteNames = writeNames;
            ReadNames = readNames;
            Shape = new JSShape(writeNames);
        }
        /// <summary>The Javascript names written, in order.</summary>
        public IReadOnlyList<string> WriteNames { get; }
        /// <summary>The Javascript names read, in order - null when this codec does not read.</summary>
        public IReadOnlyList<string>? ReadNames { get; }
        internal JSShape Shape { get; }
        /// <summary>Writes one value per <see cref="WriteNames"/> entry (the object itself is started by the caller).</summary>
        public abstract void WriteMembers(JSTape tape, T value);
        /// <summary>The schema of each <see cref="ReadNames"/> member, in order.</summary>
        protected virtual JSSchema[] ReadMemberSchemas() => throw new NotSupportedException();
        /// <summary>Builds a value and reads its members (after the object start has been read).</summary>
        public virtual T ReadMembers(ref JSTapeReader reader) => throw new NotSupportedException();

        JSSchema? _readSchema;
        /// <summary>The Object schema this codec reads by - created empty, so a type that contains itself can refer to
        /// it before its members are resolved (<see cref="ResolveReadMembers"/>).</summary>
        internal JSSchema ReadSchema => _readSchema ??= JSSchema.Object(ReadNames!.ToArray());
        internal void ResolveReadMembers()
        {
            var members = ReadMemberSchemas();
            for (var i = 0; i < members.Length; i++) ReadSchema.Members![i] = members[i];
        }
    }

    /// <summary>A generated codec, whatever its type - what <see cref="JSPocoCodecs.All"/> lists.</summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IJSPocoCodec
    {
        /// <summary>The POCO type.</summary>
        Type Type { get; }
        /// <summary>The Javascript names written, in order.</summary>
        IReadOnlyList<string> WriteNames { get; }
        /// <summary>The Javascript names read, in order - null when this codec does not read.</summary>
        IReadOnlyList<string>? ReadNames { get; }
    }

    /// <summary>
    /// One member of a generated <see cref="JSPocoCodec{T}"/>: writes a value the way the reflection plan does (the
    /// declared type's marshaller when it fixes the runtime type, what the value IS otherwise) and reads it by the
    /// declared type's marshaller.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class JSPocoMember<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TMember>
    {
        ValueWriter<TMember>? _writer;
        JSMarshaller<TMember>? _marshaller;
        JSMarshaller<TMember> Marshaller => _marshaller ??= SpawnJSRuntime.Instance.GetMarshaller<TMember>();
        /// <summary>Writes a non-null member value.</summary>
        public void Write(JSTape tape, TMember value) => (_writer ??= new ValueWriter<TMember>()).Write(tape, value);
        /// <summary>Reads the member value.</summary>
        public TMember Read(ref JSTapeReader reader) => Marshaller.Read(ref reader);
        /// <summary>The schema the member is read by.</summary>
        public JSSchema Schema => Marshaller.Schema;
    }

    /// <summary>
    /// The generated POCO codecs of this runtime. Each assembly's generated module initializer registers its own.
    /// </summary>
    public static class JSPocoCodecs
    {
        // statics are per .Net runtime, so this is per SpawnJSRuntime
        static readonly ConcurrentDictionary<Type, IJSPocoCodec> _codecs = new ConcurrentDictionary<Type, IJSPocoCodec>();
        static class Slot<T> { public static JSPocoCodec<T>? Codec; }

        /// <summary>
        /// Whether generated codecs are used. True by default; false makes every POCO take the reflection plan - the
        /// switch the test suite uses to cover both, and a way out if a generated codec is ever wrong.
        /// </summary>
        public static bool UseGenerated { get; set; } = true;

        /// <summary>Registers a generated codec. Called by generated module initializers.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void Register<T>(JSPocoCodec<T> codec)
        {
            if (!_codecs.TryAdd(typeof(T), codec)) return;
            Slot<T>.Codec = codec;
        }

        /// <summary>Every registered codec, used or not (<see cref="UseGenerated"/>) - what a test compares with the
        /// reflection plan.</summary>
        public static IReadOnlyCollection<IJSPocoCodec> All => _codecs.Values.Cast<IJSPocoCodec>().ToList();

        /// <summary>The generated codec for <typeparamref name="T"/>, or null.</summary>
        public static JSPocoCodec<T>? Get<T>() => UseGenerated ? Slot<T>.Codec : null;
        internal static IJSPocoCodec? Get(Type type) => UseGenerated && _codecs.TryGetValue(type, out var codec) ? codec : null;
    }
}
