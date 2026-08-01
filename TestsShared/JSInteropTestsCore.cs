using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;

using Function = SpawnDev.SpawnJS.JSObjects.Function;

namespace TestsShared
{
    /// <summary>
    /// .Net ➡️ JS marshalling and SpawnJSHandle lifetime tests
    /// </summary>
    public class JSInteropTestsCore(SpawnJSRuntime JS)
    {
        [SpawnJSTest]
        public async Task SetGetTest()
        {
            var testString = "Hello!";
            JS.Set("_my_test_value", testString);
            var readBack = JS.Get<string>("_my_test_value");
            if (readBack != testString) throw new Exception("Readback failed");
        }
        /// <summary>
        /// Has must resolve dotted paths like Get/Set/Call do. It used to compile to
        /// <c>'a.b' in globalThis</c>, so a capability check such as Has("navigator.gpu") answered
        /// false on a host that has it - silently, with no exception. HasDirect keeps the literal
        /// `in` behaviour, which is what the raw operator means.
        /// </summary>
        [SpawnJSTest]
        public async Task HasResolvesDottedPathTest()
        {
            var root = "_spawnjs_has_test";
            var child = JS.New("Object");
            child.Set("inner", "value");
            JS.Set(root, child);
            try
            {
                if (!JS.Has(root)) throw new Exception("Has failed on a plain property");
                if (!JS.Has($"{root}.inner")) throw new Exception("Has did not resolve a dotted path");
                if (JS.Has($"{root}.missing")) throw new Exception("Has returned true for a missing dotted path");
                if (JS.Has("_spawnjs_absent.inner")) throw new Exception("Has returned true for a path whose root is absent");
                // IsUndefined is defined as !Has, so it inherits path resolution
                if (JS.IsUndefined($"{root}.inner")) throw new Exception("IsUndefined said a present dotted path was undefined");
                if (!JS.IsUndefined($"{root}.missing")) throw new Exception("IsUndefined said a missing dotted path was defined");
                // HasDirect is the `in` operator: "root.inner" is a literal key that does not exist
                if (JS.HasDirect($"{root}.inner")) throw new Exception("HasDirect resolved a dotted path; it must stay literal");
                if (!JS.HasDirect(root)) throw new Exception("HasDirect failed on a literal property name");
            }
            finally
            {
                JS.Delete(root);
            }
        }

