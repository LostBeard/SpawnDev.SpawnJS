using System.Collections;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls types that implement IList
    /// </summary>
    public class IListMarshaller : JSMarshaller
    {
        /// <inheritdoc/>
        public override bool CanMarshal(Type? type)
        {
            var ret = type != null && typeof(IList).IsAssignableFrom(type);
            return ret;
        }
        /// <inheritdoc/>
        public override object? JSToNet(Type type, SpawnJSHandle jsHandle)
        {
            using var array = jsHandle.AsJSHandle();
            if (array == null) return null;
            var elementType = type.GetGenericArguments()[0];
            var length = array.GetPropertyAsInt32("length");
            var retList = (IList)Activator.CreateInstance(type)!;
            for (var i = 0; i < length; i++)
            {
                var value = JS.MarshallJSToNet(elementType, array, i);
                retList.Add(value);
            }
            return retList;
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
                    JS.MarshallNetToJS(outArray, i, item);
                }
                jsParent.SetProperty(jsKey, outArray);
            }
            else
            {
                jsParent.SetProperty(jsKey, (string?)null);
            }
        }
    }
}
