/* By Todd Tanner aka github.com/LostBeard 2026 */

(function () {
    if (globalThis.SpawnJSInterop) return;

    class SpawnJSInterop {
        static instances = {};
        static _idNext = 0;
        verbose = false;
        id = '';
        dotnetRuntime = null;
        // Per-runtime state. Every one of these is instance scoped ON PURPOSE: two .Net apps can share a
        // page - a custom element built on SpawnJS dropped onto a page that already runs one - and they
        // must not be able to reach each other's memory.
        ctxId = 0;
        argFrameAddress = 0;
        probeFrameAddress = 0;
        argBufferAddress = 0;
        constructor(dotnetRuntime) {
            this.dotnetRuntime = dotnetRuntime;
            this.ctxId = ++SpawnJSInterop._idNext;
            this.id = `_${this.ctxId}`;
            SpawnJSInterop.instances[this.id] = this;
            // The slot helpers are plain globals because JSImport binds to a fixed name and has no
            // instance to reach through. They therefore take a CONTEXT ID as their first argument and
            // resolve the instance here - which is how per-runtime state stays per-runtime despite the
            // functions being shared.
            SpawnJSInterop.byCtx[this.ctxId] = this;
        }
        // ctx id -> instance. The slot TABLE is deliberately not in here: slot ids come from one
        // monotonic counter and are never reused, so two runtimes can share it without being able to
        // touch each other's entries, and keeping it shared costs the hot path nothing.
        static byCtx = {};
        static ctx(id) {
            var it = SpawnJSInterop.byCtx[id];
            if (!it) throw new Error(`SpawnJSInterop: no runtime registered for context ${id}`);
            return it;
        }
        _in(key, obj) {
            if (obj === null || obj === void 0) return false;
            try {
                return key in Object(obj);
            } catch { }
            return false;
        }
        pathObjectInfo(rootObject, path) {
            if (rootObject === null || rootObject === void 0) {
                // callers must call with the globalThis if they wish to use it as the rootObject.
                throw new DOMException('spawnJSInterop.pathObjectInfo error: rootObject cannot be null');
            }
            var parent = rootObject;
            var target;
            var propertyName;
            var shortCircuit = false;
            if (typeof path === 'string' && !(this._in(path, parent))) {
                var parts = path.split('.');
                propertyName = parts[parts.length - 1];
                var part;
                for (var i = 0; i < parts.length - 1; i++) {
                    part = parts[i];
                    if (part[part.length - 1] === '?') {
                        // ? null conditonal found
                        // if parent does not exist allow undefined/null parent instead of throwing exception
                        part = part.substring(0, part.length - 1);
                        parent = parent[part];
                        if (parent === void 0 || parent === null) {
                            shortCircuit = true;
                            break;
                        }
                    }
                    else {
                        parent = parent[part];
                    }
                }
                if (!shortCircuit) {
                    target = parent[propertyName];
                }
            }
            else {
                propertyName = path;
                target = parent[propertyName];
            }
            return {
                shortCircuit,   // bool - true if the pathfinding short circuited due to a null-conditional
                parent,         // any - only null or undefined if short circuited due to a null-conditional
                propertyName,   // any
                target,         // any
            };
        }
        // Existence check that resolves dotted paths and null-conditionals exactly the way
        // getProperty/setProperty/invokeProperty do, so `Has` agrees with `Get`.
        // `_in` deliberately stays literal - it is meant to be the `in` operator, and teaching it
        // about dotted paths would break that contract.
        hasProperty(/* object */ target, /* any */ identifier, /* bool */ useIn) {
            var pathInfo;
            try {
                pathInfo = this.pathObjectInfo(target, identifier);
            } catch {
                // an ancestor in the path is missing, so the property cannot exist. An existence
                // check answers false here rather than throwing the way a read would.
                return false;
            }
            if (pathInfo.shortCircuit) return false;
            if (pathInfo.parent === null || pathInfo.parent === void 0) return false;
            return useIn
                ? this._in(pathInfo.propertyName, pathInfo.parent)
                : this.hasOwnPropertySafe(pathInfo.parent, pathInfo.propertyName);
        }
        hasOwnPropertySafe(obj, key) {
            if (obj === null || obj === undefined) return false;
            try {
                return typeof obj === 'object' ? obj.hasOwnProperty(key) : Object(obj).hasOwnProperty(key);
            } catch { }
            return false;
        }
        setProperty(/* object */ target, /* any */ identifier, /* any */ value) {
            var pathInfo = this.pathObjectInfo(target, identifier);
            if (pathInfo.shortCircuit) return;
            pathInfo.parent[pathInfo.propertyName] = value;
        }
        deleteProperty(/* object */ target, /* any */ identifier) {
            var pathInfo = this.pathObjectInfo(target, identifier);
            if (pathInfo.shortCircuit) return undefined;
            delete pathInfo.parent[pathInfo.propertyName];
        }
        getProperty(/* object */ target, /* any */ identifier) {
            var pathInfo = this.pathObjectInfo(target, identifier);
            if (pathInfo.shortCircuit) return undefined;
            if (typeof pathInfo.target === 'function') {
                return pathInfo.target.bind(pathInfo.parent);
            }
            return pathInfo.target;
        }
        invokeProperty(/* object */ target, /* any */ identifier, /* any */ args) {
            var pathInfo = this.pathObjectInfo(target, identifier);
            if (pathInfo.shortCircuit) return undefined;
            return pathInfo.target.apply(pathInfo.parent, args);
        }
        invokePropertyConstructor(/* object */ target, /* any */ identifier, /* any */ args) {
            var pathInfo = this.pathObjectInfo(target, identifier);
            if (pathInfo.shortCircuit) return undefined;
            return new pathInfo.target(...args);
        }
        newEasyPromise() {
            var _resolve = null;
            var _reject = null;
            var promise = new Promise((resolve, reject) => {
                _resolve = resolve;
                _reject = reject;
            });
            promise.resolve = _resolve;
            promise.reject = _reject;
            return promise
        }
        copyProperty(srcObj, srcKey, destObj, destKey) {
            destObj[destKey] = srcObj[srcKey];
        }
        moveProperty(srcObj, srcKey, destObj, destKey) {
            destObj[destKey] = srcObj[srcKey];
            delete srcObj[srcKey];
        }
        returnMe(/* any */ value) {
            return value;
        }
        // Builds a TypedArray or DataView directly over the .Net heap - a real Javascript view, not a copy.
        // SpawnDev.BlazorJS achieved this by JSON serializing a {_heapViewInfo:{...}} descriptor and having
        // a Javascript hook recognise that property name. There is no descriptor here: the address and
        // length come straight across as numbers and the view is constructed from them, so the zero copy
        // path costs no serialization at all.
        // viewType is a global constructor name: Uint8Array, Float32Array, DataView, and so on.
        heapView(viewType, address, byteLength) {
            var ctor = globalThis[viewType];
            if (typeof ctor !== 'function') throw new Error(`SpawnJSInterop: '${viewType}' is not a constructor`);
            var buffer = this.wasmMemoryBuffer();
            if (address < 0 || address + byteLength > buffer.byteLength) {
                throw new RangeError(`SpawnJSInterop: heap view [${address}, ${address + byteLength}) is outside the ${buffer.byteLength} byte heap`);
            }
            // DataView is constructed in bytes; a TypedArray takes an ELEMENT count, so the byte length has
            // to be divided by the target's element size. Sizing a cross type view by the source's element
            // size instead is a real bug that has been shipped before - it builds an oversized view and
            // throws RangeError only when the tail is touched.
            if (viewType === 'DataView') return new ctor(buffer, address, byteLength);
            var elementSize = ctor.BYTES_PER_ELEMENT;
            if (!elementSize) throw new Error(`SpawnJSInterop: '${viewType}' has no BYTES_PER_ELEMENT`);
            if (byteLength % elementSize !== 0) {
                throw new RangeError(`SpawnJSInterop: ${byteLength} bytes is not a whole number of ${viewType} elements (${elementSize} bytes each)`);
            }
            return new ctor(buffer, address, byteLength / elementSize);
        }
        // Assigns a record (a plain object of string keys) built .Net-side onto parent[key].
        // assignRecord was removed. It rebuilt a record from its own string keys to drop the enumerable
        // Symbol the .Net runtime tags every object it PROXIES with - which a record-typed web API chokes
        // on, because it enumerates every own key and converts each to a string ("Cannot convert a Symbol
        // value to a string", WebGPU createComputePipeline constants).
        // The marshallers now write objects through the slot table, so a descriptor is never proxied and
        // never tagged. Stripping the tag is unnecessary once nothing applies it.
        // returns string[] of the target's property names.
        // hasOwnProperty true restricts to the object's own enumerable keys (Object.keys); false walks the
        // prototype chain too, which is what you need to enumerate a DOM object's API rather than just the
        // handful of own properties it happens to carry.
        objectKeys(target, hasOwnProperty) {
            if (target === void 0 || target === null) return [];
            if (hasOwnProperty) return Object.keys(target);
            var keys = [];
            for (var key in target) {
                if (keys.indexOf(key) === -1) keys.push(key);
            }
            return keys;
        }
        // full ? strict equality : loose equality
        objectEquals(obj1, obj2, full) {
            return full ? obj1 === obj2 : obj1 == obj2;
        }
        // returns string
        getPropertyTypeInfo(parent, key) {
            var value = parent[key];
            var jsClass = Object.prototype.toString.call(value).split(' ')[1].slice(0, -1);
            var jsType = typeof (value);
            return `${jsType} ${jsClass}`;
        }
        // returns string[]
        getPropertyConstructorNames(parent, key) {
            return this.getConstructorNames(parent[key]);
        }
        // Returns the WebAssembly linear memory ArrayBuffer that the .Net heap lives in.
        // Zero copy views are built directly over this, so reaching it by the wrong path does not fail
        // loudly - it silently produces a view onto the wrong bytes. The runtime exposes it under
        // different shapes depending on version, so every known shape is tried and the one that worked is
        // reportable via wasmMemoryBufferSource().
        wasmMemoryBuffer() {
            var found = this.#findWasmMemory();
            if (!found) throw new Error('SpawnJSInterop: could not reach the WebAssembly memory buffer');
            return found.buffer;
        }
        // returns the name of the path the memory buffer was found under, or '' if it was not found
        wasmMemoryBufferSource() {
            var found = this.#findWasmMemory();
            return found ? found.source : '';
        }
        #findWasmMemory() {
            var rt = this.dotnetRuntime;
            if (!rt) return null;
            var candidates = [
                ['Module.HEAPU8.buffer', () => rt.Module?.HEAPU8?.buffer],
                ['Module.wasmMemory.buffer', () => rt.Module?.wasmMemory?.buffer],
                ['localHeapViewU8().buffer', () => rt.localHeapViewU8?.()?.buffer],
                ['getHeapU8().buffer', () => rt.getHeapU8?.()?.buffer],
            ];
            for (var i = 0; i < candidates.length; i++) {
                var buffer;
                try { buffer = candidates[i][1](); } catch (ex) { continue; }
                if (buffer && typeof buffer.byteLength === 'number' && buffer.byteLength > 0) {
                    return { buffer: buffer, source: candidates[i][0] };
                }
            }
            return null;
        }
        // returns string[] of [typeof, ...constructorNames]
        // typeof and the prototype chain together are everything needed to identify a value, so they are
        // fetched in a single call. Anything that has to pick a .Net type from a live Javascript value
        // reads this, and reads it once.
        getPropertyTypeAndConstructorNames(parent, key) {
            var value = parent[key];
            var ret = [typeof (value)];
            var names = this.getConstructorNames(value);
            for (var i = 0; i < names.length; i++) ret.push(names[i]);
            return ret;
        }
        // returns string[]
        getConstructorNames(obj) {
            var constructorNames = [];
            if (obj === void 0 || obj === null) return constructorNames;
            var o = obj;
            var cName;
            while (1) {
                o = Object.getPrototypeOf(o);
                cName = o?.constructor?.name;
                if (!cName) break;
                if (constructorNames.indexOf(cName) !== -1) continue;
                constructorNames.push(cName);
            }
            return constructorNames;
        }
        // The inbound half of the flat buffer design - the mirror of netToJSBuffer below.
        //
        // Javascript writes the arguments into this buffer at its own top and calls .Net with
        // (cmd, offset, length): a string and two numbers, no object reference. Previously the arguments
        // crossed as a JS array marshalled into a JSObject, and the result came back as a second one, so
        // every inbound call - every DOM event, every callback, every promise settlement - paid for two
        // runtime proxies.
        //
        // It is a SEPARATE buffer from netToJSBuffer, with its own top, and that is the point rather than
        // symmetry: each side owns its top locally and can bump it for free. A single shared top would
        // have to live on one side of the boundary, so the other side would pay a crossing per call just
        // to read it - the very cost this removes.
        jsToNetBuffer = [];
        jsToNetTop = 0;
        // Pushes args, calls .Net, unwinds. Returns the result when the handler produced one.
        #callNet(cmd, args, wantsResult) {
            var b = this.jsToNetBuffer;
            var offset = this.jsToNetTop;
            var length = args.length;
            // Reserve at least one slot even with no arguments: the result comes back in the FIRST slot
            // of this call's region, so a zero-argument call still has to own one or a nested call writes
            // over it. Same reservation the outbound side makes.
            this.jsToNetTop += length > 0 ? length : 1;
            for (var i = 0; i < length; i++) b[offset + i] = args[i];
            try {
                var hasResult = this._JSToNetCall(cmd, offset, length);
                return wantsResult && hasResult ? b[offset] : null;
            } finally {
                // unwind in a finally so a throwing handler cannot leak the region and grow the buffer
                this.jsToNetTop = offset;
            }
        }
        // Creates a new function for .Net to use wit hJS as it needs to allow JS to call into .Net
        registerCallback(id) {
            return (...args) => this.#callNet(id, args, true);
        }
        // Creates a new function for .Net to use wit hJS as it needs to allow JS to call into .Net
        registerCallbackVoid(id) {
            return (...args) => { this.#callNet(id, args, false); };
        }
        _JSToNetCall() {
            // this method is a placeHolder and will be overwritten
            // by SpawnJSRuntime immediately after constructed by its own JSExport-ed method.
            // this placeholder is here for clarity.
        }
        jsToNetCall(/* string */ cmd, ...args) {
            return this.#callNet(cmd, args, true);
        }
        jsToNetCallApply(/* string */ cmd, /* Array */ args) {
            return this.#callNet(cmd, args || [], true);
        }
        // Arguments and results both live in this one flat buffer, so a synchronous call carries only
        // primitives across the boundary: the command name, an offset and a length. No Javascript object
        // reference is marshalled at all, which is the cost that survived pooling the argument arrays.
        //
        // .Net appends its arguments at the current top, calls, reads the result back out of the FIRST
        // slot it wrote (the arguments there have already been consumed), then unwinds the top. That makes
        // it a stack: a nested call - a marshaller reading a property while marshalling an argument -
        // writes above the outer call's region and cannot disturb it.
        netToJSBuffer = [];
        _netToJSCall(/* string */ cmd, /* number */ offset, /* number */ length) {
            var a = this.netToJSBuffer;
            if (this.verbose) console.log(">> _netToJSCall::", cmd, offset, length);
            // Dispatch by arity rather than spreading a slice. Every command takes four arguments or
            // fewer, so the spread path is a fallback that should never run - and it is the only branch
            // here that would allocate.
            var ret;
            switch (length) {
                case 0: ret = this[cmd](); break;
                case 1: ret = this[cmd](a[offset]); break;
                case 2: ret = this[cmd](a[offset], a[offset + 1]); break;
                case 3: ret = this[cmd](a[offset], a[offset + 1], a[offset + 2]); break;
                case 4: ret = this[cmd](a[offset], a[offset + 1], a[offset + 2], a[offset + 3]); break;
                default: ret = this[cmd](...a.slice(offset, offset + length)); break;
            }
            if (this.verbose) console.log("<< _netToJSCall::", ret);
            // hand the result back in the first slot of the caller's own region
            a[offset] = ret;
        }
        // There is no _netToJSCallAsync. An async command is just a synchronous call that returns a
        // Promise, which .Net turns into a Task with then(resolve, reject) - so async rides the same flat
        // buffer stack as everything else, and no Task ever has to be marshalled across the boundary.
    }
    globalThis.SpawnJSInterop = SpawnJSInterop;
})()

