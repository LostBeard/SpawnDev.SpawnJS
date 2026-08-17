'strict';

// SpawnJSTests - the Javascript half of the SpawnJS marshaller test suite.
//
// WHY THIS FILE EXISTS
// A marshaller round trip that is verified by reading the value back through the SAME marshaller can
// pass while both directions are wrong in the same way. Every assertion that matters therefore needs a
// SECOND instrument on the Javascript side that reports what Javascript actually received, using only
// primitives (string / number / boolean) that do not depend on the marshaller under test.
//
// Two jobs:
//   1. INSPECTION - given a value .Net wrote, report its real Javascript shape (typeof, constructor
//      chain, element values, view geometry) as a plain string or number.
//   2. FIXTURES - construct Javascript values .Net cannot construct on its own (BigInt, Date, a rejected
//      Promise, a TypedArray built JS-side, an object with an explicitly-undefined member) so the
//      JS -> .Net direction is fed a genuine Javascript value rather than one .Net just wrote.
//
// Everything here returns a string, a number or a boolean unless the test is specifically about reading
// an object back. Strings and numbers cross on the simplest possible paths, so a failure in this file's
// output is a failure in the thing being measured, not in the ruler.
(function () {
    if (globalThis.SpawnJSTests) return;

    // JSON.stringify throws on BigInt, and several fixtures are BigInt on purpose.
    function jsonReplacer(key, value) {
        if (typeof value === 'bigint') return `${value}n`;
        if (typeof value === 'function') return `[function ${value.name || 'anonymous'}]`;
        if (typeof value === 'undefined') return '[undefined]';
        return value;
    }

    class SpawnJSTests {
        // values captured from the most recent capture() call - see capture()
        static captured = [];
        // set by the callback fixtures so .Net can confirm a Callback actually ran JS-side
        static callbackLog = [];

        // ******************************************************************************************
        // INSPECTION - what did Javascript actually receive?
        // ******************************************************************************************

        // "typeof:MostDerivedConstructorName". The constructor NAME alone cannot identify a derived
        // type (Object.prototype.toString.call(new TypeError()) is "[object Error]"), so the prototype
        // chain's first entry is used - that is the real constructor.
        static describe(v) {
            var t = typeof v;
            if (v === null) return 'object:null';
            if (v === undefined) return 'undefined:undefined';
            var ctor = '';
            try { ctor = Object.getPrototypeOf(v)?.constructor?.name ?? ''; } catch { ctor = '?'; }
            if (!ctor) {
                // a null-prototype object, e.g. Object.create(null)
                ctor = t === 'object' ? 'null-prototype' : t;
            }
            return `${t}:${ctor}`;
        }
        // the full prototype chain, most derived first, comma joined - "Uint8Array,TypedArray,Object"
        static chain(v) {
            var names = [];
            if (v === null || v === undefined) return '';
            var o = v;
            while (1) {
                o = Object.getPrototypeOf(o);
                var name = o?.constructor?.name;
                if (!name) break;
                if (names.indexOf(name) === -1) names.push(name);
            }
            return names.join(',');
        }
        static typeOf(v) { return typeof v; }
        static isUndefined(v) { return v === undefined; }
        static isNull(v) { return v === null; }
        // === , so a test can prove two reads returned the SAME Javascript object rather than a clone
        static same(a, b) { return a === b; }
        static json(v) { return JSON.stringify(v, jsonReplacer) ?? '[undefined]'; }
        // String(v) works for BigInt and Symbol where JSON.stringify does not
        static str(v) { return v === undefined ? '[undefined]' : v === null ? '[null]' : String(v); }
        static num(v) { return Number(v); }
        static lengthOf(v) { return v === null || v === undefined ? -1 : Number(v.length); }
        // own enumerable keys only - inherited keys belong to the prototype, not to the value
        static ownKeys(v) { return v === null || v === undefined ? '' : Object.keys(v).join(','); }
        static hasOwn(v, key) { return v !== null && v !== undefined && Object.hasOwn(Object(v), key); }
        // "in" is true for an explicitly-undefined member and false for an absent one - the ONLY way to
        // tell "wrote undefined" apart from "wrote nothing"
        static hasIn(v, key) { return v !== null && v !== undefined && (key in Object(v)); }
        // every element stringified and comma joined. Works for BigInt64Array (String(1n) === "1")
        // where a number-based read would throw, and shows -0/NaN/Infinity as themselves.
        static elements(v) {
            if (v === null || v === undefined) return '';
            var out = [];
            for (var i = 0; i < v.length; i++) out.push(String(v[i]));
            return out.join(',');
        }
        // the raw bytes a view spans, as lowercase hex. This is the byte-for-byte oracle: it reads the
        // view's own buffer/byteOffset/byteLength rather than trusting the view's element type, so a
        // view built over the wrong memory or with the wrong element size shows up as wrong bytes.
        static bytesHex(v) {
            if (v === null || v === undefined) return '';
            var bytes = v instanceof ArrayBuffer || (globalThis.SharedArrayBuffer && v instanceof SharedArrayBuffer)
                ? new Uint8Array(v)
                : new Uint8Array(v.buffer, v.byteOffset, v.byteLength);
            var out = '';
            for (var i = 0; i < bytes.length; i++) out += bytes[i].toString(16).padStart(2, '0');
            return out;
        }
        // view geometry as JSON so one crossing carries every number that matters. Sizing bugs live
        // here: length is ELEMENTS and byteLength is BYTES, and a view sized by the wrong element size
        // still looks like a well formed TypedArray until its tail is touched.
        static viewInfo(v) {
            if (v === null || v === undefined) return JSON.stringify({ ctor: null });
            var isBuffer = v instanceof ArrayBuffer || (globalThis.SharedArrayBuffer && v instanceof SharedArrayBuffer);
            return JSON.stringify({
                ctor: Object.getPrototypeOf(v)?.constructor?.name ?? '',
                length: isBuffer ? undefined : v.length,
                byteLength: v.byteLength,
                byteOffset: isBuffer ? 0 : v.byteOffset,
                bytesPerElement: isBuffer ? 1 : (v.constructor.BYTES_PER_ELEMENT ?? 1),
                bufferByteLength: isBuffer ? v.byteLength : v.buffer.byteLength,
                onWasmHeap: SpawnJSTests.isOnWasmHeap(v),
                detached: isBuffer ? !!v.detached : !!v.buffer.detached,
                // a live heap view carries the descriptor the reviver reattaches from; a copy does not
                hasHeapViewInfo: !isBuffer && Object.hasOwn(v, '_heapViewInfo'),
            });
        }
        // true when the value's backing buffer IS this app's WebAssembly memory - the difference
        // between a live view of .Net memory and a copy that merely holds the same bytes
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
        // writes into a view from Javascript. A live view of .Net memory must make the .Net array
        // change; a copy must not.
        static writeElement(v, index, value) { v[index] = value; }
        // BigInt64Array/BigUint64Array reject a plain number, so they need their own writer
        static writeElementBig(v, index, value) { v[index] = BigInt(value); }
        static readElement(v, index) { return Number(v[index]); }

        // ******************************************************************************************
        // ARGUMENT CAPTURE - the int-key (call argument) marshalling path
        // ******************************************************************************************
        //
        // .Net writes a property by STRING key and a call argument by INT key, and those are separate
        // marshaller overloads that can and do drift apart. capture() holds whatever arrived as call
        // arguments so the int-key path can be inspected with the same instruments as the string path.
        static capture(...args) {
            SpawnJSTests.captured = args;
            return args.length;
        }
        static capturedAt(index) { return SpawnJSTests.captured[index]; }
        static capturedDescribe(index) { return SpawnJSTests.describe(SpawnJSTests.captured[index]); }
        static capturedJson(index) { return SpawnJSTests.json(SpawnJSTests.captured[index]); }
        static capturedStr(index) { return SpawnJSTests.str(SpawnJSTests.captured[index]); }
        static capturedElements(index) { return SpawnJSTests.elements(SpawnJSTests.captured[index]); }
        static capturedViewInfo(index) { return SpawnJSTests.viewInfo(SpawnJSTests.captured[index]); }
        static capturedBytesHex(index) { return SpawnJSTests.bytesHex(SpawnJSTests.captured[index]); }
        static clearCaptured() { SpawnJSTests.captured = []; return true; }

        // ******************************************************************************************
        // FIXTURES - genuine Javascript values for the JS -> .Net direction
        // ******************************************************************************************

        static undefinedValue() { return undefined; }
        static nullValue() { return null; }
        static number(n) { return n; }
        static string(s) { return s; }
        static bool(b) { return b; }
        // Number(str) so the test can ask for values a .Net double literal cannot express exactly,
        // and for NaN / Infinity / -0
        static numberFrom(str) { return Number(str); }
        static bigIntFrom(str) { return BigInt(str); }
        static dateFrom(ms) { return new Date(ms); }
        static dateStringFrom(ms) { return new Date(ms).toISOString(); }
        // csv -> a real JS array of numbers
        static numberArray(csv) { return csv === '' ? [] : csv.split(',').map(Number); }
        static stringArray(csv) { return csv === '' ? [] : csv.split(','); }
        static boolArray(csv) { return csv === '' ? [] : csv.split(',').map(s => s === 'true'); }
        // an array holding null and undefined, to prove element-level absence survives the crossing
        static sparseArray() { return [1, null, undefined, 4]; }
        // kind is a TypedArray constructor name; BigInt views take BigInt elements
        static typedArray(kind, csv) {
            var ctor = globalThis[kind];
            if (!ctor) throw new Error(`${kind} is not available in this host`);
            var parts = csv === '' ? [] : csv.split(',');
            var isBig = kind === 'BigInt64Array' || kind === 'BigUint64Array';
            return ctor.from(parts, s => isBig ? BigInt(s) : Number(s));
        }
        static arrayBufferOf(byteCount) { return new ArrayBuffer(byteCount); }
        static dataViewOf(byteCount) { return new DataView(new ArrayBuffer(byteCount)); }
        static objectFromJson(json) { return JSON.parse(json); }
        // an object whose member is PRESENT but undefined, vs one that is simply absent. .Net reads of
        // both should agree, and a nullable target must report null for each.
        static objectWithUndefinedMember() { return { present: 1, absent: undefined }; }
        static objectWithNullMember() { return { present: 1, absent: null }; }
        static nullPrototypeObject() { var o = Object.create(null); o.a = 1; return o; }
        static mapOf(json) { return new Map(Object.entries(JSON.parse(json))); }
        static setOf(csv) { return new Set(csv === '' ? [] : csv.split(',')); }
        static errorOf(kind, message) {
            var ctor = globalThis[kind] ?? Error;
            return new ctor(message);
        }
        static symbolOf(description) { return Symbol(description); }
        // a Number/String/Boolean OBJECT rather than a primitive - the boxed forms a web API can hand
        // back, which typeof reports as "object"
        static boxedNumber(n) { return new Number(n); }
        static boxedString(s) { return new String(s); }

        // functions, so a .Net delegate read and a JS function write can be told apart
        static identityFunction() { return (v) => v; }
        static adderFunction() { return (a, b) => a + b; }
        // invokes a function .Net handed to Javascript, with Javascript-native arguments, and reports
        // the result. This is the only way to prove a Callback/Delegate marshalled INTO Javascript is
        // actually callable there.
        static invoke(fn, ...args) { return fn(...args); }
        static invokeDescribe(fn, ...args) { return SpawnJSTests.describe(fn(...args)); }

        // ******************************************************************************************
        // PROMISES - including the rejection shapes
        // ******************************************************************************************
        //
        // A rejected promise can carry an Error, a string, a plain object, or nothing at all, and the
        // .Net side has to turn each into an exception message without losing the reason or hanging.
        static resolvedPromise(value) { return Promise.resolve(value); }
        static resolvedPromiseVoid() { return Promise.resolve(); }
        static rejectedPromiseError(message) { return Promise.reject(new Error(message)); }
        static rejectedPromiseTypeError(message) { return Promise.reject(new TypeError(message)); }
        static rejectedPromiseString(message) { return Promise.reject(message); }
        static rejectedPromiseObject() { return Promise.reject({ code: 42, message: 'object rejection' }); }
        static rejectedPromiseNull() { return Promise.reject(null); }
        static rejectedPromiseUndefined() { return Promise.reject(undefined); }
        // async so the rejection happens on a later turn of the event loop, not synchronously
        static async asyncThrow(message) { await Promise.resolve(); throw new Error(message); }
        static async asyncReturn(value) { await Promise.resolve(); return value; }
        static async asyncReturnLate(value, ms) {
            await new Promise(r => setTimeout(r, ms));
            return value;
        }
        // throws synchronously rather than returning a rejected promise
        static throwSync(message) { throw new Error(message); }
        // resolves a promise .Net created, after a delay, so the .Net -> JS Promise direction can be
        // observed settling rather than only being constructed
        static settleLater(promise, ms) {
            return promise.then(
                v => `resolved:${SpawnJSTests.str(v)}`,
                e => `rejected:${SpawnJSTests.str(e && e.message !== undefined ? e.message : e)}`
            );
        }
        // reports how a promise .Net wrote settled, as a plain string
        static promiseOutcome(promise) {
            return Promise.resolve(promise).then(
                v => `resolved:${SpawnJSTests.str(v)}`,
                e => `rejected:${SpawnJSTests.str(e && e.message !== undefined ? e.message : e)}`
            );
        }

        // ******************************************************************************************
        // SCRATCH TARGETS
        // ******************************************************************************************

        // a fresh plain object for .Net to write members into, kept off globalThis so a stale test
        // value can never be mistaken for a fresh write
        static newTarget() { return {}; }
        static reset() {
            SpawnJSTests.captured = [];
            SpawnJSTests.callbackLog = [];
            return true;
        }
    }
    globalThis.SpawnJSTests = SpawnJSTests;
})();
