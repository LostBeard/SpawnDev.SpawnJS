// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The CSSRule interface represents a single CSS rule. There are several types of rules which inherit properties from CSSRule.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/CSSRule
    /// </summary>
    public class CSSRule : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public CSSRule(SpawnJSObjectReference _ref) : base(_ref) { }

        /// <summary>
        /// Returns the textual representation of the rule.
        /// </summary>
        public string CSSText
        {
            get => JSRef!.Get<string>("cssText");
            set => JSRef!.Set("cssText", value);
        }

        /// <summary>
        /// Returns the containing rule, if any.
        /// </summary>
        public CSSRule? ParentRule
        {
            get => JSRef!.Get<CSSRule?>("parentRule");
        }

        /// <summary>
        /// Returns the stylesheet object in which the rule is defined.
        /// </summary>
        public CSSStyleSheet? ParentStyleSheet
        {
            get => JSRef!.Get<CSSStyleSheet?>("parentStyleSheet");
        }

        /// <summary>
        /// Returns the type of the rule, as an unsigned short.
        /// </summary>
        public ushort Type
        {
            get => JSRef!.Get<ushort>("type");
        }

    }
}