        [SpawnJSTest]
        public async Task SpawnJSObjectMarshallerTest()
        {
            HostCapabilities.RequireBrowser();
            using var window = JS.Get<Window>("window");
            var testString = window.Origin;
            if (string.IsNullOrEmpty(testString)) throw new Exception("Readback failed");
        }
        [SpawnJSTest]
        public async Task ListMarshallerTest()
        {
            var data = new List<string> { "Hello", "world" };
            JS.Set("_my_test_value", data);
            var readBack = JS.Get<List<string>>("_my_test_value");
            if (readBack == null) throw new Exception("Readback is null");
            if (readBack.Count != data.Count) throw new Exception("Readback count !=");
            for (var i = 0; i < readBack.Count; i++)
            {
                if (readBack[i] != data[i]) throw new Exception("List item does not match");
            }
        }
        [SpawnJSTest]
        public async Task ArrayMarshallerTest()
        {
            var data = new string[] { "Hello", "world" };
            JS.Set("_my_test_value", data);
            var readBack = JS.Get<string[]>("_my_test_value");
            if (readBack == null) throw new Exception("Readback is null");
            if (readBack.Length != data.Length) throw new Exception("Readback count !=");
            for (var i = 0; i < readBack.Length; i++)
            {
                if (readBack[i] != data[i]) throw new Exception("List item does not match");
            }
        }
        /// <summary>
        /// Verifies that JSObjectHandle successfully keeps the SpawnJSObjectReference working after an isntance with a shared handle is disposed
        /// </summary>
        [SpawnJSTest]
        public async Task JSObjectHandleTest()
        {
            HostCapabilities.RequireBrowser();
            using var window1 = JS.Get<Window>("window");
            var jsType = window1.JSRef!.JSHandle.JSType;
            var jsClass = window1.JSRef!.JSHandle.JSClass;
            var window2 = JS.Get<Window>("window");
            var testString1 = window1.Origin;
            var testString2 = window2.Origin;
            window2.Dispose();
            var testString3 = window1.Origin;
            if (string.IsNullOrEmpty(testString3)) throw new Exception("Readback failed");
        }
        /// <summary>
        /// A volatile SpawnJSHandle borrows its parent, it does not own it. Disposing the volatile handle
        /// must leave the parent fully usable - every multi-read marshaller (List, Array, ConstructorNames)
        /// and every multi-argument JS ➡️ .Net call depends on it.
        /// </summary>
        [SpawnJSTest]
        public async Task VolatileHandleDoesNotDisposeParentTest()
        {
            using var parent = JS.NewJSArray();
            Reflect.Set(parent.JSObject!, 0, "first");
            Reflect.Set(parent.JSObject!, 1, "second");
            // borrow the parent, then release the borrow
            using (var borrowed = new SpawnJSHandle(parent, 0, true))
            {
                if (borrowed.JSType != "string") throw new Exception($"Expected string, got '{borrowed.JSType}'");
            }
            if (parent.IsDisposed) throw new Exception("Disposing a volatile handle disposed its unowned parent");
            // the parent must still be borrowable after the first borrow was released
            using var borrowedAgain = new SpawnJSHandle(parent, 1, true);
            if (borrowedAgain.JSType != "string") throw new Exception($"Parent unusable after volatile dispose, got '{borrowedAgain.JSType}'");
        }
        /// <summary>
        /// Two handles on the same Javascript object share one refcount. Disposing one must not dispose
        /// the underlying JSObject while the other still holds it, and the last release must free it.
        /// </summary>
        [SpawnJSTest]
        public async Task HandleRefCountTest()
        {
            HostCapabilities.RequireBrowser();
            using var window = JS.Get<Window>("window");
            var handle1 = window.JSRef!.JSHandle;
            var before = SpawnJSHandle.LiveObjectCount;
            // a clone points at the same Javascript object
            var handle2 = handle1.Clone();
            handle2.Dispose();
            if (handle1.IsDisposed) throw new Exception("Disposing a clone disposed the original handle");
            // original still resolves
            if (string.IsNullOrEmpty(window.Origin)) throw new Exception("Original handle unusable after clone disposed");
            if (SpawnJSHandle.LiveObjectCount != before) throw new Exception($"Live object count leaked: {before} -> {SpawnJSHandle.LiveObjectCount}");
        }
        [SpawnJSTest]
        public async Task ConstructorNamesTests()
        {
            HostCapabilities.RequireBrowser();
            using var window = JS.Get<Window>("window");
            var constructorNames = window.JSRef!.JSHandle.ConstructorNames;
            if (!constructorNames.SequenceEqual(["Window", "EventTarget", "Object"])) throw new Exception("Failed");
        }
        /// <summary>
        /// Creates a Volatile SpawnJSHandle to test undefined detection in SpawnJSHandle.<br/>
        /// Volatile SpawnJSHandle is used in Marshallers allowing marshalling of undefined to supporting types.
        /// Volatile SpawnJSHandle (unlike non-Volatile) can hold Javascript values that JSObject cannot and
        /// it does so by accessing the data as a `someObject[someKey]` where non-Volatile require a JSObject
        /// which allows holding `undefined`, Number literals, etc... any other data type
        /// </summary>
        [SpawnJSTest]
        public async Task SpawnJSHandleValueTypeTest()
        {
            using var jsHandle = new SpawnJSHandle(JS.JSHandle, "undefined");
            if (!jsHandle.IsUndefined) throw new Exception("Undefined test failed");
        }
        [SpawnJSTest(Timeout = 30000)]
        public async Task PromiseTest()
        {
            HostCapabilities.RequireGlobal("waitForTask");
            var value = "hello!";
            using var promise = new Promise(async (Function resolve, Function reject) =>
            {
                await Task.Delay(500);
                resolve.CallVoid(null, value);
            });
            var readback = await JS.CallAsync<string>("waitForTask", promise);
            if (readback != value) throw new Exception("Readback Failed");
        }

