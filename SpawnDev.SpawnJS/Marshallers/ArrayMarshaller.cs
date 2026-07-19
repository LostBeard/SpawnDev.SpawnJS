using System.Collections;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls IEnumerable
    /// </summary>
    public class ArrayMarshaller : JSMarshaller
    {
        /// <inheritdoc/>
        public override bool CanMarshal(Type? type)
        {
            if (type == null) return false;
            var ret = type.IsArray && type.HasElementType;
            return ret;
        }
        /// <inheritdoc/>
        public override object? JSToNet(Type type, SpawnJSHandle jsParent, object jsKey)
        {
            using var array = jsParent.GetPropertyAsJSHandle(jsKey);
            if (array == null) return null;
            var elementType = type.GetElementType()!;
            var length = array.GetPropertyAsInt32("length");
            var retArray = (IList)Activator.CreateInstance(type, length)!;
            for (var i = 0; i < length; i++)
            {
                var value = JS.JSToNet(elementType, array, i);
                retArray[i] = value;
            }
            return retArray;
        }
        /// <inheritdoc/>
        public override void NetToJS(Type? type, SpawnJSHandle jsParent, object jsKey, object? obj)
        {
            if (obj is IList enumerable)
            {
                using var outArray = JS.NewJSArray();
                var objects = new List<object>();
                foreach (var item in enumerable)
                {
                    objects.Add(item);
                }
                for (var i = 0; i < objects.Count; i++)
                {
                    var item = objects[i];
                    JS.NetToJS(outArray, i, item);
                }
                Reflect.Set(jsParent.JSObject, jsKey, outArray.JSObject);
            }
            else
            {
                Reflect.Set(jsParent.JSObject, jsKey, (string?)null);
            }
        }
    }
}
