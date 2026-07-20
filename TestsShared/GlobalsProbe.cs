using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;

namespace TestsShared
{
    /// <summary>
    /// Reports, for every ported wrapper type, whether a Javascript global of that name exists on this
    /// host.<br/>
    /// Run it on the browser and on the console host and the difference is the browser-specific set -
    /// measured, rather than classified by hand or taken from compatibility tables. That is the input to
    /// splitting SpawnDev.SpawnJS.Browser out of the core.<br/>
    /// One implementation, called from both hosts, so the two runs cannot drift.
    /// </summary>
    public static class GlobalsProbe
    {
        /// <summary>
        /// Every public, non-generic wrapper type name in the JSObjects namespace
        /// </summary>
        public static IEnumerable<string> WrapperTypeNames()
            => typeof(SpawnJSObject).Assembly.GetTypes()
                .Where(t => t.IsPublic && !t.IsAbstract && t.Namespace != null && t.Namespace.StartsWith("SpawnDev.SpawnJS.JSObjects"))
                .Select(t => t.Name)
                .Where(n => !n.Contains('`'))
                .Distinct()
                .OrderBy(n => n, StringComparer.Ordinal);

        /// <summary>
        /// Writes one "GLOBAL: Name|yes|no" line per wrapper type, then a PROBE-DONE line.
        /// </summary>
        public static void Emit(SpawnJSRuntime JS)
        {
            var present = 0;
            var total = 0;
            foreach (var name in WrapperTypeNames())
            {
                var exists = JS.Has(name);
                if (exists) present++;
                total++;
                Console.WriteLine($"GLOBAL: {name}|{(exists ? "yes" : "no")}");
            }
            Console.WriteLine($"PROBE-SUMMARY: {present} of {total} wrapper types exist as globals on this host");
            Console.WriteLine("PROBE-DONE");
        }
    }
}
