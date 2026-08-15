using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>Marshals <see cref="VoidType"/>: writes nothing and reads back null. Used for void calls.</summary>
    public class VoidTypeMarshaller : JSMarshallerFromVoid<VoidType>
    {
        public override VoidType JSToNet() => null!;
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, VoidType value) { }
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, VoidType value) { }
    }
}
