using SpawnDev.BlazorJS;
using BlazorBrowserDemo;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using SpawnDev.SpawnJS;
using TestsShared;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddSingleton(sp => (IJSInProcessRuntime)sp.GetRequiredService<IJSRuntime>());
// BlazorJS is referenced only so the interop benchmark can measure it against SpawnJS head to head
builder.Services.AddBlazorJSRuntime();

var app = builder.Build();

// SpawnJSRuntime.Instance is the singleton the whole library reads through, so it has to exist
// before a test touches a marshaller.
var JS = new SpawnJSRuntime();
JS.Verbose = false;

try
{
    // `?bench` runs the SpawnJS vs BlazorJS interop benchmark instead of the test suite
    if (JS.Get<string?>("location.search")?.Contains("bench") == true)
    {
        var blazorJS = app.Services.GetRequiredService<SpawnDev.BlazorJS.BlazorJSRuntime>();
        BlazorBrowserDemo.InteropBenchmark.Run(JS, blazorJS);
        Console.WriteLine("RESULTS: Failed: 0 Passed: 0 Skipped: 0 Ran: 0");
    }
    else
    {
        // `?filter=Name` in the url runs only the matching tests
        await TestSuiteRunner.RunAllAsync(TestSuiteRunner.FilterFromLocation());
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Failed: {ex.ToString()}");
}

await app.RunAsync();