// ---------------------------------------------------------------------------------------------
// SPIKE: slot-based object references.
// Holds Javascript values in a JS-side table addressed by an integer, so .Net can reference a JS
// object without the runtime creating a JSObject proxy for it. Every proxy costs a GC handle, a
// proxy-table entry and an enumerable Symbol tag on the object - measured at 21us to create an
// object and 7.4us to wrap one, against 1.4us for a scalar property write. Since the marshallers
// already move one value at a time, the proxy buys nothing outside startup.
// Free list keeps slot ids dense so the table does not grow without bound.
// Keys are allocated monotonically and NEVER reused, and the table is an object so a freed key is
// deleted rather than leaving a hole. Reuse would be denser, but it would mean a disposed handle that
// still touched its key would read whatever value now occupies that slot - silently wrong data instead
// of undefined. ReleasedSlotKeyIsNotReusedTest locks this down; do not "optimise" it into a free list.
globalThis.__sjsSlots = {};
globalThis.__sjsNextSlot = 1;
globalThis.__sjsAlloc = function (value) {
    var slot = globalThis.__sjsNextSlot++;
    globalThis.__sjsSlots[slot] = value;
    return slot;
};
globalThis.__sjsAllocEmpty = function () { return globalThis.__sjsAlloc(void 0); };
// Allocates a slot AND stores the value in one crossing. Taking a handle used to be an allocation
// call followed by a separate Reflect.Set - two crossings to park one object.
globalThis.__sjsAllocValue = function (value) { return globalThis.__sjsAlloc(value); };
globalThis.__sjsNewObject = function () { return globalThis.__sjsAlloc({}); };
// Allocates a slot holding a string. One crossing, paid once per interned string - every later use
// of that string is just its slot id, which is a number.
globalThis.__sjsAllocString = function (value) { return globalThis.__sjsAlloc(value); };
globalThis.__sjsNewArray = function () { return globalThis.__sjsAlloc([]); };
globalThis.__sjsFree = function (slot) { delete globalThis.__sjsSlots[slot]; };
// How many entries the slot table actually holds. Diagnostic: SpawnJSHandle.LiveSlotCount only counts
// the slots a HANDLE owns, so a slot allocated Javascript side and owned by nobody is invisible to it -
// which is precisely how the object-argument leak went unnoticed. This counts the table itself.
globalThis.__sjsSlotTableCount = function () { return Object.keys(globalThis.__sjsSlots).length; };
globalThis.__sjsSetDouble = function (slot, key, value) { globalThis.__sjsSlots[slot][key] = value; };
globalThis.__sjsSetString = function (slot, key, value) { globalThis.__sjsSlots[slot][key] = value; };
// Numeric-key variants. The shared call buffer is an ARRAY indexed by offset, so forcing those keys
// through a string conversion allocated a string per argument and turned an indexed array write into a
// keyed one. That regressed every call that goes through the dispatcher.
globalThis.__sjsSetDoubleAt = function (slot, index, value) { globalThis.__sjsSlots[slot][index] = value; };
globalThis.__sjsSetStringAt = function (slot, index, value) { globalThis.__sjsSlots[slot][index] = value; };
globalThis.__sjsSetBooleanAt = function (slot, index, value) { globalThis.__sjsSlots[slot][index] = value; };
globalThis.__sjsSetSlotAt = function (slot, index, valueSlot) { globalThis.__sjsSlots[slot][index] = globalThis.__sjsSlots[valueSlot]; };
globalThis.__sjsSetBoolean = function (slot, key, value) { globalThis.__sjsSlots[slot][key] = value; };
// Reads a property of the slotted object and hands the RAW value back, letting the .Net return type
// declared on the binding do the conversion. This is the same shape Reflect.get is used in - one
// Javascript function bound at several return types - and it is why a typed property read needs no
// proxy for the object it is reading from.
globalThis.__sjsGet = function (slot, key) { return globalThis.__sjsSlots[slot][key]; };
// The same function under a second name, so the .Net side can bind it with a NUMERIC key parameter and
// skip converting an index to a string. Javascript does not care - arr[0] and arr["0"] address the same
// element - but the conversion allocates a string per read, which is what the SetAt variants exist to
// avoid on the write side.
globalThis.__sjsGetAt = globalThis.__sjsGet;
// The slot TABLE stays shared, deliberately and measurably. Slot ids come from one monotonic counter
// and are never reused, so two runtimes cannot reach each other's entries through it, and nothing
// enumerates or clears it. A per-context table was measured at +11% on the hottest path (0.93us ->
// 1.03us for a property read) and buys no isolation that matters.
// The value a slot holds, rather than a property of it. A handle that OWNS its storage IS the slot, so
// reading its own value is this rather than a keyed read.
// Together with __sjsGetAt this is what lets a value be read with no proxy at either end: an owning
// handle reads itself here, and a volatile handle - which borrows its parent's storage - reads
// parent[key] there. Before, both went through JSParent, which is a JSObject, so every read of a
// number or a string out of a borrowed handle resolved a proxy for the object holding it.
globalThis.__sjsSelf = function (slot) { return globalThis.__sjsSlots[slot]; };

