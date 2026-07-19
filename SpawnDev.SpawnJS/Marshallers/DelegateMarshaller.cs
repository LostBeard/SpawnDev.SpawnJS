using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls Delegate types<br/>
    /// 
    /// </summary>
    public class DelegateMarshaller : JSMarshaller
    {

        ///<inheritdoc/>
        public override bool CanMarshal(Type? typeToConvert)
        {
            if (JS.Initializing)
            {
                // SpawnJSRuntime registers its JS to .Net callback during initialization and
                // this marshaller needs taht working before it can handle more advanced callbacks.
                // So this marshaller needs to ignore delegate types during initialization to
                // allow that setup to occur.
                return false;
            }
            return typeToConvert != null && typeToConvert.IsAssignableTo(typeof(Delegate));
        }
        /// <inheritdoc/>
        public override object? JSToNet(Type typeToConvert, SpawnJSHandle jsParent, object jsKey)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc/>
        public override void NetToJS(Type? typeToConvert, SpawnJSHandle jsParent, object jsKey, object? value)
        {
            throw new NotImplementedException();
        }
    }
}
