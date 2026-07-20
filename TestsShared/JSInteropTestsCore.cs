using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.SpawnJSObjects;

using Function = SpawnDev.SpawnJS.SpawnJSObjects.Function;

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
        [SpawnJSTest]
        public async Task SpawnJSObjectMarshallerTest()
        {
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
            var value = "hello!";
            using var promise = new Promise(async (Function resolve, Function reject) =>
            {
                await Task.Delay(500);
                resolve.CallVoid(null, value);
            });
            var readback = await JS.CallAsync<string>("waitForTask", promise);
            if (readback != value) throw new Exception("Readback Failed");
        }
    }
}
