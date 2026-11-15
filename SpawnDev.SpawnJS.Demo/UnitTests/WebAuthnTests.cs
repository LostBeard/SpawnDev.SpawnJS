using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using SpawnDev.SpawnJS.JSObjects;

namespace SpawnDev.SpawnJS.Demo.UnitTests
{
    /// <summary>
    /// The WebAuthn JSON converters: PublicKeyCredential.ParseCreationOptionsFromJSON / ParseRequestOptionsFromJSON turn a
    /// relying party server's base64url JSON into the browser's options objects, CredentialsContainer.Create / Get take
    /// them, and PublicKeyCredential.ToJSONString gives the JSON to send back (spawndev.com's passkeys, Fido2NetLib).
    /// The create / get case needs an authenticator: SpawnJS.TestRunner adds a CDP virtual authenticator (ctap2,
    /// internal, resident key, user verification) to its page, so it runs the real ceremonies end to end.
    /// </summary>
    public static class WebAuthnTests
    {
        static string B64U(byte[] bytes) => Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        static byte[] FromB64U(string s)
        {
            s = s.Replace('-', '+').Replace('_', '/');
            return Convert.FromBase64String(s + new string('=', (4 - s.Length % 4) % 4));
        }
        static void Same(byte[] got, byte[] expected, string what)
        {
            if (!got.SequenceEqual(expected)) throw new Exception($"{what}: {got.Length} bytes [{B64U(got)}], expected {expected.Length} [{B64U(expected)}]");
        }
        // JsonObject, not JsonSerializer.Serialize(anonymous): the demo runs with reflection-based serialization disabled
        static string CreationJson(string rpId, byte[] challenge, byte[] userId) => new JsonObject
        {
            ["rp"] = new JsonObject { ["id"] = rpId, ["name"] = "SpawnJS test" },
            ["user"] = new JsonObject { ["id"] = B64U(userId), ["name"] = "test@spawndev.test", ["displayName"] = "SpawnJS Test" },
            ["challenge"] = B64U(challenge),
            ["pubKeyCredParams"] = new JsonArray(
                new JsonObject { ["type"] = "public-key", ["alg"] = -7 },
                new JsonObject { ["type"] = "public-key", ["alg"] = -257 }),
            ["timeout"] = 60000,
            ["authenticatorSelection"] = new JsonObject { ["residentKey"] = "preferred", ["userVerification"] = "preferred" },
            ["attestation"] = "none",
        }.ToJsonString();
        static string RequestJson(string rpId, byte[] challenge, string credentialId) => new JsonObject
        {
            ["challenge"] = B64U(challenge),
            ["rpId"] = rpId,
            ["allowCredentials"] = new JsonArray(new JsonObject { ["type"] = "public-key", ["id"] = credentialId }),
            ["userVerification"] = "preferred",
            ["timeout"] = 60000,
        }.ToJsonString();
        /// <summary>The clientDataJSON of a response JSON, decoded: (type, challenge, origin).</summary>
        static (string Type, string Challenge, string Origin) ClientData(JsonElement response)
        {
            var clientData = JsonDocument.Parse(FromB64U(response.GetProperty("clientDataJSON").GetString()!)).RootElement;
            return (clientData.GetProperty("type").GetString()!, clientData.GetProperty("challenge").GetString()!, clientData.GetProperty("origin").GetString()!);
        }

