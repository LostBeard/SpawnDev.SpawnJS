// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The PushMessageData interface of the Push API provides methods which let you retrieve the push data sent by a server in various formats.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/PushMessageData
    /// </summary>
    public class PushMessageData : SpawnJSObject
    {
        /// <summary>
        /// Default deserialize constructor
        /// </summary>
        /// <param name="_ref"></param>
        public PushMessageData(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Extracts the data as an ArrayBuffer object.
        /// </summary>
        /// <returns></returns>
        public ArrayBuffer ArrayBuffer() => JSRef!.Call<ArrayBuffer>("arrayBuffer");
        /// <summary>
        /// Extracts the data as a Blob object.
        /// </summary>
        /// <returns></returns>
        public Blob Blob() => JSRef!.Call<Blob>("blob");
        /// <summary>
        /// Extracts the data as a JSON object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Json<T>() => JSRef!.Call<T>("json");
        /// <summary>
        /// Extracts the data as a plain text string.
        /// </summary>
        /// <returns></returns>
        public string Text() => JSRef!.Call<string>("text");
    }
}
