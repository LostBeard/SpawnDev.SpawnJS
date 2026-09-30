using SpawnDev.SpawnJS.Marshaller;
using System.Runtime.InteropServices;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// The call tape. A .Net to Javascript call is written here, in .Net memory, and then crosses the boundary
    /// ONCE: Javascript reads the whole call - target, method, every argument - straight out of the WASM heap,
    /// runs it, and writes the result back onto the tape. Writing is plain array stores; nothing crosses until
    /// the call itself.<br/>
    /// <br/>
    /// One tape per <see cref="SpawnJSRuntime"/>, so one per .Net instance. Javascript never looks the tape up:
    /// every call passes its dotnetId and the address of its own frame, so several .Net runtimes on one page
    /// each use their own tape and nothing is shared through a global.<br/>
    /// <br/>
    /// Layout. Everything is a cell of 8 bytes, so every value is 8 byte aligned and Javascript reads it through
    /// a Float64Array over the heap. A frame is a header followed by one tagged value per argument:
    /// <code>
    /// header:  methodIndex | returnType | argCount | scratchId
    /// value:   tag [payload cells]      (tags below; must match SpawnJSInterop.TapeTag)
    /// string:  TagString | length | UTF-16 code units, 4 per cell
    /// object:  [TagShape | id | count | names]  TagObject | id | one value per name (TagAbsent = not assigned)
    /// array:   TagArray | count | values          numbers: TagNumbers | kind | count | raw, padded to a cell
    /// </code>
    /// Frames form a stack. A call can re-enter .Net (a callback) which calls Javascript again, and a
    /// marshaller writing an argument can itself make an interop call, so a nested frame always starts where
    /// the outer frame's writing has got to. Growing never moves a segment an open frame lives in: it moves the
    /// innermost frame to a bigger segment, and outer frames keep theirs.
    /// </summary>
    public sealed class JSTape
    {
        // value tags - SpawnJSInterop.TapeTag in SpawnDev.SpawnJS.lib.module.js must match
        internal const double TagUndefined = 0;
        internal const double TagNull = 1;
        internal const double TagNumber = 2;
        internal const double TagBoolean = 3;
        internal const double TagString = 4;
        internal const double TagRef = 5;
        internal const double TagCallback = 6;
        // TRANSITIONAL: the value was built by a marshaller's per-value NetToJS into the call's scratch array.
        // Goes away with JSMarshaller.Write's default, once every marshaller writes the tape itself.
        internal const double TagScratch = 7;
        // a member skipped by [JsonIgnore(WhenWritingNull/WhenWritingDefault)] - the property is not assigned
        internal const double TagAbsent = 8;
        // shapeId, then one value per shape member
        internal const double TagObject = 9;
        // shapeId, name count, names (length + UTF-16) - a JSShape's definition, ahead of its first object
        internal const double TagShape = 10;
        // count, then that many values
        internal const double TagArray = 11;
        // kind, count, then the raw numbers - a primitive array in one copy. Javascript builds a plain Array.
        internal const double TagNumbers = 12;

        const int HeaderCells = 4;
        const int InitialSegmentCells = 8 * 1024;

        sealed class Segment : IDisposable
        {
            public readonly double[] Cells;
            GCHandle _handle;
            // pinned for the life of the segment, so an address handed to Javascript can never go stale
            public readonly nint Address;
            public Segment(int cells)
            {
                Cells = new double[cells];
                _handle = GCHandle.Alloc(Cells, GCHandleType.Pinned);
                Address = _handle.AddrOfPinnedObject();
            }
            public void Dispose() { if (_handle.IsAllocated) _handle.Free(); }
        }

        struct Frame
        {
            public int SegmentIndex;
            public int Start;
            // where the enclosing frame's writing had got to; restored when this frame ends
            public int OuterSegmentIndex;
            public int OuterPosition;
            public SpawnJSObjectReference? Scratch;
            public int ScratchCount;
            // unique per frame; a shape carries its definition once per frame
            public long Serial;
            // shapes whose definitions this frame carries - confirmed if the call completes
            public List<JSShape>? Defined;
        }

        readonly SpawnJSRuntime _js;
        // index = nesting order; a segment above the innermost frame's is free and reused
        readonly List<Segment> _segments = new List<Segment>();
        Frame[] _frames = new Frame[8];
        int _depth;
        int _segmentIndex;
        int _position;
        long _frameSerial;
        int _nextShapeId;

        internal JSTape(SpawnJSRuntime js)
        {
            _js = js;
            _segments.Add(new Segment(InitialSegmentCells));
        }

        Segment Current => _segments[_segmentIndex];

        /// <summary>Open call frames. 0 between calls; a count that never returns to 0 is a leaked frame.</summary>
        public int Depth => _depth;

        #region Writing - the marshaller API
        /// <summary>Writes JS undefined.</summary>
        public void WriteUndefined() { EnsureCells(1); Current.Cells[_position++] = TagUndefined; }
        /// <summary>Writes JS null.</summary>
        public void WriteNull() { EnsureCells(1); Current.Cells[_position++] = TagNull; }
        /// <summary>Writes a JS number.</summary>
        public void WriteNumber(double value)
        {
            EnsureCells(2);
            var cells = Current.Cells;
            cells[_position] = TagNumber;
            cells[_position + 1] = value;
            _position += 2;
        }
        /// <summary>Writes a JS boolean.</summary>
        public void WriteBoolean(bool value)
        {
            EnsureCells(2);
            var cells = Current.Cells;
            cells[_position] = TagBoolean;
            cells[_position + 1] = value ? 1 : 0;
            _position += 2;
        }
        /// <summary>Writes a JS string, or JS null for a null string. The UTF-16 code units are copied as they are,
        /// so a lone surrogate arrives intact.</summary>
        public void WriteString(string? value)
        {
            if (value == null) { WriteNull(); return; }
            var charCells = (value.Length + 3) >> 2;
            EnsureCells(2 + charCells);
            var cells = Current.Cells;
            cells[_position] = TagString;
            cells[_position + 1] = value.Length;
            value.AsSpan().CopyTo(MemoryMarshal.Cast<double, char>(cells.AsSpan(_position + 2, charCells)));
            _position += 2 + charCells;
        }
        /// <summary>Writes the Javascript value held under a SpawnJSObjectReference id. The sentinel ids work as
        /// they do everywhere else (-2 undefined, -3 null, -1 globalThis).</summary>
        public void WriteRef(double sjsId)
        {
            EnsureCells(2);
            var cells = Current.Cells;
            cells[_position] = TagRef;
            cells[_position + 1] = sjsId;
            _position += 2;
        }
        /// <summary>Writes the Javascript function that invokes <paramref name="callback"/>. A null callback writes
        /// undefined, as v2 did.</summary>
        public void WriteCallback(Callback? callback)
        {
            if (callback == null) { WriteUndefined(); return; }
            callback.Sent = true;
            EnsureCells(3);
            var cells = Current.Cells;
            cells[_position] = TagCallback;
            cells[_position + 1] = callback.Id;
            cells[_position + 2] = callback.Once ? 1 : 0;
            _position += 3;
        }
        /// <summary>A member that is not written: Javascript does not assign the property at all.</summary>
        public void WriteAbsent() { EnsureCells(1); Current.Cells[_position++] = TagAbsent; }

        /// <summary>
        /// Starts an object of <paramref name="shape"/>. Exactly one value per shape member must follow, in order -
        /// <see cref="WriteAbsent"/> for a member that is not written.
        /// </summary>
        public void WriteObject(JSShape shape)
        {
            if (shape.Id < 0) shape.Id = _nextShapeId++;
            var serial = _frames[_depth - 1].Serial;
            if (!shape.Confirmed && shape.Frame != serial)
            {
                var names = shape.Names;
                EnsureCells(3);
                var cells = Current.Cells;
                cells[_position] = TagShape;
                cells[_position + 1] = shape.Id;
                cells[_position + 2] = names.Count;
                _position += 3;
                for (var i = 0; i < names.Count; i++) WriteChars(names[i]);
                shape.Frame = serial;
                (_frames[_depth - 1].Defined ??= new List<JSShape>()).Add(shape);
            }
            EnsureCells(2);
            Current.Cells[_position] = TagObject;
            Current.Cells[_position + 1] = shape.Id;
            _position += 2;
        }

        /// <summary>
        /// Writes a value whose type is only known at run time, through that type's own marshaller - what a member or
        /// argument declared object, an interface or a base class does.
        /// </summary>
        public void WriteValue(object? value)
        {
            if (value == null) { WriteNull(); return; }
            Marshaller.BoxedWriter.For(value.GetType()).Write(this, value);
        }

        /// <summary>
        /// Starts an array whose length is only known once its values are written (an IEnumerable). Pass the token to
        /// <see cref="EndArray"/> with the count. The token is an offset within the frame, so it survives the frame
        /// moving to a bigger segment while the values are written.
        /// </summary>
        public int BeginArray()
        {
            EnsureCells(2);
            var cells = Current.Cells;
            cells[_position] = TagArray;
            cells[_position + 1] = 0;
            var token = _position + 1 - _frames[_depth - 1].Start;
            _position += 2;
            return token;
        }
        /// <summary>Sets the count of an array started with <see cref="BeginArray"/>.</summary>
        public void EndArray(int token, int count)
        {
            ref var frame = ref _frames[_depth - 1];
            _segments[frame.SegmentIndex].Cells[frame.Start + token] = count;
        }

        /// <summary>Starts an array. Exactly <paramref name="count"/> values must follow.</summary>
        public void WriteArray(int count)
        {
            EnsureCells(2);
            var cells = Current.Cells;
            cells[_position] = TagArray;
            cells[_position + 1] = count;
            _position += 2;
        }

        /// <summary>
        /// Writes a whole array of numbers in one copy; Javascript receives a plain Array of numbers, exactly as it
        /// did element by element. Types a Javascript number cannot view directly (long, ulong, decimal ...) cross
        /// as float64, which is what each element became anyway.
        /// </summary>
        public void WriteNumbers<T>(ReadOnlySpan<T> values) where T : unmanaged, System.Numerics.INumber<T>
        {
            var kind = NumberKind<T>.Value;
            var count = values.Length;
            if (kind < 0)
            {
                // widen to float64 as it is copied
                EnsureCells(3 + count);
                var cells = Current.Cells;
                cells[_position] = TagNumbers;
                cells[_position + 1] = NumberKindFloat64;
                cells[_position + 2] = count;
                for (var i = 0; i < count; i++) cells[_position + 3 + i] = double.CreateChecked(values[i]);
                _position += 3 + count;
                return;
            }
            var bytes = MemoryMarshal.AsBytes(values);
            var dataCells = (bytes.Length + 7) >> 3;
            EnsureCells(3 + dataCells);
            var target = Current.Cells;
            target[_position] = TagNumbers;
            target[_position + 1] = kind;
            target[_position + 2] = count;
            bytes.CopyTo(MemoryMarshal.AsBytes(target.AsSpan(_position + 3, dataCells)));
            _position += 3 + dataCells;
        }
        // SpawnJSInterop.TapeNumberCtors index; -1 = widen to float64
        const int NumberKindFloat64 = 7;
        static class NumberKind<T>
        {
            public static readonly int Value =
                typeof(T) == typeof(sbyte) ? 0 : typeof(T) == typeof(byte) ? 1 :
                typeof(T) == typeof(short) ? 2 : typeof(T) == typeof(ushort) ? 3 :
                typeof(T) == typeof(int) ? 4 : typeof(T) == typeof(uint) ? 5 :
                typeof(T) == typeof(float) ? 6 : typeof(T) == typeof(double) ? NumberKindFloat64 : -1;
        }

        // length and UTF-16 code units, no tag (a shape's names)
        void WriteChars(string value)
        {
            var charCells = (value.Length + 3) >> 2;
            EnsureCells(1 + charCells);
            var cells = Current.Cells;
            cells[_position] = value.Length;
            value.AsSpan().CopyTo(MemoryMarshal.Cast<double, char>(cells.AsSpan(_position + 1, charCells)));
            _position += 1 + charCells;
        }

        /// <summary>
        /// TRANSITIONAL - the seam for marshallers that do not write the tape yet (see
        /// <see cref="JSMarshaller{TType}.Write"/>). The value is built the v2 way, by the marshaller's own NetToJS,
        /// into this call's scratch array, and the tape carries its index. Deleted with that default.
        /// </summary>
        internal void WriteViaScratch<T>(JSMarshaller<T> marshaller, T value)
        {
            ref var frame = ref _frames[_depth - 1];
            frame.Scratch ??= _js.RentScratchArray();
            var index = frame.ScratchCount++;
            marshaller.NetToJS(frame.Scratch, index, value);
            EnsureCells(2);
            var cells = Current.Cells;
            cells[_position] = TagScratch;
            cells[_position + 1] = index;
            _position += 2;
        }
        #endregion

        #region Frames
        /// <summary>
        /// Opens a call frame at the current write position and writes its header. Every BeginCall is matched by
        /// exactly one <see cref="EndCall"/>, including when writing an argument throws.
        /// </summary>
        internal void BeginCall(int methodIndex, int argCount)
        {
            if (_depth == _frames.Length) System.Array.Resize(ref _frames, _frames.Length * 2);
            var outerSegmentIndex = _segmentIndex;
            var outerPosition = _position;
            // A new frame never moves an existing one: an outer frame may already be in Javascript's hands (this
            // call is re-entrant) and its result will be written at the address it was sent with. Out of room,
            // the new frame starts in a free segment above instead.
            if (_position + HeaderCells > Current.Cells.Length)
            {
                _segmentIndex = FreeSegmentIndex(_segmentIndex + 1, InitialSegmentCells);
                _position = 0;
            }
            _frames[_depth++] = new Frame
            {
                SegmentIndex = _segmentIndex,
                Start = _position,
                OuterSegmentIndex = outerSegmentIndex,
                OuterPosition = outerPosition,
                Serial = ++_frameSerial,
            };
            var cells = Current.Cells;
            cells[_position] = methodIndex;
            cells[_position + 1] = 0;   // returnType, filled by Send
            cells[_position + 2] = argCount;
            cells[_position + 3] = SpawnJSObjectReference.UndefinedId;   // scratchId, filled by Send
            _position += HeaderCells;
        }

        /// <summary>
        /// Finishes the innermost frame's header and hands its location to Javascript. The frame stays open, and its
        /// cells stay put, until <see cref="EndCall"/> - the result is written back over it.
        /// </summary>
        internal (double Address, int Length, int Capacity) Send(ReturnType returnType, out double scratchId)
        {
            ref var frame = ref _frames[_depth - 1];
            var segment = _segments[frame.SegmentIndex];
            segment.Cells[frame.Start + 1] = (int)returnType;
            scratchId = frame.Scratch?.Id ?? SpawnJSObjectReference.UndefinedId;
            segment.Cells[frame.Start + 3] = scratchId;
            var address = (double)(segment.Address + frame.Start * 8);
            var length = (_position - frame.Start) * 8;
            var capacity = (segment.Cells.Length - frame.Start) * 8;
            return (address, length, capacity);
        }

        /// <summary>
        /// The result area of the innermost frame, which Javascript wrote over the frame's own cells.
        /// </summary>
        internal ReadOnlySpan<double> Result(int length)
        {
            ref var frame = ref _frames[_depth - 1];
            return _segments[frame.SegmentIndex].Cells.AsSpan(frame.Start, (length + 7) >> 3);
        }

        /// <summary>
        /// A result too big for the frame's segment: a free segment of at least <paramref name="bytes"/> bytes
        /// above every open frame, for Javascript to write the result into.
        /// </summary>
        internal (double Address, int Capacity, double[] Cells) OverflowArea(int bytes)
        {
            var index = FreeSegmentIndex(Math.Max(_segmentIndex, _frames[_depth - 1].SegmentIndex) + 1, (bytes + 7) >> 3);
            var segment = _segments[index];
            return ((double)segment.Address, segment.Cells.Length * 8, segment.Cells);
        }

        /// <summary>
        /// Javascript has read the innermost frame: the shape definitions it carried are known there now. Not called
        /// for a call that failed before or while its frame was read, so those definitions are sent again.
        /// </summary>
        internal void ConfirmShapes()
        {
            var defined = _frames[_depth - 1].Defined;
            if (defined == null) return;
            for (var i = 0; i < defined.Count; i++) defined[i].Confirmed = true;
        }

        /// <summary>Closes the innermost frame, returns its scratch array, and restores the enclosing frame's position.</summary>
        internal void EndCall()
        {
            ref var frame = ref _frames[_depth - 1];
            if (frame.Scratch != null) _js.ReturnScratchArray(frame.Scratch);
            _segmentIndex = frame.OuterSegmentIndex;
            _position = frame.OuterPosition;
            frame = default;
            _depth--;
        }
        #endregion

        /// <summary>
        /// Makes room for <paramref name="cells"/> more cells in the innermost frame, which is the one being written and
        /// has not been sent yet. When its segment is full the frame moves - header and everything written so far - to
        /// a bigger free segment above. Segments below hold outer frames and are never touched.
        /// </summary>
        void EnsureCells(int cells)
        {
            if (_position + cells <= Current.Cells.Length) return;
            ref var frame = ref _frames[_depth - 1];
            var used = _position - frame.Start;
            var index = FreeSegmentIndex(_segmentIndex + 1, used + cells);
            System.Array.Copy(Current.Cells, frame.Start, _segments[index].Cells, 0, used);
            frame.SegmentIndex = index;
            frame.Start = 0;
            _segmentIndex = index;
            _position = used;
        }

        /// <summary>A segment at <paramref name="index"/> with at least <paramref name="cells"/> cells, replacing a smaller free one.</summary>
        int FreeSegmentIndex(int index, int cells)
        {
            if (index < _segments.Count)
            {
                if (_segments[index].Cells.Length >= cells) return index;
                _segments[index].Dispose();
                _segments[index] = new Segment(Math.Max(cells, _segments[index].Cells.Length * 2));
                return index;
            }
            _segments.Add(new Segment(Math.Max(cells, InitialSegmentCells)));
            return _segments.Count - 1;
        }
    }
}
