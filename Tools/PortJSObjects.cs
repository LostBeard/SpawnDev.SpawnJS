#:property LangVersion=latest

// Ports SpawnDev.BlazorJS JSObject wrappers into SpawnDev.SpawnJS.
//
//   dotnet run Tools/PortJSObjects.cs -- Location Storage MediaError
//   dotnet run Tools/PortJSObjects.cs -- --list            report which wrappers are portable
//   dotnet run Tools/PortJSObjects.cs -- --missing         report types referenced but not yet ported
//
// The transform is mechanical because SpawnJSObject mirrors BlazorJS JSObject member for member:
//   using Microsoft.JSInterop;      -> using SpawnDev.SpawnJS;
//   namespace SpawnDev.BlazorJS.*   -> namespace SpawnDev.SpawnJS.JSObjects
//   : JSObject                      -> : SpawnJSObject
//   IJSInProcessObjectReference     -> SpawnJSObjectReference
// Anything a wrapper needs beyond that is reported rather than guessed at.

using System.Text.RegularExpressions;

var repoRoot = FindUp(AppContext.BaseDirectory, d => Directory.GetFiles(d, "*.slnx").Length > 0)
    ?? Directory.GetCurrentDirectory();
var source = Path.Combine(repoRoot, "..", "..", "SpawnDev.BlazorJS", "SpawnDev.BlazorJS", "SpawnDev.BlazorJS", "JSObjects");
source = Path.GetFullPath(source);
var dest = Path.Combine(repoRoot, "SpawnDev.SpawnJS", "JSObjects");

if (!Directory.Exists(source))
{
    Console.Error.WriteLine($"BlazorJS JSObjects folder not found: {source}");
    return 1;
}
Directory.CreateDirectory(dest);

// patterns that mean the wrapper needs design work, not a mechanical copy
var blockers = new (string Name, Regex Pattern)[]
{
    ("ElementRef", new Regex(@"\bElementReference\b")),
    ("Events",     new Regex(@"\bActionEvent\b|\bJSEventCallback\b")),
    ("Union",      new Regex(@"\bUnion<")),
    ("Async",      new Regex(@"Async")),
    ("Callback",   new Regex(@"\bCallback\b|\bActionCallback\b|\bFuncCallback\b")),
    ("DateTime",   new Regex(@"\bDateTime\b")),
    ("EnumString", new Regex(@"\bEnumString\b")),
    ("Undefinable",new Regex(@"\bUndefinable\b")),
    // couples to the Blazor runtime itself rather than to a wrapper - never a mechanical port
    ("BlazorJSRuntime", new Regex(@"\bBlazorJSRuntime\b")),
    // BlazorJS support types that live outside JSObjects/ and have no SpawnJS equivalent yet
    ("EpochDateTime", new Regex(@"\bEpochDateTime\b")),
    ("GPUCoord",   new Regex(@"\bGPUIntegerCoordinate\b|\bGPUSize\d+\b")),
};

var mode = args.FirstOrDefault() ?? "";
if (mode == "--list") { Report(); return 0; }
if (mode == "--missing") { Missing(); return 0; }
if (mode == "--ready") { Ready(printOnly: true); return 0; }
if (mode == "--port-ready")
{
    // port every wrapper whose whole dependency closure is already satisfied, repeatedly,
    // until a pass adds nothing new
    var total = 0;
    for (var pass = 1; ; pass++)
    {
        var batch = Ready(printOnly: false);
        if (batch.Count == 0) break;
        foreach (var name in batch)
        {
            var src = Directory.GetFiles(source, name + ".cs", SearchOption.AllDirectories).First();
            File.WriteAllText(Path.Combine(dest, name + ".cs"), Transform(File.ReadAllText(src)));
        }
        Console.WriteLine($"pass {pass}: ported {batch.Count} ({string.Join(", ", batch.Take(12))}{(batch.Count > 12 ? ", ..." : "")})");
        total += batch.Count;
    }
    Console.WriteLine($"\nported {total} wrappers");
    return 0;
}

var names = args.Where(a => !a.StartsWith("-")).ToArray();
if (names.Length == 0)
{
    Console.WriteLine("usage: PortJSObjects.cs <TypeName> [TypeName...] | --list | --missing");
    return 1;
}

var ported = 0;
foreach (var name in names)
{
    var file = Path.Combine(source, name + ".cs");
    if (!File.Exists(file))
    {
        // wrappers are not all at the top level
        var found = Directory.GetFiles(source, name + ".cs", SearchOption.AllDirectories).FirstOrDefault();
        if (found == null) { Console.WriteLine($"  SKIP  {name} (not found)"); continue; }
        file = found;
    }
    var text = File.ReadAllText(file);
    var hits = blockers.Where(b => b.Pattern.IsMatch(text)).Select(b => b.Name).ToArray();
    if (hits.Length > 0)
    {
        Console.WriteLine($"  SKIP  {name} needs design work: {string.Join(",", hits)}");
        continue;
    }
    File.WriteAllText(Path.Combine(dest, name + ".cs"), Transform(text));
    Console.WriteLine($"  PORT  {name}");
    ported++;
}
Console.WriteLine($"\nported {ported} of {names.Length}");
return 0;

