using System.Runtime.CompilerServices;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls ValueTuple and Tuple as a Javascript array.<br/>
    /// <br/>
    /// A tuple is the natural .Net shape for a Javascript function that returns several values as an
    /// array, or takes one - <c>[x, y]</c> reads back as <c>(int X, int Y)</c> - so the mapping is
    /// positional: element i of the array is generic argument i of the tuple, marshalled through the
    /// graph like any other value.<br/>
    /// <br/>
    /// Arity 1 through 7 for both families, which is what a tuple written inline can express before the
    /// compiler starts nesting. An 8+ element tuple is really a 7-tuple whose last element is another
    /// tuple (ValueTuple's TRest), so it would arrive in Javascript as a nested array rather than the
    /// flat one the caller wrote - a silent shape change. It is declined instead, and declining means
    /// the next marshaller reports it rather than this one producing something wrong.
    /// </summary>
    public class TupleMarshaller : JSMarshaller
    {
        static readonly HashSet<Type> SupportedGenericTypes = new()
        {
            typeof(ValueTuple<>), typeof(ValueTuple<,>), typeof(ValueTuple<,,>), typeof(ValueTuple<,,,>),
            typeof(ValueTuple<,,,,>), typeof(ValueTuple<,,,,,>), typeof(ValueTuple<,,,,,,>),
            typeof(Tuple<>), typeof(Tuple<,>), typeof(Tuple<,,>), typeof(Tuple<,,,>),
            typeof(Tuple<,,,,>), typeof(Tuple<,,,,,>), typeof(Tuple<,,,,,,>),
        };

        /// <inheritdoc/>
        public override bool CanMarshal(Type? type)
        {
            if (type == null || !type.IsGenericType) return false;
            // GetMarshaller already selects for the underlying type of a Nullable<>, but a caller may
            // reach a marshaller directly, so the unwrap is repeated rather than assumed
            var actual = Nullable.GetUnderlyingType(type) ?? type;
            if (!actual.IsGenericType) return false;
            return SupportedGenericTypes.Contains(actual.GetGenericTypeDefinition());
        }

        /// <inheritdoc/>
        public override object? JSToNet(Type type, SpawnJSHandle jsHandle)
        {
            using var array = jsHandle.AsJSHandle();
            // a null or undefined Javascript value gives null for a nullable target and the tuple's
            // default for a plain one, which for a ValueTuple is a tuple of default elements
            if (array == null) return Nullable.GetUnderlyingType(type) != null ? null : type.GetDefaultValue();

            var tupleType = Nullable.GetUnderlyingType(type) ?? type;
            var elementTypes = tupleType.GenericTypeArguments;
            var items = new object?[elementTypes.Length];
            for (var i = 0; i < elementTypes.Length; i++)
            {
                items[i] = JS.MarshallJSToNet(elementTypes[i], array, i);
            }
            // both families expose a constructor taking every element in order
            return Activator.CreateInstance(tupleType, items);
        }

        /// <inheritdoc/>
        public override void NetToJS(Type? type, SpawnJSHandle jsParent, object jsKey, object? obj)
        {
            // ITuple gives positional access without knowing which of the 14 shapes this is
            if (obj is ITuple tuple)
            {
                using var outArray = JS.NewJSArray();
                for (var i = 0; i < tuple.Length; i++)
                {
                    JS.MarshallNetToJS(outArray, i, tuple[i]);
                }
                jsParent.SetProperty(jsKey, outArray);
            }
            else
            {
                Reflect.Set(jsParent.JSObjectRequired, jsKey, (string?)null);
            }
        }
    }
}
