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
            using var jsObj = (SpawnJSHandle)Reflect.GetJSObject(jsHandle.JSParent, jsHandle.JSKey)!;
            if (jsObj == null) return type.GetDefaultValue();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var retObj = Activator.CreateInstance(type);
            // iterate Javascript marshallable properties
            var classProps = type.GetTypeJsonProperties();
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
                var classProps = type.GetTypeJsonProperties();
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
                    JS.MarshallNetToJS(outObj, propName, propValue);
                }
                jsParent.SetProperty(jsKey, outObj);
            }
            else
            {
                Reflect.Set(jsParent.JSObjectRequired, jsKey, (string)null!);
            }
        }
    }
}
