#:property LangVersion=latest

// Classifies every ported wrapper type as core or browser-only, using MDN's browser-compat-data (BCD)
// cross referenced against what the globals probe actually measured on each host.
//
//   dotnet run Tools/ClassifyGlobals.cs -- <node-probe.txt> <browser-probe.txt>
//   dotnet run Tools/ClassifyGlobals.cs -- --coverage        report BCD's nodejs coverage only
//
// The probe files are the "GLOBAL: Name|yes" output of TestsShared/GlobalsProbe.cs, captured from the
// console host (--probe-globals) and the browser (SpawnJS.TestRunner GlobalsProbe --verbose).
//
// Why both sources: the probe measures one host configuration - it cannot see a capability the page was
// not isolated for, nor one a runtime only gains through an npm package. BCD knows what the platforms
// support in general but cannot see npm either. Neither is authoritative alone, so the useful output is
// where they AGREE, and an explicit list of where they do not.

using System.Text.Json;

var bcdUrl = "https://unpkg.com/@mdn/browser-compat-data/data.json";
var cache = Path.Combine(Path.GetTempPath(), "mdn-browser-compat-data.json");

if (!File.Exists(cache) || new FileInfo(cache).Length < 1_000_000)
{
    Console.WriteLine($"downloading {bcdUrl} ...");
    using var http = new HttpClient { Timeout = TimeSpan.FromMinutes(5) };
    var bytes = await http.GetByteArrayAsync(bcdUrl);
    await File.WriteAllBytesAsync(cache, bytes);
    Console.WriteLine($"cached {bytes.Length / 1024 / 1024} MB at {cache}");
}

using var doc = JsonDocument.Parse(await File.ReadAllBytesAsync(cache));
if (!doc.RootElement.TryGetProperty("api", out var api))
{
    Console.Error.WriteLine("BCD data has no 'api' section");
    return 1;
}

// A support entry is either an object, or an array of objects (ordered most recent first). version_added
// is a version string when supported, false when not, and null when unknown.
static bool? Supported(JsonElement support, string platform)
{
    if (!support.TryGetProperty(platform, out var entry)) return null;
    if (entry.ValueKind == JsonValueKind.Array) entry = entry.EnumerateArray().FirstOrDefault();
    if (entry.ValueKind != JsonValueKind.Object) return null;
    if (!entry.TryGetProperty("version_added", out var added)) return null;
    return added.ValueKind switch
    {
        JsonValueKind.False => false,
        JsonValueKind.Null => null,
        JsonValueKind.String => true,
        JsonValueKind.True => true,
        _ => null,
    };
}

static JsonElement? SupportOf(JsonElement api, string name)
{
    if (!api.TryGetProperty(name, out var type)) return null;
    if (!type.TryGetProperty("__compat", out var compat)) return null;
    if (!compat.TryGetProperty("support", out var support)) return null;
    return support;
}

// --coverage answers "does BCD actually carry nodejs data for Web APIs" before anything is built on it
if (args.Length > 0 && args[0] == "--coverage")
{
    int total = 0, withNode = 0, nodeYes = 0, nodeNo = 0;
    foreach (var entry in api.EnumerateObject())
    {
        var support = SupportOf(api, entry.Name);
        if (support == null) continue;
        total++;
        var node = Supported(support.Value, "nodejs");
        if (node == null) continue;
        withNode++;
        if (node == true) nodeYes++; else nodeNo++;
    }
    Console.WriteLine($"BCD api entries with __compat.support : {total}");
    Console.WriteLine($"  carrying a nodejs key               : {withNode}");
    Console.WriteLine($"    nodejs supported                  : {nodeYes}");
    Console.WriteLine($"    nodejs explicitly unsupported     : {nodeNo}");
    return 0;
}

if (args.Length < 2)
{
    Console.Error.WriteLine("usage: ClassifyGlobals <node-probe.txt> <browser-probe.txt> | --coverage");
    return 1;
}

static Dictionary<string, bool> LoadProbe(string path)
{
    var map = new Dictionary<string, bool>(StringComparer.Ordinal);
    foreach (var line in File.ReadLines(path))
    {
        var t = line.Trim();
        var at = t.IndexOf("GLOBAL: ", StringComparison.Ordinal);
        if (at < 0) continue;
        var rest = t[(at + 8)..];
        var bar = rest.IndexOf('|');
        if (bar < 0) continue;
        map[rest[..bar]] = rest[(bar + 1)..].StartsWith("yes", StringComparison.Ordinal);
    }
    return map;
}

// The wrappers were written from MDN and most carry the page link in their summary, which is a far better
// join key than the C# type name: it is what the author actually referenced, so it survives any case
// where the C# name and the MDN interface name differ. The URL shape is itself a signal - a
// Web/JavaScript/Reference/Global_Objects page is a language builtin and so is core by definition,
// regardless of host.
var repoRoot = FindUp(AppContext.BaseDirectory, d => Directory.GetFiles(d, "*.slnx").Length > 0)
    ?? Directory.GetCurrentDirectory();
var wrapperDir = Path.Combine(repoRoot, "SpawnDev.SpawnJS", "JSObjects");
var mdnPaths = new Dictionary<string, string>(StringComparer.Ordinal);
if (Directory.Exists(wrapperDir))
{
    var link = new System.Text.RegularExpressions.Regex(@"developer\.mozilla\.org/en-US/docs/([^\s<""]+)");
    foreach (var file in Directory.GetFiles(wrapperDir, "*.cs", SearchOption.AllDirectories))
    {
        var m = link.Match(File.ReadAllText(file));
        if (m.Success) mdnPaths[Path.GetFileNameWithoutExtension(file)] = m.Groups[1].Value.TrimEnd('.', ',', ')');
    }
}
Console.WriteLine($"MDN links found in {mdnPaths.Count} wrapper files");