        /// <summary>Registers this group's cases with the harness's async test wrapper.</summary>
        /// <param name="testAsync">The suite's TestAsync(name, body) helper.</param>
        public static async Task Run(Func<string, Func<Task>, Task> testAsync)
        {
            var js = SpawnJSRuntime.Instance;
            var challenge = Enumerable.Range(1, 32).Select(i => (byte)i).ToArray();
            var userId = Encoding.UTF8.GetBytes("spawnjs-user-1");

            await testAsync("WebAuthn.IsJSONSupported", () =>
            {
                // the runner's Chrome is far past 129; false here means the TypeOf probe is wrong
                if (!PublicKeyCredential.IsJSONSupported) throw new Exception("PublicKeyCredential JSON converters reported missing");
                return Task.CompletedTask;
            });

            await testAsync("WebAuthn.ParseCreationOptionsFromJSON fills the ArrayBuffers", () =>
            {
                var rpId = js.Get<string>("location.hostname");
                using var options = PublicKeyCredential.ParseCreationOptionsFromJSON(CreationJson(rpId, challenge, userId));
                using var challengeBuffer = options.Challenge;
                Same(challengeBuffer.ReadBytes(), challenge, "challenge");
                using var userIdBuffer = options.JSRef!.Get<ArrayBuffer>("user.id");
                Same(userIdBuffer.ReadBytes(), userId, "user.id");
                var seenRp = options.JSRef!.Get<string>("rp.id");
                if (seenRp != rpId) throw new Exception($"rp.id is '{seenRp}', expected '{rpId}'");
                var alg = options.JSRef!.Get<int>("pubKeyCredParams.1.alg");
                if (alg != -257) throw new Exception($"pubKeyCredParams[1].alg is {alg}, expected -257");
                return Task.CompletedTask;
            });

            await testAsync("WebAuthn.ParseRequestOptionsFromJSON fills the ArrayBuffers", () =>
            {
                var rpId = js.Get<string>("location.hostname");
                var credentialId = Enumerable.Range(100, 20).Select(i => (byte)i).ToArray();
                using var options = PublicKeyCredential.ParseRequestOptionsFromJSON(RequestJson(rpId, challenge, B64U(credentialId)));
                using var challengeBuffer = options.Challenge;
                Same(challengeBuffer.ReadBytes(), challenge, "challenge");
                using var idBuffer = options.JSRef!.Get<ArrayBuffer>("allowCredentials.0.id");
                Same(idBuffer.ReadBytes(), credentialId, "allowCredentials[0].id");
                return Task.CompletedTask;
            });

            await testAsync("WebAuthn.Create and Get through JSON (virtual authenticator)", async () =>
            {
                var rpId = js.Get<string>("location.hostname");
                var origin = js.Get<string>("location.origin");
                using var credentials = CredentialsContainer.GetDefaultCredentialsContainer();

                // registration: server JSON in, RegistrationResponseJSON out
                using var creationOptions = PublicKeyCredential.ParseCreationOptionsFromJSON(CreationJson(rpId, challenge, userId));
                using var created = await credentials.Create(creationOptions);
                var registration = JsonDocument.Parse(created.ToJSONString()).RootElement;
                var id = registration.GetProperty("id").GetString()!;
                if (registration.GetProperty("rawId").GetString() != id) throw new Exception("rawId differs from id");
                if (registration.GetProperty("type").GetString() != "public-key") throw new Exception($"type is {registration.GetProperty("type")}");
                using var rawId = created.RawId;
                Same(rawId.ReadBytes(), FromB64U(id), "RawId vs the JSON id");
                var regResponse = registration.GetProperty("response");
                var (regType, regChallenge, regOrigin) = ClientData(regResponse);
                if (regType != "webauthn.create") throw new Exception($"clientData.type is {regType}");
                if (regChallenge != B64U(challenge)) throw new Exception($"clientData.challenge is {regChallenge}, expected {B64U(challenge)}");
                if (regOrigin != origin) throw new Exception($"clientData.origin is {regOrigin}, expected {origin}");
                if (FromB64U(regResponse.GetProperty("attestationObject").GetString()!).Length < 37) throw new Exception("attestationObject too short");

                // authentication: server JSON naming that credential in, AuthenticationResponseJSON out
                var challenge2 = Enumerable.Range(200, 32).Select(i => (byte)i).ToArray();
                using var requestOptions = PublicKeyCredential.ParseRequestOptionsFromJSON(RequestJson(rpId, challenge2, id));
                using var asserted = await credentials.Get(requestOptions);
                if (asserted == null) throw new Exception("get() resolved null");
                var authentication = JsonDocument.Parse(asserted.ToJSONString()).RootElement;
                if (authentication.GetProperty("id").GetString() != id) throw new Exception($"asserted id {authentication.GetProperty("id")}, expected {id}");
                var authResponse = authentication.GetProperty("response");
                var (authType, authChallenge, authOrigin) = ClientData(authResponse);
                if (authType != "webauthn.get") throw new Exception($"clientData.type is {authType}");
                if (authChallenge != B64U(challenge2)) throw new Exception($"clientData.challenge is {authChallenge}, expected {B64U(challenge2)}");
                if (authOrigin != origin) throw new Exception($"clientData.origin is {authOrigin}, expected {origin}");
                if (FromB64U(authResponse.GetProperty("authenticatorData").GetString()!).Length < 37) throw new Exception("authenticatorData too short");
                if (FromB64U(authResponse.GetProperty("signature").GetString()!).Length == 0) throw new Exception("empty signature");
            });
        }
    }
}
