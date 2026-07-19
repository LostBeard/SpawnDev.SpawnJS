using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.SpawnJSObjects;

namespace TestsShared
{
    public class JSInteropTestsCore(SpawnJSRuntime JS)
    {
        public async Task RunAsync()
        {
            var methods = this.GetType().GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Where(o => o.ReturnType == typeof(Task) && o.Name != nameof(RunAsync)).ToArray();
            var passed = 0;
            var failed = 0;
            var ran = 0;
            Console.WriteLine($"Starting: {methods.Length} tests");
            foreach (var test in methods)
            {
                try
                {
                    ran++;
                    JS.Log($">> Test start: {test.Name}");
                    dynamic result = test.Invoke(this, null)!;
                    await result;
                    JS.Log($">> Test success: {test.Name}");
                    passed++;
                }
                catch (Exception ex)
                {
                    failed++;
                    JS.LogError($"Test failed: {test.Name} {ex.ToString()}");
                }
            }
            Console.WriteLine($"Failed: {failed} Passed: {passed} Ran: {ran}");
        }
        public async Task SetGetTest()
        {
            var testString = "Hello!";
            JS.Set("_my_test_value", testString);
            var readBack = JS.Get<string>("_my_test_value");
            if (readBack != testString) throw new Exception("Readback failed");
        }
        public async Task SpawnJSObjectMarshallerTest()
        {
            using var window = JS.Get<Window>("window");
            var testString = window.Origin;
            if (string.IsNullOrEmpty(testString)) throw new Exception("Readback failed");
        }
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
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task JSObjectHandleTest()
        {
            using var window1 = JS.Get<Window>("window");
            var window2 = JS.Get<Window>("window");
            var testString1 = window1.Origin;
            var testString2 = window2.Origin;
            window2.Dispose();
            var testString3 = window1.Origin;
            if (string.IsNullOrEmpty(testString3)) throw new Exception("Readback failed");
        }

        public async Task JSToNetCallingTest()
        {
            JS.SetHandler("apples", new Func<string, string>((msg) => {

                return $"ok1: {msg}";
            }));

        }

        public async Task CallbackTest()
        {
            var func = new Func<string, string>((msg) =>
            {

                return $"ok2: {msg}";
            });
            var cb = new Callback(func);
            JS.Set("_mb1", cb);
        }
    }
}