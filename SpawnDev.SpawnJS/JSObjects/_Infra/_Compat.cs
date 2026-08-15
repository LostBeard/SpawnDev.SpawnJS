using System.Diagnostics.CodeAnalysis;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// Compatibility helpers that old SpawnJS exposed as instance methods on SpawnJSRuntime / the reference.
    /// The new Core types live in a different assembly and are not partial-extendable from here, so these are
    /// provided as extension methods (call syntax is identical). Implemented against the new typed Call API.
    /// </summary>
    public static class ImportedCompatExtensions
    {
        /// <summary>document.createElement</summary>
        public static SpawnJSObjectReference DocumentCreateElement(this SpawnJSRuntime js, string elementType)
            => js.Call<string, SpawnJSObjectReference>("document.createElement", elementType)!;

        /// <summary>document.createElement, returning the typed wrapper</summary>
        public static T DocumentCreateElement<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this SpawnJSRuntime js, string elementType) where T : Element
            => js.Call<string, T>("document.createElement", elementType);

        /// <summary>document.body.appendChild</summary>
        public static void DocumentBodyAppendChild(this SpawnJSRuntime js, Element element)
            => js.CallVoid<Element>("document.body.appendChild", element);

        // ---- reference helpers old SpawnJS exposed as instance methods (reimplemented on the new interop) ----


    }
}
