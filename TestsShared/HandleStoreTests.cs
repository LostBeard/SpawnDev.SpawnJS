using SpawnDev.SpawnJS;

namespace TestsShared
{
    /// <summary>
    /// The shared handle store. Every owning SpawnJSHandle parks its value in one Javascript object under
    /// its own key, rather than each constructing a private single element Array.<br/>
    /// Two things have to hold or the design is worse than what it replaced: a disposed handle must give
    /// its slot back, and two live handles must never share a key.
    /// </summary>
    public class HandleStoreTests(SpawnJSRuntime JS)
    {
        /// <summary>
        /// A disposed handle must release its slot. A leaked slot is worse than the allocation it saved -
        /// it keeps the Javascript value reachable for the life of the process.
        /// </summary>
        [SpawnJSTest]
        public async Task DisposedHandleReleasesItsSlotTest()
        {
            var before = SpawnJSHandle.LiveSlotCount;
            using (var target = JS.New("Object"))
            using (var handle = new SpawnJSHandle(target.JSObject))
            {
                if (SpawnJSHandle.LiveSlotCount <= before)
                    throw new Exception($"Creating an owning handle did not take a slot (before {before}, now {SpawnJSHandle.LiveSlotCount})");
            }
            if (SpawnJSHandle.LiveSlotCount != before)
                throw new Exception($"Slot count is {SpawnJSHandle.LiveSlotCount} after dispose, expected {before} - a slot leaked");
        }

        /// <summary>
        /// Many handles taken and released must leave the store exactly as it was. This is the shape that
        /// would expose an off-by-one or a double release.
        /// </summary>
        [SpawnJSTest]
        public async Task ManyHandlesLeaveNoSlotsBehindTest()
        {
            var before = SpawnJSHandle.LiveSlotCount;
            for (var i = 0; i < 50; i++)
            {
                using var target = JS.New("Object");
                using var handle = new SpawnJSHandle(target.JSObject);
            }
            if (SpawnJSHandle.LiveSlotCount != before)
                throw new Exception($"Slot count drifted by {SpawnJSHandle.LiveSlotCount - before} over 50 handles");
        }

        /// <summary>
        /// Two live handles must hold DIFFERENT values. Sharing one store means a key collision would not
        /// throw - it would silently hand one handle the other's Javascript object, which is the failure
        /// this design has to rule out.
        /// </summary>
        [SpawnJSTest]
        public async Task ConcurrentHandlesDoNotCollideTest()
        {
            using var first = JS.New("Object");
            using var second = JS.New("Object");
            first.Set("tag", "first");
            second.Set("tag", "second");

            using var handleA = new SpawnJSHandle(first.JSObject);
            using var handleB = new SpawnJSHandle(second.JSObject);

            if (Equals(handleA.JSKey, handleB.JSKey))
                throw new Exception($"Two live handles share the store key {handleA.JSKey}");

            var tagA = handleA.GetPropertyAsString("tag");
            var tagB = handleB.GetPropertyAsString("tag");
            if (tagA != "first" || tagB != "second")
                throw new Exception($"Handles resolved to the wrong values: '{tagA}' and '{tagB}'");
        }

        /// <summary>
        /// A slot key is never reused, so a handle taken after another was released cannot inherit a stale
        /// value from it.
        /// </summary>
        [SpawnJSTest]
        public async Task ReleasedSlotKeyIsNotReusedTest()
        {
            object firstKey;
            using (var target = JS.New("Object"))
            using (var handle = new SpawnJSHandle(target.JSObject))
            {
                firstKey = handle.JSKey;
            }

            using var next = JS.New("Object");
            using var reacquired = new SpawnJSHandle(next.JSObject);
            if (Equals(reacquired.JSKey, firstKey))
                throw new Exception($"Slot key {firstKey} was handed out again after being released");
        }
    }
}
