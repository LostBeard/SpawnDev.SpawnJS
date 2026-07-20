// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Returned as elements in an array from the Promise returned by IDBFactory.databases() 
    /// </summary>
    public class IDBDatabaseInfo
    {
        /// <summary>
        /// The database name.
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// The database version.
        /// </summary>
        public long Version { get; set; }
    }
}
