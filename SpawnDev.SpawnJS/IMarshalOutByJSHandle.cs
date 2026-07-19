namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// Implmeenting types will be marshalled out using their JSHandle.<br/>
    /// Useful for objects that can only transfer to JS
    /// </summary>
    public interface IMarshalOutByJSHandle
    {
        /// <summary>
        ///  The handle used to marshal the implementing type out
        /// </summary>
        SpawnJSHandle JSHandle { get; }
    }
}
