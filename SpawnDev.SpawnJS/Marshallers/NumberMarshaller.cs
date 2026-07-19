using System.Collections.Concurrent;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS.Marshallers
{
    namespace SpawnDev.SpawnJS
    {
        /// <summary>
        /// Marshalls Numbers (INumber included)
        /// </summary>
        public class NumberMarshaller : JSMarshaller
        {
            // Cache whether a type is a number
            private static readonly ConcurrentDictionary<Type, bool> _numericCache = new();

            // Cache the conversion delegate for O(1) performance during JSToNet
            private static readonly ConcurrentDictionary<Type, MethodInfo?> _conversionMethodCache = new();

            /// <inheritdoc/>
            public override bool CanMarshal(Type? typeToConvert)
            {
                if (typeToConvert == null) return false;

                return _numericCache.GetOrAdd(typeToConvert, type =>
                {
                    Type actualType = Nullable.GetUnderlyingType(type) ?? type;

                    if (actualType.IsGenericType && actualType.GetGenericTypeDefinition() == typeof(INumber<>))
                        return true;

                    foreach (var iface in actualType.GetInterfaces())
                    {
                        if (iface.IsGenericType && iface.GetGenericTypeDefinition() == typeof(INumber<>))
                            return true;
                    }

                    return false;
                });
            }

            /// <inheritdoc/>
            public override object? JSToNet(Type typeToConvert, SpawnJSHandle jsParent, object jsKey)
            {
                // 1. Get raw double from JavaScript via your Reflect class
                // Use Nullable read to catch 'null' or 'undefined' gracefully from JS
                double? jsValue = Reflect.GetDoubleNullable(jsParent.JSObject, jsKey);
                if (jsValue == null) return null;

                Type actualType = Nullable.GetUnderlyingType(typeToConvert) ?? typeToConvert;

                // 2. Optimization shortcut for standard types
                if (actualType == typeof(double)) return jsValue.Value;
                if (actualType == typeof(int)) return (int)jsValue.Value;
                if (actualType == typeof(float)) return (float)jsValue.Value;

                // 3. Handle custom INumber<> types (like ILGPU.Half or System.Half)
                // We fetch the static INumber<TSelf>.CreateChecked<TOther>(TOther value) method
                var createMethod = _conversionMethodCache.GetOrAdd(actualType, type =>
                {
                    // Find the INumber<> interface implemented by the type
                    // (e.g. Find INumber<Half> on Half)
                    Type? iNumberInterface = null;
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(INumber<>))
                    {
                        iNumberInterface = type;
                    }
                    else
                    {
                        iNumberInterface = Array.Find(type.GetInterfaces(),
                            i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INumber<>));
                    }

                    if (iNumberInterface == null) return null;

                    // Get the generic "CreateChecked" method from INumber<>
                    MethodInfo? baseMethod = iNumberInterface.GetMethod("CreateChecked",
                        BindingFlags.Public | BindingFlags.Static);

                    // Make it concrete for 'double' input: CreateChecked<double>(double value)
                    return baseMethod?.MakeGenericMethod(typeof(double));
                });

                if (createMethod != null)
                {
                    // Invoke static method: actualType.CreateChecked<double>(jsValue.Value)
                    return createMethod.Invoke(null, new object[] { jsValue.Value });
                }

                // Fallback for safety
                return Convert.ChangeType(jsValue.Value, actualType);
            }

            /// <inheritdoc/>
            public override void NetToJS(Type? typeToConvert, SpawnJSHandle jsParent, object jsKey, object? value)
            {
                if (value == null)
                {
                    Reflect.Set(jsParent.JSObject, jsKey, (string)null!);
                    return;
                }

                // Convert any .NET number to double via IConvertible fallback or dynamic casting
                double doubleValue;
                if (value is IConvertible convertible)
                {
                    doubleValue = convertible.ToDouble(null);
                }
                else if (value is Half systemHalf)
                {
                    doubleValue = (double)systemHalf;
                }
                else
                {
                    // Fallback using dynamic to let the JS resolve custom explicit/implicit conversion operators
                    // This perfectly captures ILGPU.Half -> double conversion
                    doubleValue = (double)(dynamic)value;
                }

                // Push to JS via your strongly typed Reflect method signature
                Reflect.Set(jsParent.JSObject, jsKey, doubleValue);
            }
        }
    }
}
