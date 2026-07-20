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
    // NOT a blocker: SpawnJSObjectReference already carries CallAsync/GetAsync/CallVoidAsync, so
    // Task-returning wrapper members port unchanged. Verified by porting and building the
    // AsyncIterator/Iterator/IteratorResult closure. Kept out of the list deliberately.
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

// --allow <Blocker> drops a blocker from the list so its wrappers can be attempted.
// Use it to test whether a category is genuinely blocked rather than assumed to be.
// values consumed by a flag must never be mistaken for a type name
var flagValues = new HashSet<string>(StringComparer.Ordinal);
for (var i = 0; i < args.Length - 1; i++)
{
    if (args[i] != "--allow") continue;
    var drop = args[i + 1];
    flagValues.Add(drop);
    blockers = blockers.Where(b => !string.Equals(b.Name, drop, StringComparison.OrdinalIgnoreCase)).ToArray();
    Console.WriteLine($"(allowing '{drop}')");
}
bool IsTypeArg(string a) => !a.StartsWith("-") && !flagValues.Contains(a);

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

if (mode == "--impact")
{
    // for each blocker, how many wrappers would become mechanically portable if that blocker
    // alone were resolved. answers "which substrate do we build next" with a number.
    var files = Directory.GetFiles(source, "*.cs", SearchOption.AllDirectories);
    var only = new Dictionary<string, int>();
    var appears = new Dictionary<string, int>();
    var multi = 0;
    foreach (var f in files)
    {
        var text = File.ReadAllText(f);
        var hits = blockers.Where(b => b.Pattern.IsMatch(text)).Select(b => b.Name).ToArray();
        if (hits.Length == 0) continue;
        foreach (var h in hits) appears[h] = appears.GetValueOrDefault(h) + 1;
        if (hits.Length == 1) only[hits[0]] = only.GetValueOrDefault(hits[0]) + 1;
        else multi++;
    }
    Console.WriteLine($"{"blocker",-16} {"appears in",10} {"sole blocker",13}");
    foreach (var kv in appears.OrderByDescending(o => only.GetValueOrDefault(o.Key)))
        Console.WriteLine($"{kv.Key,-16} {kv.Value,10} {only.GetValueOrDefault(kv.Key),13}");
    Console.WriteLine($"\nwrappers with more than one blocker: {multi}");
    return 0;
}

if (mode == "--closure")
{
    // port a seed type together with every wrapper it transitively depends on.
    // clusters of mutually dependent wrappers can never become "ready" one file at a time,
    // so the closure is the unit that actually moves.
    var seeds = args.Skip(1).Where(IsTypeArg).ToArray();
    if (seeds.Length == 0) { Console.WriteLine("usage: --closure <TypeName> [TypeName...]"); return 1; }
    var (closure, blocked) = Closure(seeds);
    if (blocked.Count > 0)
    {
        Console.WriteLine($"closure of {string.Join(", ", seeds)} is {closure.Count + blocked.Count} types, {blocked.Count} of which need design work:");
        foreach (var kv in blocked.OrderBy(o => o.Key)) Console.WriteLine($"  {kv.Key,-34} {kv.Value}");
        Console.WriteLine("\nnothing ported - resolve or exclude those first");
        return 1;
    }
    foreach (var name in closure)
    {
        var src = Directory.GetFiles(source, name + ".cs", SearchOption.AllDirectories).First();
        File.WriteAllText(Path.Combine(dest, name + ".cs"), Transform(File.ReadAllText(src)));
    }
    Console.WriteLine($"ported closure of {string.Join(", ", seeds)}: {closure.Count} types");
    foreach (var chunk in closure.Order(StringComparer.Ordinal).Chunk(6)) Console.WriteLine("  " + string.Join(", ", chunk));
    return 0;
}

var names = args.Where(IsTypeArg).ToArray();
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

// transitive dependency closure of the seed types, split into portable and design-work-needed
(List<string> Portable, Dictionary<string, string> Blocked) Closure(string[] seeds)
{
    var available = Directory.GetFiles(source, "*.cs", SearchOption.AllDirectories)
        .GroupBy(Path.GetFileNameWithoutExtension).ToDictionary(g => g.Key!, g => g.First());
    var have = new HashSet<string>(StringComparer.Ordinal);
    foreach (var dir in new[] { dest, Path.Combine(repoRoot, "SpawnDev.SpawnJS", "SpawnJSObjects") })
        if (Directory.Exists(dir))
            foreach (var f in Directory.GetFiles(dir, "*.cs")) have.Add(Path.GetFileNameWithoutExtension(f)!);

    var portable = new List<string>();
    var blocked = new Dictionary<string, string>();
    var seen = new HashSet<string>(StringComparer.Ordinal);
    var queue = new Queue<string>(seeds);
    while (queue.Count > 0)
    {
        var name = queue.Dequeue();
        if (!seen.Add(name) || have.Contains(name)) continue;
        if (!available.TryGetValue(name, out var file)) continue;
        var text = File.ReadAllText(file);
        var hits = blockers.Where(b => b.Pattern.IsMatch(text)).Select(b => b.Name).ToArray();
        if (hits.Length > 0) { blocked[name] = string.Join(",", hits); continue; }
        portable.Add(name);
        foreach (Match m in Regex.Matches(text, @"\b([A-Z]\w+)\b"))
        {
            var dep = m.Groups[1].Value;
            if (dep != name && available.ContainsKey(dep) && !have.Contains(dep)) queue.Enqueue(dep);
        }
    }
    return (portable, blocked);
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
