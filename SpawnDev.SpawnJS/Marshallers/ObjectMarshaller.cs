using System.Reflection;
using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls objects by property walking (clone)
    /// </summary>
    public partial class ObjectMarshaller : JSMarshaller
    {
        /// <inheritdoc/>
        public override bool CanMarshal(Type? type)
        {
            return type != null && type.IsClass && !type.IsInterface && type != typeof(string);
        }
        /// <inheritdoc/>
        public override object? JSToNet(Type type, SpawnJSHandle jsHandle)
        {
            using var jsObj = jsHandle.AsJSHandle()!;
            if (jsObj == null) return null;
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
                // ONE crossing for the whole object when the parent is slotted: members go into the frame
                // as name/value pairs and Javascript builds and assigns it in a single call. The path
                // below - create, write each member, attach, free - cost one crossing PER MEMBER plus
                // three, which counted as eight on a five member descriptor and was nearly all of its
                // cost. The marshaller was already cached per member; the boundary was the expense.
                if (TryWriteObjectInto(type, jsParent, jsKey, obj)) return;
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
