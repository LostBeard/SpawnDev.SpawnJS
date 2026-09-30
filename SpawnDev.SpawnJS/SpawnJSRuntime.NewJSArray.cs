namespace SpawnDev.SpawnJS
{
    public partial class SpawnJSRuntime
    {
        #region NewArray

        internal SpawnJSObjectReference NewJSArray() 
            => new SpawnJSObjectReference(_spawnJSObjectNewArray());
        #endregion
    }
}
