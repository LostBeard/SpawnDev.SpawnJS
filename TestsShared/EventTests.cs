using SpawnDev.SpawnJS;

namespace TestsShared
{
    /// <summary>
    /// ActionEvent / FuncEvent / CallbackRef substrate tests.<br/>
    /// These drive a real Javascript EventTarget: subscribing with += must attach a real listener,
    /// dispatching from Javascript must reach the .Net handler, and -= must detach it. The refcount
    /// behaviour matters as much as the firing - CallbackRef shares one JS function per .Net method
    /// and only disposes it when the last subscription is released.
    /// </summary>
    public class EventTests(SpawnJSRuntime JS)
    {
        /// <summary>
        /// Builds an ActionEvent over a real EventTarget's addEventListener/removeEventListener.
        /// This is exactly the shape ported wrappers use for their OnX properties.
        /// </summary>
        static ActionEvent EventFor(SpawnJSObjectReference target, string eventName)
            => new ActionEvent(eventName,
                (name, cb) => target.CallVoid("addEventListener", name, cb),
                (name, cb) => target.CallVoid("removeEventListener", name, cb));

        void Dispatch(SpawnJSObjectReference target, string eventName)
            => target.CallVoid("dispatchEvent", JS.New("Event", eventName));

        [SpawnJSTest]
        public async Task ActionEventFiresAndDetachesTest()
        {
            using var target = JS.New("EventTarget");
            var fired = 0;
            void Handler() => fired++;

            EventFor(target, "ping").On(Handler);
            Dispatch(target, "ping");
            if (fired != 1) throw new Exception($"Handler fired {fired} times after first dispatch, expected 1");

            Dispatch(target, "ping");
            if (fired != 2) throw new Exception($"Handler fired {fired} times after second dispatch, expected 2");

            EventFor(target, "ping").Off(Handler);
            Dispatch(target, "ping");
            if (fired != 2) throw new Exception($"Handler fired {fired} times after detach, expected it to stay at 2");
        }

        /// <summary>
        /// The += and -= operators are the surface every ported wrapper exposes, so they need their own
        /// coverage - they route through different overloads than On/Off.
        /// </summary>
        [SpawnJSTest]
        public async Task ActionEventOperatorsTest()
        {
            using var target = JS.New("EventTarget");
            var fired = 0;
            void Handler() => fired++;

            var evt = EventFor(target, "op");
            evt += Handler;
            Dispatch(target, "op");
            if (fired != 1) throw new Exception($"+= did not attach, fired {fired}");

            evt -= Handler;
            Dispatch(target, "op");
            if (fired != 1) throw new Exception($"-= did not detach, fired {fired}");
        }

        /// <summary>
        /// CallbackRef shares one JS function per .Net method across subscriptions and disposes it only
        /// when the last reference is released. Subscribing the same handler to two events and releasing
        /// one must leave the other working.
        /// </summary>
        [SpawnJSTest]
        public async Task CallbackRefSharesAndReleasesTest()
        {
            using var target = JS.New("EventTarget");
            var fired = 0;
            void Handler() => fired++;

            EventFor(target, "a").On(Handler);
            EventFor(target, "b").On(Handler);

            Dispatch(target, "a");
            Dispatch(target, "b");
            if (fired != 2) throw new Exception($"Expected 2 fires across both events, got {fired}");

            // release only the "a" subscription - "b" must still work
            EventFor(target, "a").Off(Handler);
            Dispatch(target, "a");
            if (fired != 2) throw new Exception($"Detached event still fired, count {fired}");
            Dispatch(target, "b");
            if (fired != 3) throw new Exception($"Remaining subscription stopped working, count {fired}");

            EventFor(target, "b").Off(Handler);
            Dispatch(target, "b");
            if (fired != 3) throw new Exception($"Final detach failed, count {fired}");
        }

        /// <summary>
        /// A typed handler receives the Javascript Event object, marshalled in through the graph.
        /// </summary>
        [SpawnJSTest]
        public async Task ActionEventTypedArgumentTest()
        {
            using var target = JS.New("EventTarget");
            string? seenType = null;
            void Handler(SpawnJSObjectReference e) => seenType = e.Get<string>("type");

            var evt = new ActionEvent<SpawnJSObjectReference>("typed",
                (name, cb) => target.CallVoid("addEventListener", name, cb),
                (name, cb) => target.CallVoid("removeEventListener", name, cb));
            evt.On(Handler);
            Dispatch(target, "typed");
            if (seenType != "typed") throw new Exception($"Handler received event type '{seenType}', expected 'typed'");
            evt.Off(Handler);
        }
    }
}