// ---------------------------------------------------------------------------------------------
// PROBE: an argument buffer living in .Net's OWN memory rather than in Javascript's.
//
// Javascript can view the WebAssembly linear memory directly, so a buffer placed THERE is free to
// both sides: .Net writes are plain stores, and Javascript reads are ordinary DataView reads. The
// buffers we have today live Javascript side, which is free for Javascript but costs .Net a boundary
// crossing PER ARGUMENT. Moving the buffer into .Net memory collapses an N argument call from N+1
// crossings to 1 - the signal that says "go, at this offset".
//
// DataView rather than a typed array because the arguments are heterogeneous: a tag byte and a
// payload at an arbitrary byte offset, read with getUint8/getFloat64, off one view over the whole
// heap. A Float64Array would force one type and 8 byte alignment.
//
// ⚠️ DataView is BIG ENDIAN by default. WebAssembly memory is little endian, so every get/set here
// passes littleEndian=true explicitly. Omitting it does not throw - it silently byte swaps.
//
// ⚠️ Growing the WebAssembly memory DETACHES the old ArrayBuffer and every view over it goes to
// byteLength 0. The view is cached because re acquiring it per call would reintroduce the crossing
// this exists to remove, so every use checks for detachment first and rebinds.
// The runtime already publishes a TypedArray view over the WHOLE linear memory for each element type
// - HEAPU8, HEAPF64 and friends - and those use the PLATFORM's endianness, which is what .Net writes
// and what Javascript reads everywhere else. Reading through them means there is no byte order
// question to get wrong: no flag, no default to remember, nothing to forget at one call site out of
// fifty. Element indexing is also the shape engines optimise hardest.
//
// Emscripten REPLACES these views when the memory grows, so they are looked up per call rather than
// cached. That is two property reads Javascript side and no boundary crossing - and it makes the
// detachment problem disappear rather than needing a check, because the runtime maintains them.
//
// ⚠️ HEAPF64 is indexed in ELEMENTS, so a byte address becomes address >>> 3. That requires the
// address to be 8 byte aligned; a misaligned buffer would silently read the wrong element, so the
// alignment is asserted at bind time rather than assumed.
globalThis.__sjsHeaps = function (ctx) {
    var m = SpawnJSInterop.ctx(ctx).dotnetRuntime?.Module;
    if (!m) throw new Error('SpawnJSInterop: the dotnet Module is not reachable');
    return m;
};
// Reports which HEAP views this runtime actually exposes, so the design rests on measurement rather
// than on what Emscripten usually exports.
globalThis.__sjsHeapViewNames = function (ctx) {
    var m = globalThis.__sjsHeaps(ctx);
    var names = ['HEAP8', 'HEAPU8', 'HEAP16', 'HEAPU16', 'HEAP32', 'HEAPU32', 'HEAPF32', 'HEAPF64'];
    var found = [];
    for (var i = 0; i < names.length; i++) if (m[names[i]]) found.push(names[i]);
    return found.join(',');
};
globalThis.__sjsBindArgBuffer = function (ctx, address, byteLength) {
    if (address % 8 !== 0) throw new Error(`SpawnJSInterop: argument buffer address ${address} is not 8 byte aligned`);
    SpawnJSInterop.ctx(ctx).argBufferAddress = address;
    return true;
};
// PROBE ONLY: sums `count` float64 arguments .Net wrote at `offset` bytes.
// The point is not the sum - it is that .Net paid ZERO crossings to write them and ONE to deliver.
globalThis.__sjsHeapSum = function (ctx, offset, count) {
    var f64 = globalThis.__sjsHeaps(ctx).HEAPF64;
    var at = (SpawnJSInterop.ctx(ctx).argBufferAddress + offset) >>> 3;
    var total = 0;
    for (var i = 0; i < count; i++) total += f64[at + i];
    return total;
};
// PROBE ONLY: reads a .Net string straight out of .Net memory.
// A .Net string is UTF-16, and a pinned one hands back the address of its FIRST CHARACTER - so
// HEAPU16 indexes it directly with no copy on the .Net side and no marshalling machinery.
// The address is only valid for the duration of this call, because the string is pinned around the
// call and released after it. Nothing here may retain the subarray.
globalThis.__sjsReadUtf16 = function (ctx, address, length) {
    var u16 = globalThis.__sjsHeaps(ctx).HEAPU16;
    var at = address >>> 1;
    // fromCharCode.apply blows the argument limit on long strings, so decode those instead. The
    // decoder reads the bytes directly; neither path copies on the .Net side.
    if (length > 4096) {
        return new TextDecoder('utf-16le').decode(new Uint8Array(u16.buffer, address, length * 2));
    }
    return String.fromCharCode.apply(null, u16.subarray(at, at + length));
};
// PROBE ONLY: the INTERLEAVED frame - one padded slot per argument, the shape the .Net runtime's own
// marshaller uses (value at slot+0, a type tag BYTE inside the same slot, stride padded so every value
// stays 8 byte aligned). Measured against the structure-of-arrays layout above rather than assumed
// better. Stride 16: value at +0, tag at +8.
// The TRANSPORT frame's address. Owned by the runtime and bound exactly once, at startup.
// The probe/benchmark frames deliberately use a DIFFERENT global: they are separate frames, and when
// they shared this one, binding a probe silently redirected every live transport call to read the
// probe's memory instead. Nothing threw - the reads simply came from the wrong place.
globalThis.__sjsBindArgFrame = function (ctx, address, byteLength) {
    if (address % 8 !== 0) throw new Error(`SpawnJSInterop: argument frame address ${address} is not 8 byte aligned`);
    SpawnJSInterop.ctx(ctx).argFrameAddress = address;
    return true;
};
// The probe frame's address - benchmarks and layout tests only, never the transport.
globalThis.__sjsBindProbeFrame = function (ctx, address, byteLength) {
    if (address % 8 !== 0) throw new Error(`SpawnJSInterop: probe frame address ${address} is not 8 byte aligned`);
    SpawnJSInterop.ctx(ctx).probeFrameAddress = address;
    return true;
};
globalThis.__sjsFrameSum = function (ctx, count) {
    var f64 = globalThis.__sjsHeaps(ctx).HEAPF64;
    var at = SpawnJSInterop.ctx(ctx).probeFrameAddress >>> 3;
    var total = 0;
    // stride 16 bytes = 2 float64 elements
    for (var i = 0; i < count; i++) total += f64[at + i * 2];
    return total;
};
globalThis.__sjsFrameTaggedSum = function (ctx, count) {
    var m = globalThis.__sjsHeaps(ctx);
    var f64 = m.HEAPF64;
    var u8 = m.HEAPU8;
    var base = SpawnJSInterop.ctx(ctx).probeFrameAddress;
    var at = base >>> 3;
    var total = 0;
    for (var i = 0; i < count; i++) {
        var value = f64[at + i * 2];
        if (u8[base + i * 16 + 8] === 3) value = globalThis.__sjsSlots[value];
        total += value;
    }
    return total;
};
// PROBE ONLY: interleaved, but the tag lives in the slot's PADDING as a float64 rather than as a byte.
// The padding exists either way, so it costs no space - and it needs only ONE heap view and one width
// of read, where the byte form needs HEAPU8 as well as HEAPF64.
globalThis.__sjsFrameTaggedSumF64 = function (ctx, count) {
    var f64 = globalThis.__sjsHeaps(ctx).HEAPF64;
    var at = SpawnJSInterop.ctx(ctx).probeFrameAddress >>> 3;
    var total = 0;
    for (var i = 0; i < count; i++) {
        var value = f64[at + i * 2];
        if (f64[at + i * 2 + 1] === 3) value = globalThis.__sjsSlots[value];
        total += value;
    }
    return total;
};
// ---------------------------------------------------------------------------------------------
// THE ARGUMENT FRAME - the live outbound transport.
//
// Arguments live in .Net's OWN memory, which Javascript views directly, so .Net writes them with
// plain array stores and pays NOTHING to deliver them. Only the call itself crosses: a command name,
// an offset and a length. The Javascript-side buffer it replaces cost .Net one crossing PER
// ARGUMENT.
//
// One padded 16 byte slot per argument - value at +0, tag at +8, both float64 so there is one heap
// view and one read width. Measured against structure-of-arrays and against a byte tag; this shape
// won both.
//
// Tags. Everything a dispatch actually passes - numbers, booleans, wrappers, interned strings -
// reaches Javascript with no crossing at all. Only a value that has to be BUILT here (a descriptor
// object, an array) falls back to the scratch array, and that one still costs what it costs today.
const SJS_TAG_NUMBER = 1;
const SJS_TAG_BOOLEAN = 2;
const SJS_TAG_SLOT = 3;      // an object, a wrapper, or an interned string - resolved in the slot table
const SJS_TAG_NULL = 4;
const SJS_TAG_UNDEFINED = 5;
const SJS_TAG_SCRATCH = 6;   // built Javascript side already; the payload indexes the scratch array
const SJS_TAG_OBJECT = 7;    // an object built HERE out of a nested frame region - no slot, nothing to free
// The pair count sits in the low digits of an inline object's payload, the heap index above it.
// Must match ArgTag.InlinePackLimit.
const SJS_INLINE_BASE = 1048576;

