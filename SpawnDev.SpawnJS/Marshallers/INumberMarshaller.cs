using System.Diagnostics.CodeAnalysis;
using SpawnDev.SpawnJS.Marshaller;
using System.Numerics;

namespace SpawnDev.SpawnJS.Marshallers
{
    public class INumberMarshaller<TNumber> : JSMarshallerFromDouble<TNumber> where TNumber : INumber<TNumber>
    {
        [System.Diagnostics.CodeAnalysis.UnconditionalSuppressMessage("Trimming", "IL2070",
            Justification = "Probes whether a type implements INumber<>. INumber<> is referenced by this marshaller, so the trimmer preserves that interface implementation on any numeric type it keeps; a type whose INumber<> interface was trimmed would be an unused number type that never reaches this check.")]
        public override bool CanMarshal(Type type)
        {
            if (type.IsGenericTypeDefinition) return false;
            return type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INumber<>));
        }

        public override JSMarshaller<T> GetMarshaller<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        {
            if (this is JSMarshaller<T> _this) return _this;
            var typeT = typeof(T);
            var marshallerTyped = typeof(INumberMarshaller<>).MakeGenericType(typeT);
            return (JSMarshaller<T>)Activator.CreateInstance(marshallerTyped)!;
        }

        public override TNumber JSToNet(double value)
        {
            return TNumber.CreateChecked(value);
        }

        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, TNumber value)
        {
            double doubleValue = double.CreateChecked(value);
            jsParent.PropertySet(jsKey, doubleValue);
        }

        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, TNumber value)
        {
            double doubleValue = double.CreateChecked(value);
            jsParent.PropertySet(jsKey, doubleValue);
        }
    }
}