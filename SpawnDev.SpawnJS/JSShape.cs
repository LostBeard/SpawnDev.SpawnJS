namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// The member names of an object written to a <see cref="JSTape"/> - Javascript builds the object from a shape
    /// and the member values that follow it, so the names cross once, not with every object.<br/>
    /// <br/>
    /// A shape belongs to one runtime (a marshaller creates it, and marshallers are per runtime). Its definition rides
    /// on the tape, inline, ahead of the first object that uses it, and is only CONFIRMED once a call carrying it has
    /// completed: until then every frame that uses it carries the definition again, which Javascript accepts as the
    /// same definition. A call that was abandoned before Javascript read it can therefore never leave a shape that
    /// .Net thinks Javascript knows and Javascript does not.
    /// </summary>
    public sealed class JSShape
    {
        /// <summary>The Javascript property names, in the order member values are written.</summary>
        public IReadOnlyList<string> Names { get; }
        /// <summary>Assigned by the runtime's tape on first use.</summary>
        internal int Id = -1;
        internal bool Confirmed;
        // the frame that last carried the definition, so one frame carries it once
        internal long Frame = -1;
        /// <summary>Creates a shape for objects with these property names, in this order.</summary>
        public JSShape(IReadOnlyList<string> names) => Names = names;
    }
}
