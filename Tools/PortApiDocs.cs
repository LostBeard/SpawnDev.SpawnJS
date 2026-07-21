#:property LangVersion=latest

// Ports the per-type API reference docs from SpawnDev.BlazorJS/Docs/api into SpawnDev.SpawnJS/Docs/api,
// applying the same rename transform the wrapper port uses, and SCOPED to types SpawnJS actually has
// (BlazorJS has more wrappers than SpawnJS has ported, and a 1.0.0 must not ship reference docs for
// types that do not exist).
//
//   dotnet run Tools/PortApiDocs.cs            port every doc whose type exists in SpawnJS
//   dotnet run Tools/PortApiDocs.cs -- --list  report which docs would port / skip, write nothing
//
// The transform (order matters - namespace first so "JSObjects" is never mangled):
//   SpawnDev.BlazorJS            -> SpawnDev.SpawnJS
//   IJSInProcessObjectReference  -> SpawnJSObjectReference
//   BlazorJSRuntime              -> SpawnJSRuntime
//   \bJSObject\b                 -> SpawnJSObject   (word boundary: NOT "JSObjects", NOT "SpawnJSObject")
//   BlazorJS                     -> SpawnJS         (stray mentions)
// The file JSObject.md becomes SpawnJSObject.md; links to JSObject.md are rewritten to SpawnJSObject.md.

using System.Text.RegularExpressions;

var repoRoot = FindUp(AppContext.BaseDirectory, d => Directory.GetFiles(d, "*.slnx").Length > 0)
    ?? Directory.GetCurrentDirectory();

var source = Path.GetFullPath(Path.Combine(repoRoot, "..", "..", "SpawnDev.BlazorJS", "SpawnDev.BlazorJS", "Docs", "api"));
var dest = Path.Combine(repoRoot, "Docs", "api");

if (!Directory.Exists(source)) { Console.Error.WriteLine($"BlazorJS Docs/api not found: {source}"); return 1; }

var listOnly = args.Contains("--list");

// Every type SpawnJS defines = every .cs basename in the project (JSObjects/ wrappers + core types),
// excluding obj/bin. A doc ports only if its (transformed) type name is in this set.
var sourceFiles = Directory.GetFiles(repoRoot, "*.cs", SearchOption.AllDirectories)
    .Where(f => !f.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}")
             && !f.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}"))
    .ToArray();

// Types SpawnJS defines. File basenames catch the one-type-per-file wrappers; a public-declaration scan
// also catches types declared INSIDE another file (e.g. CallbackGroup lives in Callback.cs).
var spawnJsTypes = new HashSet<string>(sourceFiles.Select(Path.GetFileNameWithoutExtension)!, StringComparer.Ordinal);
var declRx = new Regex(@"\bpublic\s+(?:sealed\s+|abstract\s+|static\s+|partial\s+|readonly\s+)*(?:class|struct|interface|enum|record)\s+(\w+)");
foreach (var f in sourceFiles)
    foreach (Match m in declRx.Matches(File.ReadAllText(f)))
        spawnJsTypes.Add(m.Groups[1].Value);

string TypeRename(string t) => t switch
{
    "JSObject" => "SpawnJSObject",
    "BlazorJSRuntime" => "SpawnJSRuntime",
    _ => t,
};

string Transform(string text)
{
    text = text.Replace("SpawnDev.BlazorJS", "SpawnDev.SpawnJS");
    text = text.Replace("IJSInProcessObjectReference", "SpawnJSObjectReference");
    text = text.Replace("BlazorJSRuntime", "SpawnJSRuntime");
    text = Regex.Replace(text, @"\bJSObject\b", "SpawnJSObject");
    text = Regex.Replace(text, @"\bJSObject\.md\b", "SpawnJSObject.md");   // link target (JSObject.md already became SpawnJSObject.md above? no - \bJSObject\b won't touch "JSObject.md"'s stem because '.' is a boundary, so the prior line already made it SpawnJSObject.md; this is a safety net)
    text = text.Replace("BlazorJS", "SpawnJS");
    return text;
}

int ported = 0, skipped = 0;
var portedNames = new List<string>();
Directory.CreateDirectory(dest);
foreach (var file in Directory.GetFiles(source, "*.md"))
{
    var baseName = Path.GetFileNameWithoutExtension(file);
    if (baseName == "_index") continue;   // regenerated clean below, never ported (its links would be stale)
    var targetName = TypeRename(baseName);
    if (!spawnJsTypes.Contains(targetName))
    {
        skipped++;
        if (listOnly) Console.WriteLine($"SKIP  {baseName}  (SpawnJS has no type '{targetName}')");
        continue;
    }
    ported++;
    portedNames.Add(targetName);
    if (listOnly) { Console.WriteLine($"PORT  {baseName}.md -> {targetName}.md"); continue; }
    File.WriteAllText(Path.Combine(dest, targetName + ".md"), Transform(File.ReadAllText(file)));
}

// Regenerate _index.md from exactly the ported set, so it never links to a doc that was skipped.
if (!listOnly)
{
    portedNames.Sort(StringComparer.Ordinal);
    var idx = new System.Text.StringBuilder();
    idx.AppendLine("# SpawnDev.SpawnJS API Reference");
    idx.AppendLine();
    idx.AppendLine($"Per-type reference for the {portedNames.Count} JS wrapper types in SpawnDev.SpawnJS.");
    idx.AppendLine();
    foreach (var n in portedNames) idx.AppendLine($"- [{n}]({n}.md)");
    File.WriteAllText(Path.Combine(dest, "_index.md"), idx.ToString());
}

Console.WriteLine($"{(listOnly ? "WOULD PORT" : "PORTED")}: {ported}   SKIPPED (no SpawnJS type): {skipped}   (source {Directory.GetFiles(source, "*.md").Length}, + regenerated _index)");
return 0;

static string? FindUp(string start, Func<string, bool> pred)
{
    var d = new DirectoryInfo(start);
    while (d != null) { if (pred(d.FullName)) return d.FullName; d = d.Parent; }
    return null;
}
