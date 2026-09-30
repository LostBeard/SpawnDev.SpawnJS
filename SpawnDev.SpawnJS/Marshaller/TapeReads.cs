using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SpawnDev.SpawnJS.Marshaller
{
    /// <summary>Reads a collection's elements off the tape - in one copy when every element is a number.</summary>
    internal static class TapeReads
    {
        /// <summary>
        /// The schema a collection of <paramref name="element"/> is read with: Numbers - one copy - when each element
        /// is the built-in Int32 or Double (numbers, enums), an Array of the element schema otherwise.
        /// </summary>
        public static JSSchema CollectionSchema(JSSchema element)
            => element.Kind == JSSchemaKind.Int32 || element.Kind == JSSchemaKind.Double
                ? JSSchema.Numbers(element.Kind)
                : JSSchema.Array(element);

        /// <summary>
        /// Reads <paramref name="count"/> elements (after <see cref="JSTapeReader.ReadCount"/>) straight into
        /// <paramref name="destination"/> - an array's or a list's own memory.
        /// </summary>
        public static void ReadElements<T>(ref JSTapeReader reader, JSSchema schema, JSMarshaller<T> element, Span<T> destination)
        {
            var count = destination.Length;
            if (schema.Kind != JSSchemaKind.Numbers)
            {
                for (var i = 0; i < count; i++) destination[i] = element.Read(ref reader);
                return;
            }
            if (schema.NumberKind == JSSchemaKind.Int32)
            {
                var raw = reader.ReadRaw<int>(count);
                if (typeof(T) == typeof(int)) raw.CopyTo(MemoryMarshal.CreateSpan(ref Unsafe.As<T, int>(ref MemoryMarshal.GetReference(destination)), count));
                else for (var i = 0; i < count; i++) destination[i] = element.JSToNet(raw[i]);
            }
            else
            {
                var raw = reader.ReadRaw<double>(count);
                if (typeof(T) == typeof(double)) raw.CopyTo(MemoryMarshal.CreateSpan(ref Unsafe.As<T, double>(ref MemoryMarshal.GetReference(destination)), count));
                else for (var i = 0; i < count; i++) destination[i] = element.JSToNet(raw[i]);
            }
        }

        /// <summary>A list of <paramref name="count"/> elements, read into its own backing store.</summary>
        public static List<T> ReadList<T>(ref JSTapeReader reader, JSSchema schema, JSMarshaller<T> element, int count)
        {
            var list = new List<T>(count);
            CollectionsMarshal.SetCount(list, count);
            ReadElements(ref reader, schema, element, CollectionsMarshal.AsSpan(list));
            return list;
        }
    }
}
