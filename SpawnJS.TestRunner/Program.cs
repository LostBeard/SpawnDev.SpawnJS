using Microsoft.Playwright;
using System.Diagnostics;
using System.Text.RegularExpressions;

// SpawnJS test harness.
//
//   dotnet run --project SpawnJS.TestRunner                      run everything
//   dotnet run --project SpawnJS.TestRunner -- JSToNet           run tests whose name contains "JSToNet"
//   dotnet run --project SpawnJS.TestRunner -- --headed          watch it in a real browser window
//   dotnet run --project SpawnJS.TestRunner -- --url http://...  use an already running dev server
//   dotnet run --project SpawnJS.TestRunner -- --bench [filter]   run the interop benchmark instead
//   dotnet run --project SpawnJS.TestRunner -- --twin             two runtimes in one page (TwinTests)
//   dotnet run --project SpawnJS.TestRunner -- --bench <case> --profile   CPU profile of the run, self time by function
//
// Exit code is the number of failed tests, so it is usable as a gate.

var filter = "";
var headed = false;
var externalUrl = "";
var verbose = false;
var bench = false;
var twin = false;
var profile = false;
for (var i = 0; i < args.Length; i++)
{
    switch (args[i])
    {
        case "--headed": headed = true; break;
        case "--verbose": verbose = true; break;
        case "--bench": bench = true; break;
        case "--twin": twin = true; break;
        case "--profile": profile = true; break;
        case "--url": externalUrl = ++i < args.Length ? args[i] : ""; break;
        case "--filter": filter = ++i < args.Length ? args[i] : ""; break;
        case "-h":
        case "--help":
            Console.WriteLine("usage: [filter] [--filter <text>] [--headed] [--verbose] [--url <url>] [--bench] [--twin] [--profile]");
            return 0;
        default:
            if (!args[i].StartsWith("-")) filter = args[i];
            break;
    }
}

// The app under test is SpawnDev.SpawnJS.Demo: it runs the suite on startup and writes the
// TEST:/RESULTS: lines this harness parses. (It used to be WasmBrowserDemo, which still targets the
// SpawnJS 1.0 API through TestsShared and no longer compiles.)
var repoRoot = FindRepoRoot();
var demoProject = Path.Combine(repoRoot, "SpawnDev.SpawnJS.Demo", "SpawnDev.SpawnJS.Demo.csproj");
if (!File.Exists(demoProject))
{
    Console.Error.WriteLine($"Could not find SpawnDev.SpawnJS.Demo.csproj (looked in {demoProject})");
    return 1;
}

Process? server = null;
var url = externalUrl;
try
{
    if (string.IsNullOrEmpty(url))
    {
        (server, url) = await StartServerAsync(demoProject);
        if (string.IsNullOrEmpty(url))
        {
            Console.Error.WriteLine("Dev server did not report an app url");
            return 1;
        }
    }
    // Always ask for the suite with ?tests=[filter], even when the filter is empty. The demo app the
    // suite lives in is also TJ's scratch host, and whatever sits at the top of its Program.cs may
    // return before reaching the tests. The parameter makes the suite unconditional instead of
    // dependent on that. The app runs everything when the value is empty.
    // --bench runs the interop benchmark (?bench=) instead of the suite; same TEST:/RESULTS: contract
    var target = $"{url.TrimEnd('/')}/?{(bench ? "bench" : "tests")}={Uri.EscapeDataString(filter)}";
    // --twin boots two runtimes of the app into one page and runs TwinTests in each
    if (twin) target = $"{url.TrimEnd('/')}/twin.html";
    return await RunAsync(target, headed, verbose, profile);
}
finally
{
    if (server != null && !server.HasExited)
    {
        try { server.Kill(entireProcessTree: true); } catch { }
    }
}

// walks up from the assembly location to the folder holding the solution
static string FindRepoRoot()
{
    var dir = AppContext.BaseDirectory;
    while (!string.IsNullOrEmpty(dir))
    {
        if (Directory.GetFiles(dir, "*.slnx").Length > 0 || Directory.GetFiles(dir, "*.sln").Length > 0) return dir;
        dir = Path.GetDirectoryName(dir) ?? "";
    }
    return Directory.GetCurrentDirectory();
}

static async Task<(Process?, string)> StartServerAsync(string demoProject)
{
    Console.WriteLine($"building and starting {Path.GetFileNameWithoutExtension(demoProject)}...");
    var psi = new ProcessStartInfo("dotnet", $"run -c Release --project \"{demoProject}\"")
    {
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
    };
    var process = Process.Start(psi);
    if (process == null) return (null, "");

    var urlFound = new TaskCompletionSource<string>();
    // "App url:" is what a SpawnJS console-style host prints; "Now listening on:" is what the
    // WebAssembly SDK dev server prints. Accept either so the harness works with both hosts.
    var appUrl = new Regex(@"(?:App url|Now listening on):\s*(http://\S+)", RegexOptions.IgnoreCase);
    process.OutputDataReceived += (_, e) =>
    {
        if (e.Data == null) return;
        var match = appUrl.Match(e.Data);
        if (match.Success) urlFound.TrySetResult(match.Groups[1].Value.TrimEnd('/'));
    };
    process.BeginOutputReadLine();
    process.BeginErrorReadLine();

    var completed = await Task.WhenAny(urlFound.Task, Task.Delay(TimeSpan.FromMinutes(3)));
    return (process, completed == urlFound.Task ? urlFound.Task.Result : "");
}

