using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>Marshals a plain <see cref="object"/> (used for the null case): reads back as null, writes JS null.</summary>
    public class ObjectMarshaller : JSMarshallerFromSpawnJSObjectReference<object>
    {
        public override object JSToNet(SpawnJSObjectReference value) => value;
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, object value)
        {
            if (value == null)
            {
                jsParent.PropertySetNull(jsKey);
                return;
            }
            var valueType = value.GetType();
            if (valueType == typeof(object))
            {
                throw new NotImplementedException("TODO");
            }
            else
            {
                ((Delegate)writeTyped<object>).InvokeGeneric(valueType, value);
                void writeTyped<T>(T value)
                {
                    var marshaller = JS.GetMarshallerForWrite<T>();
                    marshaller.NetToJS(jsParent, jsKey, value);
                }
            }
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, object value)
        {
            if (value == null)
            {
                jsParent.PropertySetNull(jsKey);
                return;
            }
            var valueType = value.GetType();
            if (valueType == typeof(object))
            {
                throw new NotImplementedException("TODO");
            }
            else
            {
                ((Delegate)writeTyped<object>).InvokeGeneric(valueType, value);
                void writeTyped<T>(T value)
                {
                    var marshaller = JS.GetMarshallerForWrite<T>();
                    marshaller.NetToJS(jsParent, jsKey, value);
                }
            }
        }
    }
}
