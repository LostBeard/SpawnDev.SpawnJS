using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The HTMLTextAreaElement interface provides properties and methods for manipulating the layout and presentation of &lt;textarea&gt; elements.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/HTMLTextAreaElement
    /// </summary>
    public class HTMLTextAreaElement : HTMLElement
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public HTMLTextAreaElement(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The raw value contained in the control.
        /// </summary>
        public string Value { get => JSRef!.Get<string>("value"); set => JSRef!.Set("value", value); }
        /// <summary>
        /// The control's default value, which behaves like the Node.textContent property.
        /// </summary>
        public string DefaultValue { get => JSRef!.Get<string>("defaultValue"); set => JSRef!.Set("defaultValue", value); }
        /// <summary>
        /// The codepoint length of the control's value. Same as reading Value.Length in UTF-16 code units.
        /// </summary>
        public int TextLength => JSRef!.Get<int>("textLength");
        /// <summary>
        /// The element's name attribute.
        /// </summary>
        public string Name { get => JSRef!.Get<string>("name"); set => JSRef!.Set("name", value); }
        /// <summary>
        /// The element's placeholder attribute, a hint to the user of what can be entered in the control.
        /// </summary>
        public string Placeholder { get => JSRef!.Get<string>("placeholder"); set => JSRef!.Set("placeholder", value); }
        /// <summary>
        /// The element's rows attribute, the number of visible text lines for the control.
        /// </summary>
        public int Rows { get => JSRef!.Get<int>("rows"); set => JSRef!.Set("rows", value); }
        /// <summary>
        /// The element's cols attribute, the visible width of the text area.
        /// </summary>
        public int Cols { get => JSRef!.Get<int>("cols"); set => JSRef!.Set("cols", value); }
        /// <summary>
        /// The element's wrap attribute: "hard", "soft" or "off".
        /// </summary>
        public string Wrap { get => JSRef!.Get<string>("wrap"); set => JSRef!.Set("wrap", value); }
        /// <summary>
        /// The element's disabled attribute, indicating that the control is not available for interaction.
        /// </summary>
        public bool Disabled { get => JSRef!.Get<bool>("disabled"); set => JSRef!.Set("disabled", value); }
        /// <summary>
        /// The element's readOnly attribute, indicating that the user cannot modify the value of the control.
        /// </summary>
        public bool ReadOnly { get => JSRef!.Get<bool>("readOnly"); set => JSRef!.Set("readOnly", value); }
        /// <summary>
        /// The element's required attribute, indicating that the user must specify a value before submitting the form.
        /// </summary>
        public bool Required { get => JSRef!.Get<bool>("required"); set => JSRef!.Set("required", value); }
        /// <summary>
        /// The element's minlength attribute.
        /// </summary>
        public int MinLength { get => JSRef!.Get<int>("minLength"); set => JSRef!.Set("minLength", value); }
        /// <summary>
        /// The element's maxlength attribute (-1 when unset).
        /// </summary>
        public int MaxLength { get => JSRef!.Get<int>("maxLength"); set => JSRef!.Set("maxLength", value); }
        /// <summary>
        /// The element's autocomplete attribute.
        /// </summary>
        public string Autocomplete { get => JSRef!.Get<string>("autocomplete"); set => JSRef!.Set("autocomplete", value); }
        /// <summary>
        /// The &lt;form&gt; element this textarea is associated with, or null.
        /// </summary>
        public HTMLFormElement? Form => JSRef!.Get<HTMLFormElement?>("form");
        /// <summary>
        /// The index of the beginning of selected text. When nothing is selected, the position of the text cursor.
        /// </summary>
        public int SelectionStart { get => JSRef!.Get<int>("selectionStart"); set => JSRef!.Set("selectionStart", value); }
        /// <summary>
        /// The index of the end of selected text. When nothing is selected, the position of the text cursor.
        /// </summary>
        public int SelectionEnd { get => JSRef!.Get<int>("selectionEnd"); set => JSRef!.Set("selectionEnd", value); }
        /// <summary>
        /// The direction in which selection occurred: "forward", "backward" or "none".
        /// </summary>
        public string SelectionDirection { get => JSRef!.Get<string>("selectionDirection"); set => JSRef!.Set("selectionDirection", value); }
        /// <summary>
        /// True if the element is a candidate for constraint validation.
        /// </summary>
        public bool WillValidate => JSRef!.Get<bool>("willValidate");
        /// <summary>
        /// The localized message describing the validation constraints the control does not satisfy, if any.
        /// </summary>
        public string ValidationMessage => JSRef!.Get<string>("validationMessage");
        /// <summary>
        /// Returns true if the element meets all its constraint validations, false otherwise.
        /// </summary>
        public bool CheckValidity() => JSRef!.Call<bool>("checkValidity");
        /// <summary>
        /// Like CheckValidity, but also reports a failure to the user.
        /// </summary>
        public bool ReportValidity() => JSRef!.Call<bool>("reportValidity");
        /// <summary>
        /// Sets a custom validity message. Pass an empty string to mark the element valid.
        /// </summary>
        public void SetCustomValidity(string message) => JSRef!.CallVoid("setCustomValidity", message);
        /// <summary>
        /// Selects all of the text in the element.
        /// </summary>
        public void Select() => JSRef!.CallVoid("select");
        /// <summary>
        /// Selects a range of text in the element (but does not focus it).
        /// </summary>
        public void SetSelectionRange(int start, int end, string? direction = null)
        {
            if (direction == null)
                JSRef!.CallVoid("setSelectionRange", start, end);
            else
                JSRef!.CallVoid("setSelectionRange", start, end, direction);
        }
        /// <summary>
        /// Replaces the current selection with <paramref name="replacement"/>.
        /// </summary>
        public void SetRangeText(string replacement) => JSRef!.CallVoid("setRangeText", replacement);
        /// <summary>
        /// Replaces the text from <paramref name="start"/> to <paramref name="end"/> with <paramref name="replacement"/>.
        /// <paramref name="selectMode"/> is "select", "start", "end" or "preserve" (the default).
        /// </summary>
        public void SetRangeText(string replacement, int start, int end, string selectMode = "preserve") => JSRef!.CallVoid("setRangeText", replacement, start, end, selectMode);
    }
}
