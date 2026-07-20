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

// SpawnJS's own hand-written wrapper types live in JSObjects/ alongside the ported ones (Error,
// Promise, Function, Array, Window, JSException...). They are what the runtime itself is built
// against, so a ported BlazorJS wrapper of the same name must never overwrite one.
//
// A file is native when it does NOT carry the ported marker. Identifying natives by "everything
// already sitting in the destination folder" is what this used to do, and it made the tool a one shot:
// after the first run every ported file looked native, so a re-run silently skipped all of them while
// still reporting them as ported. Any later fix to Transform then appeared to do nothing.
const string PortedMarker = "// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit";

bool IsPorted(string text)
    => text.Contains(PortedMarker, StringComparison.Ordinal)
    // files ported before the marker existed are still recognisable by the injected using header,
    // which Transform emits verbatim and no hand-written wrapper starts with
    || Regex.IsMatch(text, @"\A﻿?using SpawnDev\.SpawnJS;\r?\nusing SpawnDev\.SpawnJS\.JSObjects;");

// Scanned across the WHOLE project, not just JSObjects/. A ported wrapper lands in
// SpawnDev.SpawnJS.JSObjects while the hand-written types live in SpawnDev.SpawnJS, and every ported
// file imports both namespaces - so a ported wrapper sharing a name with a core type (Reflect, Union,
// Callback...) makes that name ambiguous in every file that uses it, which the namespaces alone do not
// prevent.
var projectDir = Path.GetFullPath(Path.Combine(dest, ".."));
var NativeTypes = new HashSet<string>(
    Directory.GetFiles(projectDir, "*.cs", SearchOption.AllDirectories)
        .Where(f => !f.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}")
                 && !f.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}"))
        .Where(f => !IsPorted(File.ReadAllText(f)))
        .Select(Path.GetFileNameWithoutExtension)!,
    StringComparer.Ordinal);

// Wrappers held back on a named, open design decision rather than a missing type. Listed explicitly so
// the reason travels with the skip and the entry is trivially removed once the decision is made.
var deferred = new Dictionary<string, string>(StringComparer.Ordinal)
{
    // empty. ReadableStreamDefaultSource and ReadableByteStreamSource were here while the implicit
    // delegate-to-Callback conversions and their CallbackGet backing were still commented out - that was
    // waiting on code which has since been ported, not on a design decision.
};

// patterns that mean the wrapper needs design work, not a mechanical copy
var blockers = new (string Name, Regex Pattern)[]
{
    // NOT a blocker: ElementReference members are stripped by Transform and reinstated as extension
    // methods in SpawnDev.SpawnJS.Blazor. Keeping the Blazor type out of core is the point, not a gap.
    // ActionEvent/FuncEvent/CallbackEvent/CallbackRef are ported, so event-bearing wrappers are no
    // longer blocked on the substrate. JSEventCallback has no SpawnJS equivalent yet.
    ("JSEventCallback", new Regex(@"\bJSEventCallback\b")),
    // NOT a blocker: Union and UnionMarshaller are both in SpawnJS. The union type ported over unchanged
    // (it only ever needed the Json attributes stripped) and arm selection now reads the live Javascript
    // value's prototype chain instead of a Json token, so Union-bearing wrappers port mechanically.
    // NOT a blocker: SpawnJSObjectReference already carries CallAsync/GetAsync/CallVoidAsync, so
    // Task-returning wrapper members port unchanged. Verified by porting and building the
    // AsyncIterator/Iterator/IteratorResult closure. Kept out of the list deliberately.
    // Callback/ActionCallback/FuncCallback/CallbackGroup all exist in SpawnJS, so not a blocker.
    // NOT a blocker: DateTimeMarshaller and EpochDateTimeMarshaller are registered, and EpochDateTime
    // plus its DateTime extensions are ported.
    // NOT a blocker: EnumString and EnumStringMarshaller are both in SpawnJS. An EnumString is just the
    // Javascript string naming an enum member, so it marshals as a string - it never needed the Json
    // converter it carried in BlazorJS.
    ("Undefinable",new Regex(@"\bUndefinable\b")),
    // NOT a blocker: every use is either the `static BlazorJSRuntime JS => BlazorJSRuntime.JS;` ambient
    // accessor or a `BlazorJSRuntime.JS?.IsUndefined(...)` capability probe. Both map straight onto
    // SpawnJSRuntime / SpawnJSRuntime.Instance and are rewritten by Transform.
    // NOT a blocker: GPUIntegerCoordinate, GPUFlagsConstant, GPUSize32 and friends are not types at all,
    // they are `global using X = System.UInt32;` aliases in JSObjects/WebGPU/GPUTypeDefinitions.cs. That
    // file matched the pattern naming its own aliases, so it blocked itself and every file using them.
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

// Every wrapper type name the source tree defines. Used to tell a reference-typed property from a
// value-typed one when deciding what may safely be made nullable.
var WrapperTypeNames = new HashSet<string>(
    Directory.GetFiles(source, "*.cs", SearchOption.AllDirectories).Select(Path.GetFileNameWithoutExtension)!,
    StringComparer.Ordinal);

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
            Emit(src);
        }
        Console.WriteLine($"pass {pass}: ported {batch.Count} ({string.Join(", ", batch.Take(12))}{(batch.Count > 12 ? ", ..." : "")})");
        total += batch.Count;
    }
    Console.WriteLine($"\nported {total} wrappers");
    return 0;
}