static string? InterfaceFromMdn(string mdnPath)
{
    var parts = mdnPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
    // Web/API/<Interface>[/<member>]
    if (parts.Length >= 3 && parts[0] == "Web" && parts[1] == "API") return parts[2];
    return null;
}
static bool IsLanguageBuiltin(string mdnPath) => mdnPath.StartsWith("Web/JavaScript/", StringComparison.Ordinal);

var measuredNode = LoadProbe(args[0]);
var measuredBrowser = LoadProbe(args[1]);
var names = measuredNode.Keys.Intersect(measuredBrowser.Keys).OrderBy(n => n, StringComparer.Ordinal).ToList();

var rows = new List<(string Name, bool MNode, bool MBrowser, bool? BNode, bool BKnown, string Verdict, string Why)>();

foreach (var name in names)
{
    mdnPaths.TryGetValue(name, out var mdnPath);
    // prefer the interface the author linked; fall back to the C# type name
    var bcdKey = (mdnPath != null ? InterfaceFromMdn(mdnPath) : null) ?? name;
    var support = SupportOf(api, bcdKey);
    var bcdKnown = support != null;
    bool? bcdNode = support == null ? null : Supported(support.Value, "nodejs");
    // Does ANY browser implement an interface called exactly this? Distinguishes a dictionary that no
    // runtime ever instantiates from a real interface the probe's Chromium simply lacks.
    //
    // This looks up the type's OWN name, deliberately NOT the MDN-derived key. An options type links to
    // the constructor page of the interface that consumes it - BlobOptions points at Web/API/Blob/Blob -
    // so the derived key is Blob, which is of course browser-supported, and every options bag in the
    // library would be misfiled as a browser interface. A real interface has a BCD entry under its own
    // name; a dictionary does not.
    var ownSupport = SupportOf(api, name);
    var bcdAnyBrowser = ownSupport != null &&
        (Supported(ownSupport.Value, "chrome") == true
      || Supported(ownSupport.Value, "firefox") == true
      || Supported(ownSupport.Value, "safari") == true);
    var mNode = measuredNode[name];
    var mBrowser = measuredBrowser[name];

    string verdict, why;
    if (mdnPath != null && IsLanguageBuiltin(mdnPath))
    {
        verdict = "core"; why = "Javascript language builtin (MDN Web/JavaScript)";
    }
    else if (mNode && mBrowser)
    {
        verdict = "core"; why = "measured present on both hosts";
    }
    else if (!mNode && !mBrowser && !bcdAnyBrowser)
    {
        // Absent as a global on BOTH hosts, and no browser claims to implement an interface by that name.
        // That is a dictionary or options bag - a plain data shape with no runtime presence anywhere - so
        // it is host agnostic and belongs in core.
        //
        // This deliberately keys off the MEASUREMENT rather than off whether BCD knows the name. Keying it
        // off BCD knowledge misfiled every options type that happens to have an MDN page (BlobOptions,
        // AddEventListenerOptions, GPUBlendComponent...) as browser-only, because knowing the name says
        // nothing about whether anything instantiates it at runtime.
        verdict = "core"; why = "no runtime global on either host and no browser implements it; plain data shape";
    }
    else if (!mNode && !mBrowser && bcdAnyBrowser)
    {
        // A real interface that some browser implements but Chromium does not - so the probe could not see
        // it. Rare, and genuinely browser side.
        verdict = "browser"; why = "implemented by some browser but not by the probe's Chromium";
    }
    else if (bcdNode == true)
    {
        verdict = "core"; why = mNode ? "BCD: node supported" : "BCD: node supported (probe missed it)";
    }
    else if (mNode && !mBrowser)
    {
        verdict = "core"; why = "measured on node; absent in browser is a page configuration artifact";
    }
    else if (bcdNode == false)
    {
        verdict = "browser"; why = "BCD: node explicitly unsupported";
    }
    else
    {
        verdict = "browser?"; why = bcdKnown ? "BCD has no node data; browser only by measurement" : "unknown to BCD; browser only by measurement";
    }
    rows.Add((name, mNode, mBrowser, bcdNode, bcdKnown, verdict, why));
}

var outPath = Path.Combine(Path.GetTempPath(), "spawnjs-globals-classification.csv");
await File.WriteAllLinesAsync(outPath,
    new[] { "name,measured_node,measured_browser,bcd_node,bcd_known,verdict,why" }
    .Concat(rows.Select(r => $"{r.Name},{r.MNode},{r.MBrowser},{(r.BNode?.ToString() ?? "")},{r.BKnown},{r.Verdict},\"{r.Why}\"")));

foreach (var group in rows.GroupBy(r => r.Verdict).OrderByDescending(g => g.Count()))
{
    Console.WriteLine($"{group.Key,-10} {group.Count()}");
}
Console.WriteLine();
Console.WriteLine("disagreements worth a human look:");
foreach (var r in rows.Where(r => r.BNode == true && !r.MNode).Take(30))
{
    Console.WriteLine($"  BCD says node supports {r.Name}, probe did not find it");
}
Console.WriteLine();
Console.WriteLine($"full table: {outPath}");
return 0;

static string? FindUp(string start, Func<string, bool> predicate)
{
    var dir = new DirectoryInfo(start);
    while (dir != null)
    {
        if (predicate(dir.FullName)) return dir.FullName;
        dir = dir.Parent;
    }
    return null;
}
