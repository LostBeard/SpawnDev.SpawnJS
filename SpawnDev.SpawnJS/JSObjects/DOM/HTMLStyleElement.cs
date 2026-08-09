namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The HTMLStyleElement interface represents a style element. It inherits properties and methods from its parent, HTMLElement.<br/>
    /// Set the CSS text via the inherited TextContent property.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/HTMLStyleElement
    /// </summary>
    public class HTMLStyleElement : HTMLElement
    {
        #region Constructors
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public HTMLStyleElement(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Shortcut method for document.createElement('style')<br/>
        /// Non-standard implementation
        /// </summary>
        public HTMLStyleElement() : base(JS.DocumentCreateElement("style")) { }
        #endregion

        #region Properties
        /// <summary>
        /// A boolean value representing whether or not the stylesheet is disabled (true) or not (false).
        /// </summary>
        public bool Disabled { get => JSRef!.Get<bool>("disabled"); set => JSRef!.Set("disabled", value); }
        /// <summary>
        /// A string representing the intended destination medium for style information. Reflects the media attribute.
        /// </summary>
        public string Media { get => JSRef!.Get<string>("media"); set => JSRef!.Set("media", value); }
        /// <summary>
        /// A string representing the type of style being applied by this statement. Reflects the type attribute.
        /// </summary>
        public string Type { get => JSRef!.Get<string>("type"); set => JSRef!.Set("type", value); }
        #endregion
    }
}