static async Task<int> RunAsync(string url, bool headed, bool verbose, bool profile)
{
    using var playwright = await Playwright.CreateAsync();
    await using var browser = await LaunchAsync(playwright, headed);
    var page = await browser.NewPageAsync();
    // --profile: Chrome's sampling CPU profiler over CDP for the whole run. Interpreted .Net runs inside wasm
    // functions, so the split between those and the named Javascript functions is the first thing it shows.
    ICDPSession? cdp = null;
    if (profile)
    {
        cdp = await page.Context.NewCDPSessionAsync(page);
        await cdp.SendAsync("Profiler.enable");
        await cdp.SendAsync("Profiler.setSamplingInterval", new Dictionary<string, object> { ["interval"] = 50 });
        await cdp.SendAsync("Profiler.start");
    }

    var finished = new TaskCompletionSource<string>();
    var results = new List<string>();
    page.Console += (_, msg) =>
    {
        var text = msg.Text;
        if (text.StartsWith("TEST: "))
        {
            results.Add(text.Substring(6));
        }
        else if (text.StartsWith("RESULTS: "))
        {
            finished.TrySetResult(text.Substring(9));
        }
        else if (verbose || msg.Type == "error")
        {
            Console.WriteLine($"  [{msg.Type}] {text}");
        }
    };
    page.PageError += (_, err) => Console.WriteLine($"  [pageerror] {err}");
    // a console "Failed to load resource" line does not say WHICH resource
    page.Response += (_, response) => { if (verbose && response.Status >= 400) Console.WriteLine($"  [http {response.Status}] {response.Url}"); };

    Console.WriteLine($"running {url}");
    // Navigate only until the document is parsed - NOT network-idle. A test that holds a long-lived
    // connection (a SharedWorker, an open stream) never lets the network go idle, so a network-idle wait
    // would time out here even though the suite runs fine. The real completion signal is the page's own
    // "RESULTS:" console line, awaited via finished.Task below.
    await page.GotoAsync(url, new() { WaitUntil = WaitUntilState.DOMContentLoaded, Timeout = 60000 });
    var completed = await Task.WhenAny(finished.Task, Task.Delay(TimeSpan.FromMinutes(5)));

    Console.WriteLine();
    var failed = 0;
    foreach (var line in results)
    {
        // Name|Result|DurationMs|Detail
        var parts = line.Split('|', 4);
        if (parts.Length < 3) { Console.WriteLine(line); continue; }
        var mark = parts[1] switch { "Success" => "PASS", "Skipped" => "SKIP", _ => "FAIL" };
        if (mark == "FAIL") failed++;
        Console.WriteLine($"  {mark}  {parts[0]} ({parts[2]}ms)");
        if (parts.Length > 3 && !string.IsNullOrWhiteSpace(parts[3])) Console.WriteLine($"        {parts[3]}");
    }
    if (cdp != null) PrintProfile(await cdp.SendAsync("Profiler.stop"));
    Console.WriteLine();
    if (completed != finished.Task)
    {
        Console.WriteLine("TIMED OUT - the suite never reported a summary");
        return Math.Max(1, failed);
    }
    Console.WriteLine(finished.Task.Result);
    return failed;
}

// Self time by function, heaviest first. A wasm function is reported by its name when the module carries a name
// section and by its index otherwise; all wasm is also totalled, since that is where interpreted .Net runs.
static void PrintProfile(System.Text.Json.JsonElement? result)
{
    if (result == null) return;
    var profileJson = result.Value.GetProperty("profile");
    var nodes = profileJson.GetProperty("nodes").EnumerateArray().ToList();
    var samples = profileJson.GetProperty("samples").EnumerateArray().Select(s => s.GetInt32()).ToList();
    var byId = nodes.ToDictionary(n => n.GetProperty("id").GetInt32());
    var self = new Dictionary<string, int>();
    var wasm = 0;
    foreach (var id in samples)
    {
        var frame = byId[id].GetProperty("callFrame");
        var fn = frame.GetProperty("functionName").GetString();
        var scriptUrl = frame.GetProperty("url").GetString() ?? "";
        var isWasm = scriptUrl.StartsWith("wasm://") || scriptUrl.EndsWith(".wasm");
        if (isWasm) wasm++;
        var key = $"{(string.IsNullOrEmpty(fn) ? "(anonymous)" : fn)}  [{(isWasm ? "wasm" : Path.GetFileName(scriptUrl.Split('?')[0]))}]";
        self[key] = self.GetValueOrDefault(key) + 1;
    }
    var total = samples.Count;
    Console.WriteLine();
    Console.WriteLine($"PROFILE: {total} samples, wasm {100.0 * wasm / total:F1}%");
    foreach (var (key, count) in self.OrderByDescending(kv => kv.Value).Take(40))
        Console.WriteLine($"  {100.0 * count / total,5:F1}%  {key}");
}

static async Task<IBrowser> LaunchAsync(IPlaywright playwright, bool headed)
{
    var options = new BrowserTypeLaunchOptions { Headless = !headed };
    try
    {
        // prefer installed Chrome - the bundled chromium build often lags the package version
        return await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = !headed, Channel = "chrome" });
    }
    catch
    {
        return await playwright.Chromium.LaunchAsync(options);
    }
}
