using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;

namespace TestsShared
{
    /// <summary>
    /// Tests for SpawnDev.BlazorJS JSObject wrappers ported into SpawnJS.<br/>
    /// These prove the port is real: each wrapper is resolved from the live browser through the
    /// marshaller graph and its properties and methods are exercised against the actual browser API,
    /// not just compiled.
    /// </summary>
    public class PortedJSObjectTests(SpawnJSRuntime JS)
    {
        /// <summary>
        /// Location is the archetype for the mechanically ported wrapper: property getters only,
        /// straight through to JSRef.Get.
        /// </summary>
        [SpawnJSTest]
        public async Task PortedLocationTest()
        {
            using var location = JS.Get<Location>("location");
            if (string.IsNullOrEmpty(location.Href)) throw new Exception("Location.Href was empty");
            if (string.IsNullOrEmpty(location.Origin)) throw new Exception("Location.Origin was empty");
            if (!location.Protocol.StartsWith("http")) throw new Exception($"Unexpected Location.Protocol '{location.Protocol}'");
            // the wrapper must agree with the same value read directly
            var origin = JS.Get<string>("location.origin");
            if (location.Origin != origin) throw new Exception($"Location.Origin '{location.Origin}' != '{origin}'");
            // ToString() is an override that routes through JSRef.Call
            if (location.ToString() != location.Href) throw new Exception("Location.ToString() != Href");
        }

        /// <summary>
        /// Storage exercises the method side of a ported wrapper - argument passing and a value return.
        /// </summary>
        [SpawnJSTest]
        public async Task PortedStorageTest()
        {
            using var storage = JS.Get<Storage>("localStorage");
            var key = "_spawnjs_ported_storage_test";
            var value = "round trip";
            try
            {
                storage.SetItem(key, value);
                var readBack = storage.GetItem(key);
                if (readBack != value) throw new Exception($"Expected '{value}', got '{readBack}'");
                if (!storage.ItemExists(key)) throw new Exception("ItemExists returned false for a key that was just set");
                if (storage.Length < 1) throw new Exception("Storage.Length was 0 after setting an item");
            }
            finally
            {
                storage.RemoveItem(key);
            }
            if (storage.GetItem(key) != null) throw new Exception("RemoveItem did not remove the key");
        }

        /// <summary>
        /// History confirms a ported wrapper resolved from a different global works too.
        /// </summary>
        [SpawnJSTest]
        public async Task PortedHistoryTest()
        {
            using var history = JS.Get<History>("history");
            if (history.Length < 1) throw new Exception($"History.Length was {history.Length}");
        }

        /// <summary>
        /// The ported base type carries the BlazorJS JSObject surface. JSRefIs/JSEquals are part of that
        /// contract and wrappers rely on them, so they have to work against a real object.
        /// </summary>
        [SpawnJSTest]
        public async Task PortedBaseTypeSurfaceTest()
        {
            using var location = JS.Get<Location>("location");
            if (!location.JSRefIs("Location")) throw new Exception($"JSRefIs('Location') was false, constructor is '{location.JSRef!.ConstructorName()}'");
            if (location.JSRefIs("Storage")) throw new Exception("JSRefIs('Storage') was true for a Location");
            // the same JS object read twice must compare equal in Javascript
            using var location2 = JS.Get<Location>("location");
            if (!location.JSEquals(location2, true)) throw new Exception("JSEquals(===) was false for two reads of location");
            using var storage = JS.Get<Storage>("localStorage");
            if (location.JSEquals(storage, true)) throw new Exception("JSEquals(===) was true for two different objects");
        }

        /// <summary>
        /// Ported wrappers that construct their own Javascript object go through JS.New rather than
        /// being resolved from an existing global, which is a different path through the marshallers.
        /// </summary>
        [SpawnJSTest]
        public async Task PortedConstructedWrapperTest()
        {
            using var regex = new RegExp(@"^a\d+$");
            if (!regex.JSRefIs("RegExp")) throw new Exception($"Constructed object is a '{regex.JSRef!.ConstructorName()}', expected RegExp");
            if (!regex.JSRef!.Call<bool>("test", "a123")) throw new Exception("RegExp did not match 'a123'");
            if (regex.JSRef!.Call<bool>("test", "b123")) throw new Exception("RegExp incorrectly matched 'b123'");
            // the flags overload takes a second constructor argument
            using var ci = new RegExp("^abc$", "i");
            if (!ci.JSRef!.Call<bool>("test", "ABC")) throw new Exception("Case insensitive flag was not applied");
        }

        /// <summary>
        /// JSRefCopy hands back an independent wrapper over the same Javascript object, so disposing the
        /// copy must leave the original usable. This is the ported-surface view of the handle refcount.
        /// </summary>
        [SpawnJSTest]
        public async Task PortedJSRefCopyTest()
        {
            using var location = JS.Get<Location>("location");
            var copy = location.JSRefCopy<Location>();
            copy.Dispose();
            if (string.IsNullOrEmpty(location.Origin)) throw new Exception("Original unusable after its JSRefCopy was disposed");
        }
    }
}
