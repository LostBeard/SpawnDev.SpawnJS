'strict';

// SpawnJSTests - the Javascript half of the SpawnJS marshaller tests.
//
// The .Net side does the work: create data, marshal it, verify it. This file only supplies the two
// things .Net cannot do on its own:
//
//   1. Values that only Javascript can create, for the JS -> .Net direction (undefined, BigInt, Date,
//      a TypedArray built JS-side, a rejected Promise, a JS function).
//   2. A read of what Javascript actually got, reported as a primitive - so a marshaller is never
//      verified only by reading it back through itself.
//
// Everything returns a string, number or boolean. No JSON: SpawnJS does not marshal through JSON and
// neither does the thing measuring it.
(function () {
    if (globalThis.SpawnJSTests) return;

    class SpawnJSTests {
        // ---- reads: what did Javascript actually receive? ----

        // "typeof:ConstructorName" from the prototype chain, so a derived type reports itself
        // (Object.prototype.toString.call(new TypeError()) says "Error", which cannot tell them apart)
        static describe(v) {
            if (v === null) return 'object:null';
            if (v === undefined) return 'undefined:undefined';
            var ctor = '';
            try { ctor = Object.getPrototypeOf(v)?.constructor?.name ?? ''; } catch { ctor = '?'; }
            return `${typeof v}:${ctor || 'null-prototype'}`;
        }
        static typeOf(v) { return typeof v; }
        static isUndefined(v) { return v === undefined; }
        static isNull(v) { return v === null; }
        static same(a, b) { return a === b; }
        // String() rather than a number read, so BigInt, NaN, -0 and Infinity all report themselves
        static str(v) { return v === undefined ? '[undefined]' : v === null ? '[null]' : String(v); }
        static num(v) { return Number(v); }
        static lengthOf(v) { return v === null || v === undefined ? -1 : Number(v.length); }
        static ownKeys(v) { return v === null || v === undefined ? '' : Object.keys(v).join(','); }
        // "in" is true for a member explicitly set to undefined and false for one never written - the
        // only way to tell "wrote undefined" from "wrote nothing"
        static hasIn(v, key) { return v !== null && v !== undefined && (key in Object(v)); }
        // every element stringified, comma joined
        static elements(v) {
            if (v === null || v === undefined) return '';
            var out = [];
            for (var i = 0; i < v.length; i++) out.push(String(v[i]));
            return out.join(',');
        }
        // the bytes the view actually spans, as hex. Reads through the view's own buffer/byteOffset/
        // byteLength, so a view built over the wrong memory or sized by the wrong element size shows up
        // as wrong bytes rather than as a well formed TypedArray.
        static bytesHex(v) {
            if (v === null || v === undefined) return '';
            var isBuffer = v instanceof ArrayBuffer || (globalThis.SharedArrayBuffer && v instanceof SharedArrayBuffer);
            var bytes = isBuffer ? new Uint8Array(v) : new Uint8Array(v.buffer, v.byteOffset, v.byteLength);
            var out = '';
            for (var i = 0; i < bytes.length; i++) out += bytes[i].toString(16).padStart(2, '0');
            return out;
        }

        // ---- view geometry, one scalar per call ----
        static viewCtor(v) { return Object.getPrototypeOf(v)?.constructor?.name ?? ''; }
        static viewLength(v) { return Number(v.length); }
        static viewByteLength(v) { return Number(v.byteLength); }
        static viewByteOffset(v) { return Number(v.byteOffset ?? 0); }
        static viewBufferByteLength(v) { return Number((v.buffer ?? v).byteLength); }
        // true when the backing buffer IS this app's WebAssembly memory - the difference between a live
        // view of .Net memory and a copy that merely holds the same bytes
        static isOnWasmHeap(v) {
            if (v === null || v === undefined) return false;
            var buffer = v instanceof ArrayBuffer ? v : v.buffer;
            if (!buffer) return false;
            var instances = globalThis.SpawnJSInterop?._instances ?? {};
            for (var id in instances) {
                if (!Object.hasOwn(instances, id)) continue;
                try { if (instances[id].getHeap() === buffer) return true; } catch { }
            }
            return false;
        }
        // a live heap view carries the descriptor the reviver reattaches from; a copy does not
        static hasHeapViewInfo(v) { return v !== null && v !== undefined && Object.hasOwn(v, '_heapViewInfo'); }

        // ---- writes from Javascript, to prove a view is live rather than a copy ----
        static writeElement(v, index, value) { v[index] = value; }
        static writeElementBig(v, index, value) { v[index] = BigInt(value); }

        // ---- argument capture: the int-key (call argument) path ----
        // .Net writes a property by STRING key and a call argument by INT key. Those are separate
        // marshaller overloads that can drift apart, so the argument path needs its own instrument.
        static captured = [];
        static capture(...args) { SpawnJSTests.captured = args; return args.length; }
        static capturedAt(index) { return SpawnJSTests.captured[index]; }

        // ---- fixtures: values only Javascript can create ----
        static undefinedValue() { return undefined; }
        static nullValue() { return null; }
        static numberFrom(str) { return Number(str); }
        static bigIntFrom(str) { return BigInt(str); }
        static dateFrom(ms) { return new Date(ms); }
        static numberArray(csv) { return csv === '' ? [] : csv.split(',').map(Number); }
        static stringArray(csv) { return csv === '' ? [] : csv.split(','); }
        // an array holding null and undefined, so element-level absence is exercised
        static arrayWithHoles() { return [1, null, undefined, 4]; }
        static typedArray(kind, csv) {
            var ctor = globalThis[kind];
            if (!ctor) throw new Error(`${kind} is not available in this host`);
            var parts = csv === '' ? [] : csv.split(',');
            var isBig = kind === 'BigInt64Array' || kind === 'BigUint64Array';
            return ctor.from(parts, s => isBig ? BigInt(s) : Number(s));
        }
        static arrayBufferOf(byteCount) { return new ArrayBuffer(byteCount); }
        static newObject() { return {}; }
        // a member that is PRESENT but undefined, next to one that is simply absent
        static objectWithUndefinedMember() { return { present: 1, absent: undefined }; }
        static objectWithNullMember() { return { present: 1, absent: null }; }
        static errorOf(kind, message) { return new (globalThis[kind] ?? Error)(message); }
        static identityFunction() { return (v) => v; }
        // invokes a function .Net handed to Javascript, with Javascript-native arguments - the only way
        // to prove a Callback/Delegate marshalled INTO Javascript is callable there
        static invoke(fn, arg1) { return fn(arg1); }
        static invokeVoid(fn) { fn(); return true; }

        // ---- promises, including every rejection shape ----
        static resolvedPromise(value) { return Promise.resolve(value); }
        static rejectedPromiseError(message) { return Promise.reject(new Error(message)); }
        static rejectedPromiseTypeError(message) { return Promise.reject(new TypeError(message)); }
        static rejectedPromiseString(message) { return Promise.reject(message); }
        static rejectedPromiseNull() { return Promise.reject(null); }
        static rejectedPromiseUndefined() { return Promise.reject(undefined); }
        // rejects on a later turn of the event loop rather than synchronously
        static async asyncThrow(message) { await Promise.resolve(); throw new Error(message); }
        static async asyncReturn(value) { await Promise.resolve(); return value; }
        static throwSync(message) { throw new Error(message); }
        // reports how a promise .Net wrote settled, as a plain string
        static promiseOutcome(promise) {
            return Promise.resolve(promise).then(
                v => `resolved:${SpawnJSTests.str(v)}`,
                e => `rejected:${SpawnJSTests.str(e && e.message !== undefined ? e.message : e)}`
            );
        }
    }
    globalThis.SpawnJSTests = SpawnJSTests;
})();
