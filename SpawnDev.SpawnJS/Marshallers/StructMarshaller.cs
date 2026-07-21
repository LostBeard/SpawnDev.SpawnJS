using System.Reflection;
using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls structs by property walking (clone)
    /// </summary>
    public class StructMarshaller : JSMarshaller
    {
        /// <inheritdoc/>
        public override bool CanMarshal(Type? type)
        {
            return type != null && type.IsValueType && !type.IsEnum && !type.IsPrimitive;
        }
        /// <inheritdoc/>
        public override object? JSToNet(Type type, SpawnJSHandle jsHandle)
        {
            // The declared type reaching a read may be Nullable<T>; the members to walk are T's. Walking
            // Nullable<T> itself would read HasValue and Value as if they were Javascript properties.
            var underlying = Nullable.GetUnderlyingType(type);
            var structType = underlying ?? type;
            using var jsObj = jsHandle.AsJSHandle()!;
            // absent on the Javascript side is null for a nullable target, and the default value for a
            // plain struct, which has no way to represent absence
            if (jsObj == null) return underlying != null ? null : structType.GetDefaultValue();
            var retObj = Activator.CreateInstance(structType);
            // iterate Javascript marshallable properties
            var classProps = structType.GetTypeJsonProperties();
            foreach (var prop in classProps)
            {
                var propertyInfo = prop.PropertyInfo;
                var fieldInfo = prop.FieldInfo;
                var pType = propertyInfo?.PropertyType ?? fieldInfo!.FieldType;
                var propName = prop.GetJsonName();
                object? value;
                try
                {
                    value = JS.MarshallJSToNet(pType, jsObj, propName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{nameof(ObjectMarshaller)} import failed: {ex.ToString()}");
                    continue;
                }
                if (value == null) continue;
                if (propertyInfo != null)
                {
                    if (propertyInfo.SetMethod != null)
                    {
                        propertyInfo.SetValue(retObj, value);
                    }
                }
                else if (fieldInfo != null)
                {
                    fieldInfo.SetValue(retObj, value);
                }
            }
            return retObj;
        }
        /// <inheritdoc/>
        public override void NetToJS(Type? type, SpawnJSHandle jsParent, object jsKey, object? obj)
        {
            if (type != null && obj != null)
            {
                using var outObj = JS.NewJSObject()!;
                // a boxed Nullable<T> already reports T here, but the declared type can still arrive on a
                // path that types the write from the declaration rather than the value
                var classProps = (Nullable.GetUnderlyingType(type) ?? type).GetTypeJsonProperties();
                foreach (var prop in classProps)
                {
                    var propertyInfo = prop.PropertyInfo;
                    var fieldInfo = prop.FieldInfo;
                    var propName = prop.GetJsonName();
                    object? propValue = null;
                    if (propertyInfo != null)
                    {
                        propValue = propertyInfo.GetValue(obj);
                    }
                    else if (fieldInfo != null)
                    {
                        propValue = fieldInfo.GetValue(obj);
                    }
                    if (!prop.GetShouldWrite(propValue)) continue;
                    WriteMember(prop, outObj, propName, propValue);
                }
                jsParent.SetProperty(jsKey, outObj);
            }
            else
            {
                jsParent.SetProperty(jsKey, (string?)null);
            }
        }
    }
}
