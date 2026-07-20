using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;

namespace TestsShared
{
    /// <summary>
    /// Implicit delegate to Callback conversion, and the CallbackGet table behind it.<br/>
    /// This is what lets a wrapper declare a FuncCallback/ActionCallback property and have a caller assign
    /// a plain Func/Action to it - the shape ReadableStreamDefaultSource and ReadableByteStreamSource use.
    /// The conversion CREATES a Callback, so the questions that matter are whether the same delegate yields
    /// the same Callback rather than a new one each time, and whether the created Callback actually
    /// dispatches.
    /// </summary>
    public class CallbackConversionTests(SpawnJSRuntime JS)
    {
        int _calls;
        void Handler() => _calls++;
        string Describe(string value) => $"got:{value}";

        void InvokeFromJS(Callback callback)
        {
            const string key = "_spawnjs_conversion_test";
            JS.Set(key, callback);
            try { JS.CallVoid(key); }
            finally { JS.Delete(key); }
        }

        /// <summary>
        /// An Action assigned where an ActionCallback is expected must convert, and the resulting Callback
        /// must really reach the .Net method when Javascript calls it.
        /// </summary>
        [SpawnJSTest]
        public async Task ActionConvertsAndDispatchesTest()
        {
            Action handler = Handler;
            ActionCallback? callback = handler;
            if (callback == null) throw new Exception("An Action did not convert to an ActionCallback");
            try
            {
                _calls = 0;
                InvokeFromJS(callback);
                if (_calls != 1) throw new Exception($"The converted Callback fired {_calls} times, expected 1");
            }
            finally
            {
                handler.DisposeJS();
            }
        }

        /// <summary>
        /// The conversion is a get-or-create, not a create. Converting the same delegate twice must yield
        /// the same Callback - otherwise every assignment silently allocates another one and the caller has
        /// no way to dispose the ones it never saw.
        /// </summary>
        [SpawnJSTest]
        public async Task ConversionReturnsTheSameCallbackForOneDelegateTest()
        {
            Action handler = Handler;
            ActionCallback? first = handler;
            ActionCallback? second = handler;
            try
            {
                if (first == null || second == null) throw new Exception("Conversion produced null");
                if (!ReferenceEquals(first, second))
                    throw new Exception("Converting the same delegate twice produced two different Callbacks");
                if (first.Id != second.Id) throw new Exception($"Ids differ: {first.Id} vs {second.Id}");
            }
            finally
            {
                handler.DisposeJS();
            }
        }

        /// <summary>
        /// Different delegates must not collide in the lookup table.
        /// </summary>
        [SpawnJSTest]
        public async Task DifferentDelegatesConvertToDifferentCallbacksTest()
        {
            Action a = Handler;
            Action b = () => { };
            ActionCallback? first = a;
            ActionCallback? second = b;
            try
            {
                if (ReferenceEquals(first, second)) throw new Exception("Two different delegates shared one Callback");
                if (first!.Id == second!.Id) throw new Exception("Two different delegates shared one callback id");
            }
            finally
            {
                a.DisposeJS();
                b.DisposeJS();
            }
        }

        /// <summary>
        /// A Func must convert too, and its return value has to come back through the Javascript call.
        /// </summary>
        [SpawnJSTest]
        public async Task FuncConvertsAndReturnsAValueTest()
        {
            Func<string, string> handler = Describe;
            FuncCallback<string, string>? callback = handler;
            if (callback == null) throw new Exception("A Func did not convert to a FuncCallback");
            try
            {
                const string key = "_spawnjs_conversion_func_test";
                JS.Set(key, callback);
                try
                {
                    var result = JS.Call<string>(key, "abc");
                    if (result != "got:abc") throw new Exception($"The converted Func returned '{result}', expected 'got:abc'");
                }
                finally
                {
                    JS.Delete(key);
                }
            }
            finally
            {
                handler.DisposeJS();
            }
        }

        /// <summary>
        /// DisposeJS is how a caller releases what the conversion created, since the conversion itself
        /// returns nothing to hold. After it, converting again must produce a fresh live Callback rather
        /// than handing back the disposed one.
        /// </summary>
        [SpawnJSTest]
        public async Task DisposeJSReleasesAndAllowsReacquireTest()
        {
            Action handler = Handler;
            ActionCallback? first = handler;
            var firstId = first!.Id;
            handler.DisposeJS();

            if (!first.IsDisposed) throw new Exception("DisposeJS did not dispose the Callback");
            if (Callback.HasHandler(firstId)) throw new Exception("DisposeJS left the handler registered");

            ActionCallback? second = handler;
            try
            {
                if (second == null) throw new Exception("Conversion after DisposeJS produced null");
                if (second.IsDisposed) throw new Exception("Conversion after DisposeJS handed back the disposed Callback");
                _calls = 0;
                InvokeFromJS(second);
                if (_calls != 1) throw new Exception($"The re-acquired Callback fired {_calls} times, expected 1");
            }
            finally
            {
                handler.DisposeJS();
            }
        }
    }
}