globalThis.__sjsFrameArg = function (f64, at, i, scratch) {
    var o = at + i * 2;
    switch (f64[o + 1]) {
        case SJS_TAG_NUMBER: return f64[o];
        case SJS_TAG_SLOT: return globalThis.__sjsSlots[f64[o]];
        case SJS_TAG_BOOLEAN: return f64[o] !== 0;
        case SJS_TAG_NULL: return null;
        case SJS_TAG_UNDEFINED: return void 0;
        case SJS_TAG_SCRATCH: return scratch[f64[o]];
        // The payload carries the region's own absolute heap index, so this needs no frame base - and
        // because the region is read here, nested inline objects fall out of the recursion for free.
        case SJS_TAG_OBJECT: {
            var p = f64[o];
            var n = p % SJS_INLINE_BASE;
            return globalThis.__sjsBuildFromFrame(f64, (p - n) / SJS_INLINE_BASE, n, scratch);
        }
        default: throw new Error(`SpawnJSInterop: argument ${i} has unknown tag ${f64[o + 1]}`);
    }
};
// Writes the result back into the CALLER'S OWN slot, the same convention the array buffer used - the
// arguments there have already been consumed by the time the result lands.
// A primitive goes into the frame itself, so .Net reads it with no crossing. Anything else takes a
// slot, which means an object returned from a call reaches .Net as a slot id - also no crossing, and
// no proxy.
globalThis.__sjsFrameResult = function (f64, at, value) {
    if (value === null) { f64[at] = 0; f64[at + 1] = SJS_TAG_NULL; return; }
    if (value === void 0) { f64[at] = 0; f64[at + 1] = SJS_TAG_UNDEFINED; return; }
    var t = typeof value;
    if (t === "number") { f64[at] = value; f64[at + 1] = SJS_TAG_NUMBER; return; }
    if (t === "boolean") { f64[at] = value ? 1 : 0; f64[at + 1] = SJS_TAG_BOOLEAN; return; }
    f64[at] = globalThis.__sjsAlloc(value);
    f64[at + 1] = SJS_TAG_SLOT;
};
globalThis.__sjsFrameCall = function (ctx, cmd, offset, length) {
    var interop = SpawnJSInterop.ctx(ctx);
    var scratch = interop.netToJSBuffer;
    var base = interop.argFrameAddress;
    var f64 = globalThis.__sjsHeaps(ctx).HEAPF64;
    var at = (base >>> 3) + offset * 2;
    var A = globalThis.__sjsFrameArg;
    // dispatch by arity rather than spreading a slice - the spread is the only branch that allocates
    var ret;
    switch (length) {
        case 0: ret = interop[cmd](); break;
        case 1: ret = interop[cmd](A(f64, at, 0, scratch)); break;
        case 2: ret = interop[cmd](A(f64, at, 0, scratch), A(f64, at, 1, scratch)); break;
        case 3: ret = interop[cmd](A(f64, at, 0, scratch), A(f64, at, 1, scratch), A(f64, at, 2, scratch)); break;
        case 4: ret = interop[cmd](A(f64, at, 0, scratch), A(f64, at, 1, scratch), A(f64, at, 2, scratch), A(f64, at, 3, scratch)); break;
        default: {
            var spread = new Array(length);
            for (var i = 0; i < length; i++) spread[i] = A(f64, at, i, scratch);
            ret = interop[cmd].apply(interop, spread);
            break;
        }
    }
    // RE-FETCH the view before writing the result. The call may have re-entered .Net - a callback, a
    // marshaller reading a property - and anything that grows the WebAssembly memory DETACHES the view
    // captured above. Writing through the stale one would throw, or worse, write nowhere.
    globalThis.__sjsFrameResult(globalThis.__sjsHeaps(ctx).HEAPF64, at, ret);
};
// Building an OBJECT whose members are already in the frame.
//
// Marshalling a descriptor used to cost one crossing PER MEMBER plus three more - create the object,
// attach it, free the temporary handle. Counted on a five member descriptor: eight crossings, which
// is nearly all of its ~18us. The marshaller was already cached per member, so the registry was never
// the problem; the boundary was.
//
// Members are written into the frame as name/value PAIRS - slot 2i is the property name, slot 2i+1 is
// its value - and the whole object is built here in one go. Property names are fixed literals per
// type, so they intern to a slot once per process and are thereafter just numbers like any other
// argument.
// Takes the scratch array rather than a context id, because __sjsFrameArg has scratch but no context -
// and an inline object argument is built straight from there.
globalThis.__sjsBuildFromFrame = function (f64, at, count, scratch) {
    var A = globalThis.__sjsFrameArg;
    var obj = {};
    for (var i = 0; i < count; i++) {
        obj[A(f64, at, i * 2, scratch)] = A(f64, at, i * 2 + 1, scratch);
    }
    return obj;
};
// There is deliberately no "build an object and hand back its slot". That shape leaked by construction:
// the slot table is a strong reference, a temporary built for one call belongs to nobody afterwards, and
// nothing freed it. An object argument is carried as SJS_TAG_OBJECT and built in place instead.
// Builds the object AND assigns it, so the whole descriptor costs exactly one crossing - no temporary
// slot is allocated, so none has to be freed either.
globalThis.__sjsBuildObjectInto = function (ctx, parentSlot, key, offset, count) {
    var interop = SpawnJSInterop.ctx(ctx);
    var f64 = globalThis.__sjsHeaps(ctx).HEAPF64;
    var at = (interop.argFrameAddress >>> 3) + offset * 2;
    globalThis.__sjsSlots[parentSlot][key] = globalThis.__sjsBuildFromFrame(f64, at, count, interop.netToJSBuffer);
};
// Calling a METHOD with its arguments already in the frame.
//
// This is the path a wrapper method call takes - setPipeline, setBindGroup, dispatchWorkgroups - and
// it was still building a Javascript argument array the expensive way: one crossing to create the
// array, one PER ARGUMENT to fill it, one to invoke, one to free it. N+3 crossings for a call whose
// arguments were already sitting in .Net memory.
//
// Now the target is a slot, the arguments are frame slots, and the whole call is ONE crossing.
globalThis.__sjsInvokeFrameArgs = function (ctx, f64, at, length) {
    var A = globalThis.__sjsFrameArg;
    var scratch = SpawnJSInterop.ctx(ctx).netToJSBuffer;
    var out = new Array(length);
    for (var i = 0; i < length; i++) out[i] = A(f64, at, i, scratch);
    return out;
};
globalThis.__sjsInvokeFrameVoid = function (ctx, targetSlot, name, offset, length) {
    var interop = SpawnJSInterop.ctx(ctx);
    var target = globalThis.__sjsSlots[targetSlot];
    var at = (interop.argFrameAddress >>> 3) + offset * 2;
    var f64 = globalThis.__sjsHeaps(ctx).HEAPF64;
    var A = globalThis.__sjsFrameArg;
    var scratch = interop.netToJSBuffer;
    // dispatch by arity so the common shapes allocate no argument array at all
    switch (length) {
        case 0: target[name](); return;
        case 1: target[name](A(f64, at, 0, scratch)); return;
        case 2: target[name](A(f64, at, 0, scratch), A(f64, at, 1, scratch)); return;
        case 3: target[name](A(f64, at, 0, scratch), A(f64, at, 1, scratch), A(f64, at, 2, scratch)); return;
        case 4: target[name](A(f64, at, 0, scratch), A(f64, at, 1, scratch), A(f64, at, 2, scratch), A(f64, at, 3, scratch)); return;
        default: target[name].apply(target, globalThis.__sjsInvokeFrameArgs(ctx, f64, at, length)); return;
    }
};
// Same, and the result goes back into the caller's own frame slot - so a call that returns a number
// or a boolean moves no data across the boundary in either direction, and one that returns an object
// hands back a slot id rather than a proxy.
globalThis.__sjsInvokeFrameResult = function (ctx, targetSlot, name, offset, length) {
    var interop = SpawnJSInterop.ctx(ctx);
    var target = globalThis.__sjsSlots[targetSlot];
    var at = (interop.argFrameAddress >>> 3) + offset * 2;
    var f64 = globalThis.__sjsHeaps(ctx).HEAPF64;
    var A = globalThis.__sjsFrameArg;
    var scratch = interop.netToJSBuffer;
    var ret;
    switch (length) {
        case 0: ret = target[name](); break;
        case 1: ret = target[name](A(f64, at, 0, scratch)); break;
        case 2: ret = target[name](A(f64, at, 0, scratch), A(f64, at, 1, scratch)); break;
        case 3: ret = target[name](A(f64, at, 0, scratch), A(f64, at, 1, scratch), A(f64, at, 2, scratch)); break;
        case 4: ret = target[name](A(f64, at, 0, scratch), A(f64, at, 1, scratch), A(f64, at, 2, scratch), A(f64, at, 3, scratch)); break;
        default: ret = target[name].apply(target, globalThis.__sjsInvokeFrameArgs(ctx, f64, at, length)); break;
    }
    // re-fetch: the call may have re-entered .Net and grown the memory, which detaches the view
    globalThis.__sjsFrameResult(globalThis.__sjsHeaps(ctx).HEAPF64, at, ret);
};
// PROBE ONLY: STRING ARGUMENTS, the last undecided piece of the frame layout.
// A string needs TWO fields - where its characters are, and how many - which is exactly why the
// runtime's own slot is wider than ours. Either the frame grows an aux field, or strings keep
// crossing one at a time. Measured rather than chosen.
//
// Frame arm: .Net pins each string and writes (address, length) into the slot's value and aux
// fields, so the strings themselves never cross; Javascript decodes them out of the heap.
// Stride 24: value +0, tag +8, aux +16.
globalThis.__sjsFrameStringLength = function (ctx, count) {
    var f64 = globalThis.__sjsHeaps(ctx).HEAPF64;
    var u16 = globalThis.__sjsHeaps(ctx).HEAPU16;
    var at = SpawnJSInterop.ctx(ctx).probeFrameAddress >>> 3;
    var total = 0;
    for (var i = 0; i < count; i++) {
        var address = f64[at + i * 3];
        var length = f64[at + i * 3 + 2];
        // decode it for real - measuring only the address read would measure nothing
        var s = String.fromCharCode.apply(null, u16.subarray(address >>> 1, (address >>> 1) + length));
        total += s.length;
    }
    return total;
};
// Slot arm: the strings were written across the boundary one at a time, the way they are today.
globalThis.__sjsSlotStringLength = function (ctx, argsSlot, count) {
    var a = globalThis.__sjsSlots[argsSlot];
    var total = 0;
    for (var i = 0; i < count; i++) total += a[i].length;
    return total;
};
// PROBE ONLY: the SAME sum, but over an argument array held Javascript side - the transport in use
// today. The Javascript work is identical to __sjsHeapSum by construction, so an A/B between them
// isolates exactly one thing: what .Net paid to get the arguments here.
globalThis.__sjsSlotSum = function (ctx, argsSlot, count) {
    var a = globalThis.__sjsSlots[argsSlot];
    var total = 0;
    for (var i = 0; i < count; i++) total += a[i];
    return total;
};
// PROBE ONLY: a heterogeneous argument list, structure of arrays rather than interleaved.
// Interleaving a tag byte with an eight byte payload gives a stride of 9, which breaks the alignment
// HEAPF64 indexing needs - so tags live in their own region and payloads in theirs, parallel by
// index. Both regions are read through the runtime's own views, so both are platform endian.
// tag 1 = number, 2 = boolean, 3 = slot reference.
globalThis.__sjsHeapTaggedSum = function (ctx, tagOffset, valueOffset, count) {
    var m = globalThis.__sjsHeaps(ctx);
    var u8 = m.HEAPU8;
    var f64 = m.HEAPF64;
    var tagAt = SpawnJSInterop.ctx(ctx).argBufferAddress + tagOffset;
    var valueAt = (SpawnJSInterop.ctx(ctx).argBufferAddress + valueOffset) >>> 3;
    var total = 0;
    for (var i = 0; i < count; i++) {
        var value = f64[valueAt + i];
        if (u8[tagAt + i] === 3) value = globalThis.__sjsSlots[value];
        total += value;
    }
    return total;
};
// A property write whose VALUE type is decided by the .Net binding rather than by this function - the
// write-side twin of __sjsGet. It covers the cases the typed setters do not: an arbitrary Any value, a
// JSObject the caller genuinely holds, and a byte array. In every one of them the value was never the
// problem; the PARENT had to become a proxy just to be written through, and that is what this removes.
globalThis.__sjsSetAny = function (slot, key, value) { globalThis.__sjsSlots[slot][key] = value; };
globalThis.__sjsSetAnyAt = globalThis.__sjsSetAny;
// Own enumerable keys of a slotted object, so a record can be read back without proxying it.
// Returns NULL - not an empty array - for null and undefined, so a caller can tell "there is no object
// here" from "an object with no keys". The proxy path it replaces made that distinction by handing back
// a null JSObject, and collapsing the two would turn a null record into an empty one.
globalThis.__sjsKeys = function (slot, ownOnly) {
    var target = globalThis.__sjsSlots[slot];
    if (target === void 0 || target === null) return null;
    if (ownOnly) return Object.keys(target);
    var out = [];
    for (var k in target) out.push(k);
    return out;
};
// Whether a property exists, without materialising the object to ask.
globalThis.__sjsHas = function (slot, key, useIn) {
    var target = globalThis.__sjsSlots[slot];
    return useIn ? (key in target) : Object.prototype.hasOwnProperty.call(target, key);
};
globalThis.__sjsSetSlot = function (slot, key, valueSlot) { globalThis.__sjsSlots[slot][key] = globalThis.__sjsSlots[valueSlot]; };

