namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The HTMLTemplateElement interface enables the use of HTML <c>&lt;template&gt;</c> elements. It inherits
    /// properties and methods from its parent, HTMLElement.<br/>
    /// Setting <see cref="Element.InnerHTML"/> (or <see cref="Element.SetInnerHTML(TrustedHTML)"/> on a Trusted
    /// Types page) parses the markup in the HTML "template" insertion mode, so head-only elements such as
    /// <c>&lt;style&gt;</c>, <c>&lt;script&gt;</c>, <c>&lt;link&gt;</c>, <c>&lt;meta&gt;</c> and
    /// <c>&lt;title&gt;</c> stay as children of <see cref="Content"/> in source order instead of being hoisted
    /// into a document <c>&lt;head&gt;</c> (which is what parsing the same string as a full <c>text/html</c>
    /// document does). This makes the template the correct primitive for turning an arbitrary HTML fragment into
    /// detached DOM nodes without losing or reordering any node.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/HTMLTemplateElement
    /// </summary>
    public class HTMLTemplateElement : HTMLElement
    {
        #region Constructors
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public HTMLTemplateElement(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Shortcut method for document.createElement('template')<br/>
        /// Non-standard implementation
        /// </summary>
        public HTMLTemplateElement() : base(JS.DocumentCreateElement("template")) { }
        #endregion

        #region Properties
        /// <summary>
        /// A read-only DocumentFragment that contains the DOM subtree representing the template's contents. The
        /// nodes live in the template's inert "template contents owner" document until they are appended (which
        /// adopts them) into a live document or shadow root.
        /// </summary>
        public DocumentFragment Content => JSRef!.Get<DocumentFragment>("content");
        #endregion
    }
}
