// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Used for Ink RequestPresenter call
    /// </summary>
    public class InkPresenterParam
    {
        /// <summary>
        /// An Element inside which rendering of ink strokes is confined (the element's border box, to be precise). If param is not included, or presentationArea is set to null, ink rendering is confined to the containing viewport by default.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Element? PresentationArea { get; set; }
    }
}
