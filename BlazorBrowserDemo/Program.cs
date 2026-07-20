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

var app = builder.Build();

// SpawnJSRuntime.Instance is the singleton the whole library reads through, so it has to exist
// before a test touches a marshaller.
var JS = new SpawnJSRuntime();
JS.Verbose = false;

try
{
    // `?filter=Name` in the url runs only the matching tests
    await TestSuiteRunner.RunAllAsync(TestSuiteRunner.FilterFromLocation());
}
catch (Exception ex)
{
    Console.WriteLine($"Failed: {ex.ToString()}");
}

await app.RunAsync();
