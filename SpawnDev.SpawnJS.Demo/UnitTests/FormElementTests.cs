using System;
using System.Threading.Tasks;
using SpawnDev.SpawnJS.JSObjects;

namespace SpawnDev.SpawnJS.Demo.UnitTests
{
    /// <summary>
    /// Form-control wrappers read against real DOM elements.
    /// </summary>
    public static class FormElementTests
    {
        /// <summary>Registers this group's cases with the harness's async test wrapper.</summary>
        /// <param name="testAsync">The suite's TestAsync(name, body) helper.</param>
        public static async Task Run(Func<string, Func<Task>, Task> testAsync)
        {
            var js = SpawnJSRuntime.Instance;

            await testAsync("HTMLTextAreaElement value round-trips exactly", () =>
            {
                using var ta = js.Call<string, HTMLTextAreaElement>("document.createElement", "textarea");
                // newlines, <tags>, quotes, a backslash and $-patterns: nothing may be interpreted or normalised
                const string text = "line1\nList<int> x = \"q\";\\ $1 $& \U0001F4A1";
                ta.Value = text;
                if (ta.Value != text) throw new Exception($"Value {ta.Value}");
                if (ta.TextLength != text.Length) throw new Exception($"TextLength {ta.TextLength}, expected {text.Length}");
                ta.DefaultValue = "dv";
                if (ta.DefaultValue != "dv") throw new Exception($"DefaultValue {ta.DefaultValue}");
                return Task.CompletedTask;
            });

            await testAsync("HTMLTextAreaElement attributes round-trip", () =>
            {
                using var ta = js.Call<string, HTMLTextAreaElement>("document.createElement", "textarea");
                ta.Name = "user-prompt";
                ta.Placeholder = "Ask anything";
                ta.Rows = 4;
                ta.Cols = 33;
                ta.Wrap = "off";
                ta.MaxLength = 50;
                ta.ReadOnly = true;
                ta.Disabled = true;
                ta.Required = true;
                if (ta.Name != "user-prompt" || ta.Placeholder != "Ask anything" || ta.Rows != 4 || ta.Cols != 33
                    || ta.Wrap != "off" || ta.MaxLength != 50 || !ta.ReadOnly || !ta.Disabled || !ta.Required)
                    throw new Exception($"read back {ta.Name}/{ta.Placeholder}/{ta.Rows}/{ta.Cols}/{ta.Wrap}/{ta.MaxLength}/{ta.ReadOnly}/{ta.Disabled}/{ta.Required}");
                return Task.CompletedTask;
            });

            await testAsync("HTMLTextAreaElement selection and SetRangeText", () =>
            {
                using var ta = js.Call<string, HTMLTextAreaElement>("document.createElement", "textarea");
                ta.Value = "hello world";
                ta.SetSelectionRange(6, 11);
                if (ta.SelectionStart != 6 || ta.SelectionEnd != 11) throw new Exception($"selection {ta.SelectionStart}-{ta.SelectionEnd}");
                ta.SetRangeText("there");
                if (ta.Value != "hello there") throw new Exception($"after SetRangeText(replacement): {ta.Value}");
                ta.SetRangeText("HEY", 0, 5, "end");
                if (ta.Value != "HEY there") throw new Exception($"after SetRangeText(range): {ta.Value}");
                if (ta.SelectionStart != 3 || ta.SelectionEnd != 3) throw new Exception($"selectMode end -> {ta.SelectionStart}-{ta.SelectionEnd}");
                return Task.CompletedTask;
            });

            await testAsync("HTMLTextAreaElement.Form is the owning form, null when detached", () =>
            {
                using var ta = js.Call<string, HTMLTextAreaElement>("document.createElement", "textarea");
                if (ta.Form != null) throw new Exception("detached textarea reported a form");
                using var form = js.Call<string, HTMLFormElement>("document.createElement", "form");
                form.AppendChild(ta);
                using var owner = ta.Form;
                if (owner == null) throw new Exception("textarea inside a form reported no form");
                return Task.CompletedTask;
            });
        }
    }
}
