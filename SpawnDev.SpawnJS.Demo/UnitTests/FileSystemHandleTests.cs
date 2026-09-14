using System;
using System.Threading.Tasks;
using SpawnDev.SpawnJS.JSObjects;

namespace SpawnDev.SpawnJS.Demo.UnitTests
{
    /// <summary>
    /// <see cref="FileSystemHandle.IsSameEntry(FileSystemHandle)"/> against the live OPFS implementation.
    /// <para>
    /// 🔴 <c>isSameEntry()</c> RETURNS A PROMISE. It was wrapped as a synchronous <c>bool</c> through
    /// <c>JSRef.Call&lt;...&gt;</c>, so it handed back the pending Promise marshalled as a bool instead of
    /// the comparison result - the same answer for matching and non-matching handles, and no exception to
    /// reveal it. SpawnDev.BlazorJS carried the identical defect and fixed it in 3.5.29 (<c>d898efe</c>),
    /// so this was a shared-lineage bug in both parallel wrappers, not a drift in one of them.
    /// </para>
    /// <para>
    /// ⚠️ THE NEGATIVE TEST IS THE ONE THAT CATCHES IT. "Two handles to the same file match" is the
    /// obvious case to write, and a wrapper that always answers truthy passes it. Only "two handles to
    /// DIFFERENT files must answer false" can fail against a Promise.
    /// </para>
    /// <para>
    /// Multiple handles may represent one entry, so <c>isSameEntry</c> - not reference equality, not
    /// <see cref="FileSystemHandle.Name"/> - is the way to ask whether two handles are the same file.
    /// </para>
    /// </summary>
    public static class FileSystemHandleTests
    {
        static SpawnJSRuntime JS => SpawnJSRuntime.Instance!;

        static async Task<FileSystemDirectoryHandle> RootAsync()
        {
            // Fails loudly rather than skipping: this suite runs in a real browser, where OPFS exists.
            if (!JS.Has("navigator.storage.getDirectory"))
                throw new Exception("navigator.storage.getDirectory is unavailable - OPFS is required");
            using var navigator = JS.Get<Navigator>("navigator");
            using var storage = navigator.Storage;
            return await storage.GetDirectory();
        }

        static string NewName(string tag) => $"issameentry-{tag}-{Guid.NewGuid():N}.bin";

        /// <summary>Registers this group's cases with the harness's async test wrapper.</summary>
        /// <param name="testAsync">The suite's TestAsync(name, body) helper.</param>
        public static async Task Run(Func<string, Func<Task>, Task> testAsync)
        {
            await testAsync("IsSameEntry matches two handles to one file", async () =>
            {
                var name = NewName("same");
                using var root = await RootAsync();
                try
                {
                    using var a = await root.GetFileHandle(name, true);
                    using var b = await root.GetFileHandle(name, false);
                    if (!await a.IsSameEntry(b))
                        throw new Exception("IsSameEntry returned false for two handles to the same file");
                }
                finally { try { await root.RemoveEntry(name); } catch { } }
            });

            await testAsync("IsSameEntry rejects handles to different files", async () =>
            {
                var nameA = NewName("a");
                var nameB = NewName("b");
                using var root = await RootAsync();
                try
                {
                    using var a = await root.GetFileHandle(nameA, true);
                    using var b = await root.GetFileHandle(nameB, true);
                    if (await a.IsSameEntry(b))
                        throw new Exception(
                            "IsSameEntry returned true for handles to two DIFFERENT files - the result is " +
                            "not the comparison, it is a truthy Promise (isSameEntry must be awaited)");
                }
                finally
                {
                    try { await root.RemoveEntry(nameA); } catch { }
                    try { await root.RemoveEntry(nameB); } catch { }
                }
            });

            await testAsync("IsSameEntry compares the OPFS root to a file and to itself", async () =>
            {
                var name = NewName("root");
                using var root = await RootAsync();
                try
                {
                    using var file = await root.GetFileHandle(name, true);
                    if (await root.IsSameEntry(file))
                        throw new Exception("IsSameEntry returned true comparing the OPFS root to a file in it");

                    using var root2 = await RootAsync();
                    if (!await root.IsSameEntry(root2))
                        throw new Exception("IsSameEntry returned false comparing the OPFS root to itself");
                }
                finally { try { await root.RemoveEntry(name); } catch { } }
            });
        }
    }
}
