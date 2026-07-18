using System.Collections;
using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls IEnumerable
    /// </summary>
    public class IEnumerableMarshaller : JSMarshaller
    {
        /// <inheritdoc/>
        public override bool CanMarshal(Type? type)
        {
            if (type == null) return false;
            var ret = type.IsArray && type.HasElementType;
            return ret;
        }
        /// <inheritdoc/>
        public override object? JSToNet(Type type, JSObject jsParent, object jsKey, SpawnJSRuntime runtime)
        {
            if (type.IsArray && type.HasElementType)
            {
                var elementType = type.GetElementType()!;
                var array = Reflect.GetJSObject(jsParent, jsKey);
                if (array == null) return null;
                var length = Reflect.GetInt32(array, "length");
                var retArray = Array.CreateInstance(elementType, length);
                for (var i = 0; i < length; i++)
                {
                    var value = runtime.JSToNet(elementType, array, i);
                    retArray.SetValue(value, i);
                }
                return retArray;
            }
            else
            {
                throw new NotImplementedException($"{nameof(IEnumerableMarshaller)}.JSToNet type not supported !!!!");
            }
        }
        /// <inheritdoc/>
        public override void NetToJS(Type? type, JSObject jsParent, object jsKey, object? obj, SpawnJSRuntime runtime)
        {
            if (obj is IEnumerable enumerable)
            {
                var outArray = runtime.NewJSArray();
                var objects = new List<object>();
                foreach (var item in enumerable)
                {
                    objects.Add(item);
                }
                for (var i = 0; i < objects.Count; i++)
                {
                    var item = objects[i];
                    runtime.NetToJS(outArray, i, item);
                }
                Reflect.SetObject(jsParent, jsKey, outArray);
            }
        }
    }
}
