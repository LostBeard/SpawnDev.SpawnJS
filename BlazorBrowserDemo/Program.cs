using BlazorBrowserDemo;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using TestsShared;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddSingleton(sp => (IJSInProcessRuntime)sp.GetRequiredService<IJSRuntime>());

builder.Services.AddSingleton<JSInteropTestsCore>();

var app = builder.Build();

var testService = app.Services.GetRequiredService<JSInteropTestsCore>();

try
{
    await testService.RunAsync();
    Console.WriteLine("Success");
}
catch (Exception ex)
{
    Console.WriteLine($"Failed: {ex.ToString()}");
}

await app.RunAsync();