// Slot-native READS. These are the other half of the slot table: writing a descriptor without a proxy
// was only ever half the path, because every value READ back out of Javascript - every JS.Get<Window>,
// every wrapper returned from a call - still materialised one.
//
// A read into a NEW slot, so the reader OWNS what it read rather than borrowing its parent's storage.
// Two sentinels let one crossing both answer the question and hand back the reference:
//   0  the value is null or undefined - and no slot was allocated that the caller would have to free
//  -1  the value is not a reference, so there is nothing a handle can own; .Net falls back to the
//      proxy path, which raises exactly the error it always did for this case
// Neither is ever a valid slot: allocation starts at 1 and never reuses a key.
// A function counts as a reference. Javascript functions are legitimate wrapper targets, and typeof
// reports them separately from "object", so omitting them here would reject every one of them.
globalThis.__sjsIsRef = function (v) { var t = typeof v; return t === "object" || t === "function"; };
// As __sjsGetObjectSlot, but a value that is NOT a reference is slotted rather than refused. A slot
// holds any Javascript value, so a wrapper over a primitive - StringPrimitive is the one that exists -
// works perfectly well; it is only a JSObject PROXY that cannot represent one, which is why that path
// throws "JSObject proxy of string is not supported".
// Still returns 0 for null and undefined: those are absence, not a value to wrap.
globalThis.__sjsGetValueSlot = function (slot, key) {
    var v = globalThis.__sjsSlots[slot][key];
    if (v === void 0 || v === null) return 0;
    return globalThis.__sjsAlloc(v);
};
globalThis.__sjsCloneValueSlot = function (slot) {
    var v = globalThis.__sjsSlots[slot];
    if (v === void 0 || v === null) return 0;
    return globalThis.__sjsAlloc(v);
};
globalThis.__sjsGetObjectSlot = function (slot, key) {
    var v = globalThis.__sjsSlots[slot][key];
    if (v === void 0 || v === null) return 0;
    return globalThis.__sjsIsRef(v) ? globalThis.__sjsAlloc(v) : -1;
};
// Same read, addressed by numeric index. The shared call buffer is an ARRAY, so its reads must not pay
// a string key conversion per element - the same reason the SetAt variants exist.
globalThis.__sjsGetObjectSlotAt = function (slot, index) {
    var v = globalThis.__sjsSlots[slot][index];
    if (v === void 0 || v === null) return 0;
    return globalThis.__sjsIsRef(v) ? globalThis.__sjsAlloc(v) : -1;
};
// Takes a SECOND, independent slot on the value a slot already holds, so one handle can hand ownership
// of what it points at to another without either becoming a proxy. The two slots are separate
// references to the same Javascript value: freeing one does not disturb the other.
globalThis.__sjsCloneObjectSlot = function (slot) {
    var v = globalThis.__sjsSlots[slot];
    if (v === void 0 || v === null) return 0;
    return globalThis.__sjsIsRef(v) ? globalThis.__sjsAlloc(v) : -1;
};

