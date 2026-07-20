using System;
using System.Linq;
using System.Threading.Tasks;
using SpawnDev.SpawnJS;
using TestsShared;

// Non-browser host. Runs the same suite the browser demo runs, so any test that fails here is
// telling us the code under it is browser-specific rather than host-neutral.
Console.WriteLine("WasmConsoleDemo starting");
var JS = new SpawnJSRuntime();
JS.Verbose = false;

// --probe-host identifies the Javascript engine this wasm console app is actually running on.
// It matters because a Node host can load npm packages (the `webgpu` Dawn binding, for example),
// which decides whether an API is genuinely browser-only or merely not-built-in.
if (args.Length > 0 && args[0] == "--probe-host")
{
    // NOTE: Has() is a literal key check ('a.b' in globalThis) and does NOT resolve dotted paths.
    // Get() does, via pathObjectInfo, so read through Get with a null-conditional path instead.
    void Report(string label, string path)
    {
        string? value;
        try { value = JS.Get<string?>(path); }
        catch (Exception ex) { value = $"(error: {ex.GetType().Name})"; }
        Console.WriteLine($"HOST: {label}={value ?? "(absent)"}");
    }

    Console.WriteLine($"HOST: process={(JS.Has("process") ? "present" : "absent")}");
    Report("process.version", "process?.version");
    Report("process.versions.node", "process?.versions?.node");
    Report("process.versions.v8", "process?.versions?.v8");
    Report("process.platform", "process?.platform");
    Report("process.argv0", "process?.argv0");
    Console.WriteLine($"HOST: Deno={(JS.Has("Deno") ? "present" : "absent")}");
    Console.WriteLine($"HOST: Bun={(JS.Has("Bun") ? "present" : "absent")}");
    Console.WriteLine($"HOST: require={(JS.Has("require") ? "present" : "absent")}");
    Console.WriteLine($"HOST: window={(JS.Has("window") ? "present" : "absent")}");
    Console.WriteLine($"HOST: navigator={(JS.Has("navigator") ? "present" : "absent")}");
    Report("navigator.userAgent", "navigator.userAgent");
    Console.WriteLine("PROBE-DONE");
    return 0;
}

// --probe-globals reports, for every ported wrapper type, whether a Javascript global of that name
// exists in this host. Run it on both hosts and the difference is the browser-specific set, measured
// rather than classified by hand.
if (args.Length > 0 && args[0] == "--probe-globals")
{
    var wrapperTypes = typeof(SpawnJSObject).Assembly.GetTypes()
        .Where(t => t.IsPublic && !t.IsAbstract && t.Namespace != null && t.Namespace.StartsWith("SpawnDev.SpawnJS.JSObjects"))
        .Select(t => t.Name)
        .Where(n => !n.Contains('`'))
        .Distinct()
        .OrderBy(n => n, StringComparer.Ordinal);
    foreach (var name in wrapperTypes)
    {
        Console.WriteLine($"GLOBAL: {name}|{(JS.Has(name) ? "yes" : "no")}");
    }
    Console.WriteLine("PROBE-DONE");
    return 0;
}

var filter = args.Length > 0 ? args[0] : null;
var failed = await TestSuiteRunner.RunAllAsync(filter);
return failed;
