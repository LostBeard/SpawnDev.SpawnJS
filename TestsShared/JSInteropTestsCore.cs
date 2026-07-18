

using Microsoft.JSInterop;
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.SpawnJSObjects;
using System.ComponentModel;

namespace TestsShared
{
    public class JSInteropTestsCore(SpawnJSRuntime JS)
    {
        public async Task RunAsync()
        {
            //Console.WriteLine(">> RunAsync");
            //await Task.Delay(3000);
            //Console.WriteLine("<< RunAsync");
            using var window = JS.Get<Window>("window");
            var testString = "Hello!";
            window.JSRef!.Set("_my_test_value", testString);
            var readBack = window.JSRef!.Get<string>("_my_test_value");
            if (readBack != testString) throw new Exception("Readback failed");
        }
    }
}