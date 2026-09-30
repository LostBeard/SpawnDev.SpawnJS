using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    public class HeapViewDescriptorMarshaller : JSMarshallerFromString<HeapViewDescriptor>
    {
        public override HeapViewDescriptor JSToNet(string value)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc/>
        public override void Write(JSTape tape, HeapViewDescriptor value) => tape.WriteHeapView(value);
    }
}
