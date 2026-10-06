using System;
using System.Threading.Tasks;
using SpawnDev.SpawnJS.JSObjects;

namespace SpawnDev.SpawnJS.Demo.UnitTests
{
    /// <summary>
    /// DOM event wrappers read against real browser events. SpawnDev.SpawnJS.RazorRenderer builds Blazor's typed event args
    /// (PointerEventArgs, DragEventArgs...) from these, so a member typed wrong there reaches every Razor handler.
    /// </summary>
    public static class EventWrapperTests
    {
        /// <summary>Registers this group's cases with the harness's async test wrapper.</summary>
        /// <param name="testAsync">The suite's TestAsync(name, body) helper.</param>
        public static async Task Run(Func<string, Func<Task>, Task> testAsync)
        {
            // width / height are DOUBLES in CSS px (a touch contact is fractional); they were typed int.
            await testAsync("PointerEvent width / height keep fractional CSS px", () =>
            {
                var init = new SpawnJSObject(SpawnJSRuntime.Instance.New("Object"));
                init.JSRef!.Set("width", 23.5);
                init.JSRef!.Set("height", 7.25);
                init.JSRef!.Set("pointerId", 7);
                using var e = new PointerEvent(SpawnJSRuntime.Instance.New("PointerEvent", "pointerdown", init));
                init.Dispose();
                if (e.Width != 23.5) throw new Exception($"Width {e.Width}, expected 23.5");
                if (e.Height != 7.25) throw new Exception($"Height {e.Height}, expected 7.25");
                if (e.PointerId != 7) throw new Exception($"PointerId {e.PointerId}, expected 7");
                return Task.CompletedTask;
            });

            await testAsync("DataTransfer.Types lists the formats set, in order", () =>
            {
                using var dt = new DataTransfer();
                dt.SetData("text/plain", "hello");
                dt.SetData("text/uri-list", "https://example.com/");
                var types = dt.Types;
                if (types.Length != 2 || types[0] != "text/plain" || types[1] != "text/uri-list")
                    throw new Exception($"Types [{string.Join(", ", types)}], expected [text/plain, text/uri-list]");
                return Task.CompletedTask;
            });
        }
    }
}
