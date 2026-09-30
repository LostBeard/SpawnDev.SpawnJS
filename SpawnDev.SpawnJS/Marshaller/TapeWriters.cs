using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace SpawnDev.SpawnJS.Marshaller
{
    /// <summary>
    /// Writes a value whose type is only known at run time through that type's own marshaller. One writer per runtime
    /// Type, built once; a write is then a typed call. It replaces the per-value runtime Type bridge
    /// (DelegateExtensions.InvokeGeneric: a delegate and an argument array allocated, a composite-key lookup and a
    /// reflection invoke, on every value).
    /// </summary>
    internal abstract class BoxedWriter
    {
        public abstract void Write(JSTape tape, object value);

        // statics are per .Net runtime, so this is per SpawnJSRuntime
        static readonly ConcurrentDictionary<Type, BoxedWriter> _writers = new ConcurrentDictionary<Type, BoxedWriter>();

        [UnconditionalSuppressMessage("Trimming", "IL2070", Justification = "Closes SpawnJS's own writer over a runtime Type; the write path never constructs the value's type.")]
        [UnconditionalSuppressMessage("Trimming", "IL2055", Justification = "See IL2070.")]
        [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "The same runtime-Type generic instantiation v2's write path made through InvokeGeneric.")]
        public static BoxedWriter For(Type type) => _writers.GetOrAdd(type,
            t => (BoxedWriter)Activator.CreateInstance(typeof(BoxedWriter<>).MakeGenericType(t))!);
    }

    internal sealed class BoxedWriter<T> : BoxedWriter
    {
        readonly JSMarshaller<T> _marshaller = SpawnJSRuntime.Instance.GetMarshallerForWrite<T>();
        public override void Write(JSTape tape, object value) => _marshaller.Write(tape, (T)value);
    }

    /// <summary>
    /// Writes values of a DECLARED type <typeparamref name="T"/> the way every v2 write path chose a marshaller: when
    /// T fixes the runtime type (a value type or a sealed class) through T's own marshaller, which also decides how a
    /// null is written; otherwise by what each value IS - an object, an interface or a base class holds some concrete
    /// type, and only that type has a marshaller. A member or element almost always holds the same type again, so
    /// the last one seen is kept.
    /// </summary>
    internal sealed class ValueWriter<T>
    {
        readonly JSMarshaller<T>? _known;
        Type? _lastType;
        BoxedWriter? _lastWriter;

        public ValueWriter()
        {
            var type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
            if (type.IsValueType || type.IsSealed) _known = SpawnJSRuntime.Instance.GetMarshallerForWrite<T>();
        }

        public void Write(JSTape tape, T value)
        {
            if (_known != null) { _known.Write(tape, value); return; }
            if (value is null) { tape.WriteNull(); return; }
            var type = value.GetType();
            if (type != _lastType)
            {
                _lastWriter = BoxedWriter.For(type);
                _lastType = type;
            }
            _lastWriter!.Write(tape, value);
        }
    }

    /// <summary>
    /// A primitive array in one copy (<see cref="JSTape.WriteNumbers{T}"/>), for the element types a Javascript number
    /// holds. <see cref="Write"/> is null for every other element type, which is then written element by element.
    /// </summary>
    internal static class TapeNumbers<T>
    {
        public delegate void SpanWriter(JSTape tape, ReadOnlySpan<T> values);
        public static readonly SpanWriter? Write = Create();

        [UnconditionalSuppressMessage("Trimming", "IL2060", Justification = "Closes SpawnJS's own method over a primitive number type.")]
        [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "Primitive number types only.")]
        static SpanWriter? Create()
        {
            var t = typeof(T);
            var bulk = t == typeof(sbyte) || t == typeof(byte) || t == typeof(short) || t == typeof(ushort)
                || t == typeof(int) || t == typeof(uint) || t == typeof(long) || t == typeof(ulong)
                || t == typeof(float) || t == typeof(double);
            if (!bulk) return null;
            var method = typeof(TapeNumbers<T>).GetMethod(nameof(WriteNumbers), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!.MakeGenericMethod(t);
            return (SpanWriter)Delegate.CreateDelegate(typeof(SpanWriter), method);
        }
        static void WriteNumbers<TNumber>(JSTape tape, ReadOnlySpan<TNumber> values) where TNumber : unmanaged, System.Numerics.INumber<TNumber>
            => tape.WriteNumbers(values);
    }

    /// <summary>Writes a collection's elements as a Javascript array, in one copy when the elements are numbers.</summary>
    internal static class TapeCollections
    {
        public static void WriteSpan<T>(JSTape tape, ReadOnlySpan<T> items, ValueWriter<T> writer)
        {
            tape.WriteArray(items.Length);
            for (var i = 0; i < items.Length; i++) writer.Write(tape, items[i]);
        }

        public static void WriteEnumerable<T>(JSTape tape, IEnumerable<T> items, ValueWriter<T> writer)
        {
            if (items is T[] array) { Write(tape, array, writer); return; }
            if (items is List<T> list) { Write(tape, list, writer); return; }
            // the count is only known at the end
            var token = tape.BeginArray();
            var count = 0;
            foreach (var item in items)
            {
                writer.Write(tape, item);
                count++;
            }
            tape.EndArray(token, count);
        }

        public static void Write<T>(JSTape tape, T[] array, ValueWriter<T> writer)
        {
            if (TapeNumbers<T>.Write is { } numbers) numbers(tape, array);
            else WriteSpan<T>(tape, array, writer);
        }

        public static void Write<T>(JSTape tape, List<T> list, ValueWriter<T> writer)
        {
            // the list's own backing store, no copy
            ReadOnlySpan<T> span = System.Runtime.InteropServices.CollectionsMarshal.AsSpan(list);
            if (TapeNumbers<T>.Write is { } numbers) numbers(tape, span);
            else WriteSpan(tape, span, writer);
        }
    }
}