        /// <summary>
        /// Reading a Javascript number into a Nullable&lt;T&gt; over the frame path.<br/>
        /// A dotted identifier is not eligible for the fast property path, so it goes through NetRun and
        /// ReadFrameResult - the frame fast-path selects on the DECLARED type, and before the fix a
        /// Nullable&lt;int&gt; matched none of the primitive types there and fell through to a marshaller
        /// handle built over the scratch buffer at this call's offset, where the call's own first argument
        /// (the target object) still sat. That threw "Value is not a Number: [object Window]" on
        /// JS.Get&lt;int?&gt;("navigator.hardwareConcurrency"). This is the regression guard.
        /// </summary>
        [SpawnJSTest]
        public async Task NullableNumberFromDottedPathTest()
        {
            var root = "_spawnjs_nullable_test";
            var child = JS.New("Object");
            child.Set("i", 12);
            child.Set("d", 3.5);
            child.Set("b", true);
            child.Set("nothing", (object?)null);
            JS.Set(root, child);
            try
            {
                var i = JS.Get<int?>($"{root}.i");
                if (i != 12) throw new Exception($"Get<int?> expected 12, got {(i.HasValue ? i.Value.ToString() : "null")}");

                var d = JS.Get<double?>($"{root}.d");
                if (d != 3.5) throw new Exception($"Get<double?> expected 3.5, got {(d.HasValue ? d.Value.ToString() : "null")}");

                var b = JS.Get<bool?>($"{root}.b");
                if (b != true) throw new Exception($"Get<bool?> expected true, got {(b.HasValue ? b.Value.ToString() : "null")}");

                var l = JS.Get<long?>($"{root}.i");
                if (l != 12L) throw new Exception($"Get<long?> expected 12, got {(l.HasValue ? l.Value.ToString() : "null")}");

                var f = JS.Get<float?>($"{root}.d");
                if (f != 3.5f) throw new Exception($"Get<float?> expected 3.5, got {(f.HasValue ? f.Value.ToString() : "null")}");

                // a null Javascript value read as a nullable is null, not an exception and not zero
                var missing = JS.Get<int?>($"{root}.nothing");
                if (missing != null) throw new Exception($"Get<int?> of a null property expected null, got {missing.Value}");
            }
            finally
            {
                JS.Delete(root);
            }
        }

        /// <summary>
        /// Reading a Javascript number into a numeric type the frame fast-path does not name (short here).
        /// It falls through to the marshaller, and before the fix that handle addressed the scratch buffer
        /// at this call's offset - the call's first argument - rather than the number the call returned.
        /// Now the payload is stored into the scratch slot first, so any INumber&lt;T&gt; resolves correctly.
        /// </summary>
        [SpawnJSTest]
        public async Task NonFastNumericFromDottedPathTest()
        {
            var root = "_spawnjs_narrow_numeric_test";
            var child = JS.New("Object");
            child.Set("n", 7);
            JS.Set(root, child);
            try
            {
                var s = JS.Get<short>($"{root}.n");
                if (s != 7) throw new Exception($"Get<short> expected 7, got {s}");

                var s2 = JS.Get<short?>($"{root}.n");
                if (s2 != 7) throw new Exception($"Get<short?> expected 7, got {(s2.HasValue ? s2.Value.ToString() : "null")}");
            }
            finally
            {
                JS.Delete(root);
            }
        }
    }
}
