/* By Todd Tanner aka github.com/LostBeard 2026 */

(function () {
    if (globalThis.SpawnJSInterop) return;

    class SpawnJSInterop {
        static instances = {};
        static _idNext = 0;
        verbose = false;
        id = '';
        dotnetRuntime = null;
        constructor(dotnetRuntime) {
            this.dotnetRuntime = dotnetRuntime;
            this.id = `_${++SpawnJSInterop._idNext}`;
            SpawnJSInterop.instances[this.id] = this;
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
        // Creates a new function for .Net to use wit hJS as it needs to allow JS to call into .Net
        registerCallback(id) {
            return (...args) => {
                var ret = this._JSToNetCall(id, args);
                return ret == null || ret.length == 0 ? null : ret[0];
            }
        }
        // Creates a new function for .Net to use wit hJS as it needs to allow JS to call into .Net
        registerCallbackVoid(id) {
            return (...args) => {
                this._JSToNetCall(id, args);
            }
        }
        _JSToNetCall() {
            // this method is a placeHolder and will be overwritten
            // by SpawnJSRuntime immediately after constructed by its own JSExport-ed method.
            // this placeholder is here for clarity.
        }
        jsToNetCall(/* string */ cmd, ...args) {
        // intentionally extra code here to allow breakpoint return value debugging
            var ret = this._JSToNetCall(cmd, args);
            return ret;
        }
        jsToNetCallApply(/* string */ cmd, /* Array */ args) {
            // intentionally extra code here to allow breakpoint return value debugging
            var ret = this._JSToNetCall(cmd, args);
            return ret;
        }
        _netToJSCall(/* string */ cmd, /* Array */ argArray) {
            if (this.verbose) console.log(">> _netToJSCall::", cmd, argArray);
            var ret = this[cmd](...argArray);
            if (this.verbose) console.log("<< _netToJSCall::", ret);
            // package the results in an array so it can be read as a JSObject and then pulled into as needed wit hthe correct typing and marshalling
            return [ret];
        }
        async _netToJSCallAsync(/* string */ cmd, /* Array */ argArray) {
            if (this.verbose) console.log(">> _netToJSCallAsync::", cmd, argArray);
            var ret = await this[cmd](...argArray);
            if (this.verbose) console.log("<< _netToJSCallAsync::", ret);
            // package the results in an array so it can be read as a JSObject and then pulled into as needed wit hthe correct typing and marshalling
            return [ret];
        }
    }
    globalThis.SpawnJSInterop = SpawnJSInterop;
})()