if (mode == "--port-all")
{
    // port every blocker-free wrapper in one pass. The remaining wrappers form a cyclic dependency
    // graph (Element <-> Node <-> Document), so a "port only what has zero unported deps" rule can
    // never move them. Port the lot and let the compiler name whatever is genuinely missing.
    var count = 0;
    foreach (var f in Directory.GetFiles(source, "*.cs", SearchOption.AllDirectories))
    {
        var text = File.ReadAllText(f);
        if (blockers.Any(b => b.Pattern.IsMatch(text))) continue;
        if (Emit(f)) count++;
    }
    Console.WriteLine($"ported {count} wrappers");
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
        Emit(src);
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
    Emit(file);
    Console.WriteLine($"  PORT  {name}");
    ported++;
}
Console.WriteLine($"\nported {ported} of {names.Length}");
return 0;

// mirrors the BlazorJS JSObjects/ subfolder layout (WebGPU/, WebRTC/, DOM/, ...) rather than
// flattening ~1000 wrappers into one directory
string DestPathFor(string sourceFile)
{
    var relative = Path.GetRelativePath(source, sourceFile);
    var target = Path.Combine(dest, relative);
    Directory.CreateDirectory(Path.GetDirectoryName(target)!);
    return target;
}

bool Emit(string sourceFile)
{
    var name = Path.GetFileNameWithoutExtension(sourceFile);
    if (NativeTypes.Contains(name))
    {
        Console.WriteLine($"  SKIP  {name} - SpawnJS has its own implementation");
        return false;
    }
    if (deferred.TryGetValue(name, out var reason))
    {
        Console.WriteLine($"  DEFER {name} - {reason}");
        return false;
    }
    File.WriteAllText(DestPathFor(sourceFile), Transform(File.ReadAllText(sourceFile)));
    return true;
}