string Transform(string text)
{
    text = Regex.Replace(text, @"^using Microsoft\.JSInterop;\s*\r?\n", "", RegexOptions.Multiline);
    text = Regex.Replace(text, @"^using SpawnDev\.BlazorJS[^\r\n]*;\s*\r?\n", "", RegexOptions.Multiline);
    text = Regex.Replace(text, @"namespace SpawnDev\.BlazorJS(\.\w+)*", "namespace SpawnDev.SpawnJS.JSObjects");
    text = Regex.Replace(text, @"\bIJSInProcessObjectReference\b", "SpawnJSObjectReference");
    // only the base type reference, never a member or a string
    text = Regex.Replace(text, @"(:\s*)JSObject\b", "$1SpawnJSObject");
    text = Regex.Replace(text, @"\bJSObject<", "SpawnJSObject<");
    // Microsoft's IJSInProcessObjectReference.Invoke/InvokeVoid are the same operation as
    // SpawnJS's Call/CallVoid. Normalize rather than carry a legacy alias on the reference type.
    text = Regex.Replace(text, @"\.Invoke<", ".Call<");
    text = Regex.Replace(text, @"\.InvokeVoid\(", ".CallVoid(");
    // SpawnJS's own wrapper types (Error, Promise, Function, Array, Window...) live in a different
    // namespace than the ported ones, so ported files need both.
    var usings = "using SpawnDev.SpawnJS;\r\nusing SpawnDev.SpawnJS.SpawnJSObjects;\r\n";
    // a `global using` must precede every non-global using, so insert after any leading global usings
    var lastGlobal = Regex.Matches(text, @"^global using [^\r\n]*;\s*\r?\n", RegexOptions.Multiline).LastOrDefault();
    if (lastGlobal != null)
    {
        var at = lastGlobal.Index + lastGlobal.Length;
        text = text[..at] + usings + text[at..];
    }
    else
    {
        text = usings + text;
    }
    return text;
}

void Report()
{
    var files = Directory.GetFiles(source, "*.cs", SearchOption.AllDirectories);
    var clean = new List<string>();
    var counts = new Dictionary<string, int>();
    foreach (var f in files)
    {
        var text = File.ReadAllText(f);
        var hits = blockers.Where(b => b.Pattern.IsMatch(text)).Select(b => b.Name).ToList();
        if (hits.Count == 0) clean.Add(Path.GetFileNameWithoutExtension(f));
        foreach (var h in hits) counts[h] = counts.GetValueOrDefault(h) + 1;
    }
    Console.WriteLine($"total wrappers   : {files.Length}");
    Console.WriteLine($"mechanically portable: {clean.Count}");
    Console.WriteLine($"needs design work    : {files.Length - clean.Count}");
    foreach (var kv in counts.OrderByDescending(o => o.Value)) Console.WriteLine($"    {kv.Key,-12} {kv.Value}");
}

// wrappers that are mechanically portable AND whose every JSObject dependency is already ported.
// this is the correct batch unit: the port closes over dependencies, not over single files.
List<string> Ready(bool printOnly)
{
    var available = Directory.GetFiles(source, "*.cs", SearchOption.AllDirectories)
        .GroupBy(Path.GetFileNameWithoutExtension).ToDictionary(g => g.Key!, g => g.First());
    var have = new HashSet<string>(StringComparer.Ordinal);
    foreach (var dir in new[] { dest, Path.Combine(repoRoot, "SpawnDev.SpawnJS", "SpawnJSObjects") })
        if (Directory.Exists(dir))
            foreach (var f in Directory.GetFiles(dir, "*.cs")) have.Add(Path.GetFileNameWithoutExtension(f)!);

    var ready = new List<string>();
    foreach (var (name, file) in available)
    {
        if (have.Contains(name)) continue;
        var text = File.ReadAllText(file);
        if (blockers.Any(b => b.Pattern.IsMatch(text))) continue;
        // any other wrapper type this file names is a dependency
        var deps = Regex.Matches(text, @"\b([A-Z]\w+)\b").Select(m => m.Groups[1].Value)
            .Where(t => t != name && available.ContainsKey(t) && !have.Contains(t)).Distinct().ToList();
        if (deps.Count == 0) ready.Add(name);
    }
    ready.Sort(StringComparer.Ordinal);
    if (printOnly)
    {
        Console.WriteLine($"ready to port now ({ready.Count}):");
        foreach (var chunk in ready.Chunk(6)) Console.WriteLine("  " + string.Join(", ", chunk));
    }
    return ready;
}

// reports types referenced by already-ported files that do not exist in SpawnJS yet
void Missing()
{
    if (!Directory.Exists(dest)) { Console.WriteLine("nothing ported yet"); return; }
    var have = new HashSet<string>(Directory.GetFiles(dest, "*.cs").Select(Path.GetFileNameWithoutExtension)!);
    foreach (var f in Directory.GetFiles(Path.Combine(repoRoot, "SpawnDev.SpawnJS", "SpawnJSObjects"), "*.cs"))
        have.Add(Path.GetFileNameWithoutExtension(f));
    var available = new HashSet<string>(Directory.GetFiles(source, "*.cs", SearchOption.AllDirectories).Select(Path.GetFileNameWithoutExtension)!);
    var referenced = new SortedSet<string>();
    foreach (var f in Directory.GetFiles(dest, "*.cs"))
        foreach (Match m in Regex.Matches(File.ReadAllText(f), @"\b([A-Z]\w+)\b"))
            if (available.Contains(m.Groups[1].Value) && !have.Contains(m.Groups[1].Value))
                referenced.Add(m.Groups[1].Value);
    Console.WriteLine(referenced.Count == 0 ? "no missing types" : "referenced but not ported:\n  " + string.Join("\n  ", referenced));
}

static string? FindUp(string start, Func<string, bool> match)
{
    var dir = start;
    while (!string.IsNullOrEmpty(dir))
    {
        if (match(dir)) return dir;
        dir = Path.GetDirectoryName(dir) ?? "";
    }
    return null;
}
