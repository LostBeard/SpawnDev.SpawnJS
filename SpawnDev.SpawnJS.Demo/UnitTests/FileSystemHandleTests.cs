using System;
using System.Threading.Tasks;
using SpawnDev.SpawnJS.JSObjects;

namespace SpawnDev.SpawnJS.Demo.UnitTests
{
    /// <summary>
    /// <see cref="FileSystemHandle.IsSameEntry(FileSystemHandle)"/> against the live OPFS implementation.
    /// <para>
    /// 🔴 <c>isSameEntry()</c> RETURNS A PROMISE. It was wrapped as a synchronous <c>bool</c> through
    /// <c>JSRef.Call&lt;...&gt;</c>, so it asked for the comparison result and got the pending Promise.
    /// SpawnDev.BlazorJS carried the identical defect and fixed it in 3.5.29 (<c>d898efe</c>), so this was
    /// a shared-lineage bug in both parallel wrappers, not a drift in one of them.
    /// </para>
    /// <para>
    /// ⭐ MEASURED, and it corrects what this file and commit <c>1bf1b2c</c> first claimed: under SpawnJS
    /// the broken wrapper does NOT quietly answer truthy. Its boolean marshaller refuses the value and
    /// throws - <c>Assert failed: Value is not a Boolean: [object Promise] (object)</c> - so ALL THREE
    /// cases below fail against it, not only the negative one. Red-checked 2026-09-14 by restoring the
    /// synchronous <c>Call</c> behind this same signature: 3 failed, 0 passed.
    /// </para>
    /// <para>
    /// ⚠️ Keep the negative case anyway. It is the one that stays load-bearing if the marshaller ever
    /// coerces instead of asserting (a truthy Promise satisfies "two handles to the same file match"
    /// perfectly), and it is the case that pins the ANSWER rather than merely the type.
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
