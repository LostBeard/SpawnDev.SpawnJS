namespace TestsShared
{
    /// <summary>
    /// Marks a method as a SpawnJS test.<br/>
    /// The method must return <see cref="Task"/> and take no parameters. Pass by returning normally,
    /// fail by throwing, skip by throwing <see cref="SkipTestException"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SpawnJSTestAttribute : Attribute
    {
        /// <summary>
        /// Milliseconds to wait before the test is reported as timed out. 0 uses the runner default.<br/>
        /// Note: .Net WASM is single threaded, so this catches a test that awaits forever, not one stuck
        /// in a tight synchronous loop.
        /// </summary>
        public int Timeout { get; set; }
    }

    /// <summary>
    /// Throw from a test to report it as skipped rather than failed
    /// </summary>
    public class SkipTestException : Exception
    {
        /// <summary>
        /// New instance
        /// </summary>
        public SkipTestException(string reason) : base(reason) { }
    }
}