// Slot-native invocation. `this`, the method, and the argument array all live in Javascript, so a call
// makes NO .Net proxy at all - the only things crossing are a slot number, a name, and a slot number.
// The old path had to materialise a JSObject for the target AND the arguments just to hand them over,
// which is why building a descriptor cheaply in slots still ended up creating proxies at call time.
globalThis.__sjsInvokeVoid = function (slot, name, argsSlot) {
    var target = globalThis.__sjsSlots[slot];
    target[name].apply(target, globalThis.__sjsSlots[argsSlot]);
};
globalThis.__sjsInvokeDouble = function (slot, name, argsSlot) {
    var target = globalThis.__sjsSlots[slot];
    return target[name].apply(target, globalThis.__sjsSlots[argsSlot]);
};
globalThis.__sjsInvokeString = function (slot, name, argsSlot) {
    var target = globalThis.__sjsSlots[slot];
    var r = target[name].apply(target, globalThis.__sjsSlots[argsSlot]);
    return r === void 0 || r === null ? null : r;
};
globalThis.__sjsInvokeBoolean = function (slot, name, argsSlot) {
    var target = globalThis.__sjsSlots[slot];
    return !!target[name].apply(target, globalThis.__sjsSlots[argsSlot]);
};
// Returns the RESULT IN A NEW SLOT, so an object-returning call still never becomes a proxy unless the
// caller genuinely needs one.
globalThis.__sjsInvokeSlot = function (slot, name, argsSlot) {
    var target = globalThis.__sjsSlots[slot];
    return globalThis.__sjsAlloc(target[name].apply(target, globalThis.__sjsSlots[argsSlot]));
};
// typeof of a slot's value, so .Net can tell "returned an object" from "returned a primitive" without
// dragging the value across.
globalThis.__sjsTypeOf = function (slot) {
    var v = globalThis.__sjsSlots[slot];
    return v === null ? "null" : typeof v;
};
