using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.Blazor.Demo;
using SpawnDev.SpawnJS.JSObjects;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSpawnJSRuntime(out var JS);



var mybytes = new int[] { 1, 2, 3, 45, 3, 65, 47, 67, 56, 4, 54, 35, 35, 23, 78 };
using var memoryView = HeapView.Create(mybytes, false);



JS.Set("_heapView1", mybytes);

mybytes[0] = 255;

JS.Set("_heapView2", mybytes);




builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().SpawnJSRunAsync();