string Transform(string text)
{
    text = Regex.Replace(text, @"^using Microsoft\.JSInterop;\s*\r?\n", "", RegexOptions.Multiline);
    // ElementReference is Microsoft.AspNetCore.Components - a Blazor type. SpawnJS core has no Blazor
    // dependency by design, which is what lets it run under Avalonia, a console host or a raw worker.
    // The members that take one are pure convenience (a constructor and two explicit conversions per
    // element wrapper, all single line and each preceded by its doc comment), so they are stripped here
    // and reinstated as extension methods in SpawnDev.SpawnJS.Blazor, where the dependency is legal.
    text = Regex.Replace(text, @"^using Microsoft\.AspNetCore\.Components;\s*\r?\n", "", RegexOptions.Multiline);
    text = Regex.Replace(text, @"(?:^[ \t]*///[^\r\n]*\r?\n)*^[ \t]*public[^\r\n]*\bElementReference\b[^\r\n]*\r?\n", "", RegexOptions.Multiline);
    text = Regex.Replace(text, @"^using SpawnDev\.BlazorJS[^\r\n]*;\s*\r?\n", "", RegexOptions.Multiline);
    text = Regex.Replace(text, @"namespace SpawnDev\.BlazorJS(\.\w+)*", "namespace SpawnDev.SpawnJS.JSObjects");
    // fully qualified references survive the namespace rewrite above, which only touches the declaration.
    // The union typedef file (TypeDefinitions.cs) is written entirely in fully qualified names.
    text = Regex.Replace(text, @"\bSpawnDev\.BlazorJS\.JSObjects\b", "SpawnDev.SpawnJS.JSObjects");
    text = Regex.Replace(text, @"\bSpawnDev\.BlazorJS\b", "SpawnDev.SpawnJS");
    text = Regex.Replace(text, @"\bIJSInProcessObjectReference\b", "SpawnJSObjectReference");
    // Enum wrappers carry [JsonConverter(typeof(EnumStringConverterFactory))] so System.Text.Json knows
    // how to write them. SpawnJS marshals an EnumString through EnumStringMarshaller instead, and there
    // is no Json converter to point at, so the attribute goes. The [JsonPropertyName] attributes on the
    // enum MEMBERS stay - EnumString reads those reflectively to learn each member's Javascript string.
    text = Regex.Replace(text, @"^[ \t]*\[JsonConverter\(typeof\((?:JsonConverters\.)?EnumStringConverterFactory\)\)\][ \t]*\r?\n", "", RegexOptions.Multiline);
    // BlazorJSRuntime is SpawnJSRuntime here, and its ambient `JS` singleton is SpawnJSRuntime.Instance.
    // The accessor shape is rewritten first so it throws like the rest of the codebase rather than
    // silently handing back a null runtime.
    text = Regex.Replace(text,
        @"(?<prefix>(?:private |protected |internal |public )?static )BlazorJSRuntime JS => BlazorJSRuntime\.JS;",
        "${prefix}SpawnJSRuntime JS => SpawnJSRuntime.Instance ?? throw new InvalidOperationException(\"SpawnJSRuntime has not been created.\");");
    // remaining uses are null conditional capability probes: BlazorJSRuntime.JS?.IsUndefined("...")
    text = Regex.Replace(text, @"\bBlazorJSRuntime\.JS\b", "SpawnJSRuntime.Instance");
    text = Regex.Replace(text, @"\bBlazorJSRuntime\b", "SpawnJSRuntime");
    // JSObject is BlazorJS's wrapper base type; SpawnJSObject is ours. \b will not match the JSObjects
    // namespace segment, so this is safe to apply to the whole file rather than only the base type
    // position - wrappers also take and return JSObject as a plain type (ProxyHandler alone does it 68
    // times) and those uses have to move too.
    text = Regex.Replace(text, @"\bJSObject\b", "SpawnJSObject");
    // The pre-built-argument-array escape hatch is spelled Void-before-Apply in SpawnJS
    // (CallVoidApply) and Void-after-Apply in BlazorJS (CallApplyVoid). Same method, same arguments.
    // \b will not match CallApplyVoidAsync when rewriting CallApplyVoid, so the order here is safe.
    text = Regex.Replace(text, @"\bCallApplyVoidAsync\b", "CallVoidApplyAsync");
    text = Regex.Replace(text, @"\bCallApplyVoid\b", "CallVoidApply");
    // Microsoft's IJSInProcessObjectReference.Invoke/InvokeVoid are the same operation as
    // SpawnJS's Call/CallVoid. Normalize rather than carry a legacy alias on the reference type.
    text = Regex.Replace(text, @"\.Invoke<", ".Call<");
    text = Regex.Replace(text, @"\.InvokeVoid\(", ".CallVoid(");
    // SpawnJS's own wrapper types (Error, Promise, Function, Array, Window...) live in a different
    // namespace than the ported ones, so ported files need both.
    // Toolbox holds the support types wrappers reach for by simple name (HeapView and friends), the same
    // way they do in SpawnDev.BlazorJS.
    var usings = PortedMarker + "\r\nusing SpawnDev.SpawnJS;\r\nusing SpawnDev.SpawnJS.JSObjects;\r\nusing SpawnDev.SpawnJS.Toolbox;\r\n";
    text = MakeDataShapePropertiesNullable(text, WrapperTypeNames);
    text = DocumentConstants(text);
    // a `global using` must precede every non-global using, so insert after any leading global usings.
    // A global using alias can span lines (the WebIDL union typedefs are written one arm per line), so
    // the match has to run to the terminating semicolon rather than to the end of the line - matching
    // only single line ones lands the injected usings in the middle of the block and breaks the file.
    var lastGlobal = Regex.Matches(text, @"^global using\b[\s\S]*?;[ \t]*\r?\n", RegexOptions.Multiline).LastOrDefault();
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
    foreach (var dir in new[] { dest, Path.Combine(repoRoot, "SpawnDev.SpawnJS") })
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
    foreach (var dir in new[] { dest, Path.Combine(repoRoot, "SpawnDev.SpawnJS") })
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
    foreach (var f in Directory.GetFiles(Path.Combine(repoRoot, "SpawnDev.SpawnJS"), "*.cs"))
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

// Documents undocumented public constants. WebGLConstants.cs alone carries 632 of them, and an
// undocumented public member is a CS1591 each.
//
// The doc states facts rather than restating the name: which spec group the constant belongs to (taken
// from the #region it sits in) and its value. That is what is actually useful at a call site - "which
// family is this and what number does it send to WebGL" - and it is information the name alone does not
// carry. A generated "Gets the COLOR_BUFFER_BIT constant" would be noise and is not worth writing.
static string DocumentConstants(string text)
{
    if (!text.Contains("public const ", StringComparison.Ordinal)) return text;
    var lines = text.Replace("\r\n", "\n").Split('\n');
    var output = new List<string>(lines.Length);
    var region = "";
    var constant = new Regex(@"^(\s*)public const (\w+) (\w+)\s*=\s*([^;]+);");

    foreach (var line in lines)
    {
        var regionMatch = Regex.Match(line, @"^\s*#region\s+(.+?)\s*$");
        if (regionMatch.Success) region = regionMatch.Groups[1].Value;
        if (Regex.IsMatch(line, @"^\s*#endregion")) region = "";

        var m = constant.Match(line);
        if (m.Success)
        {
            // skip if it already carries a doc comment
            var previous = output.Count > 0 ? output[^1].TrimStart() : "";
            if (!previous.StartsWith("///", StringComparison.Ordinal))
            {
                var indent = m.Groups[1].Value;
                var value = m.Groups[4].Value.Trim();
                var where = region.Length > 0 ? $"{region}. " : "";
                output.Add($"{indent}/// <summary>");
                output.Add($"{indent}/// {where}Value {value}.");
                output.Add($"{indent}/// </summary>");
            }
        }
        output.Add(line);
    }
    return string.Join("\r\n", output);
}

// Makes the properties of a data shape nullable.
//
// An options bag or descriptor is a plain object whose members are all optional - Javascript simply omits
// the ones you do not set - so a non-nullable reference-typed property is a lie, and the compiler says so
// with CS8618 ("must contain a non-null value when exiting constructor"). BlazorJS is inconsistent about
// it even within one file: CredentialCreateFederated declares IconURL as string? and Id as string.
//
// The discriminator is exact rather than heuristic. A data shape uses an AUTO-property; a wrapper property
// is backed by JSRef accessors:
//     public string Id { get; set; }                                        <- data shape
//     public string Href { get => JSRef!.Get<string>("href"); set => ... }  <- wrapper
// so "{ get; set; }" identifies the first and never the second.
//
// Only types known to be reference types are touched. Adding ? to a value type would change its meaning,
// and CS8618 never fires for one anyway.
static string MakeDataShapePropertiesNullable(string text, HashSet<string> wrapperTypeNames)
{
    // modifiers have to be captured separately or they end up inside the type ("override string")
    var property = new Regex(@"^(?<indent>\s*)public (?<mods>(?:override |virtual |new |sealed )*)(?<type>[\w<>,\[\]\. ]+?) (?<name>\w+) \{ get; set; \}\s*$");
    var lines = text.Replace("\r\n", "\n").Split('\n');
    var changed = false;

    for (var i = 0; i < lines.Length; i++)
    {
        var m = property.Match(lines[i]);
        if (!m.Success) continue;
        var type = m.Groups["type"].Value.Trim();
        if (type.EndsWith("?", StringComparison.Ordinal)) continue;
        // An override does not get to redeclare nullability - the member it overrides defines the
        // contract, and changing it here earns CS8765 plus a cascade of CS8604/CS8602 at every caller
        // that now sees a nullable value. Same for virtual, whose overrides live in other files.
        var mods = m.Groups["mods"].Value;
        if (mods.Contains("override", StringComparison.Ordinal) || mods.Contains("virtual", StringComparison.Ordinal)) continue;

        var isReferenceType =
            type == "string"
            || type == "object"
            || type.EndsWith("[]", StringComparison.Ordinal)
            || type.StartsWith("List<", StringComparison.Ordinal)
            || type.StartsWith("Dictionary<", StringComparison.Ordinal)
            || type.StartsWith("IEnumerable<", StringComparison.Ordinal)
            // Deliberately NOT extended to Union, EnumString or the Callback family. Making those
            // properties nullable is right for the declaration but wrong overall: their consumers assume
            // non-null, so it trades 33 CS8618 for 45 CS8604 plus 11 CS8602. Measured, then reverted.
            || wrapperTypeNames.Contains(type.Split('<')[0]);
        if (!isReferenceType) continue;

        lines[i] = $"{m.Groups["indent"].Value}public {m.Groups["mods"].Value}{type}? {m.Groups["name"].Value} {{ get; set; }}";
        changed = true;
    }
    return changed ? string.Join("\r\n", lines) : text;
}
