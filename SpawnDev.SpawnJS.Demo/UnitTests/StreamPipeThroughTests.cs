using System;
using System.Threading.Tasks;
using SpawnDev.SpawnJS.JSObjects;

namespace SpawnDev.SpawnJS.Demo.UnitTests
{
    /// <summary>
    /// <see cref="ReadableStream.PipeThrough(CompressionStream)"/> and its siblings: pipeThrough() takes any
    /// {writable, readable} pair, and CompressionStream, DecompressionStream, TextDecoderStream and TextEncoderStream are
    /// pairs but not TransformStreams (in Javascript as here). PipeThrough only took a TransformStream, so a consumer could
    /// not gzip a Blob without viewing the CompressionStream's reference as a TransformStream (SpawnScene's .spawnscene v2
    /// export, 2026-10-04). Each case checks the bytes that come back, not just that a call returned.
    /// </summary>
    public static class StreamPipeThroughTests
    {
        /// <summary>A compressible but non-trivial payload: a repeating ramp with a slow drift.</summary>
        static byte[] Payload(int n)
        {
            var b = new byte[n];
            for (int i = 0; i < n; i++) b[i] = (byte)((i * 7 + i / 1000) & 0xFF);
            return b;
        }

        static async Task<byte[]> ReadAll(ReadableStream stream)
        {
            using var response = new Response(stream, (ResponseOptions?)null);
            using var buffer = await response.ArrayBuffer();
            using var view = new Uint8Array(buffer);
            return view.ReadBytes();
        }

        static void SameBytes(byte[] expected, byte[] actual, string what)
        {
            if (actual.Length != expected.Length)
                throw new Exception($"{what}: {actual.Length} bytes back, expected {expected.Length}");
            for (int i = 0; i < expected.Length; i++)
                if (actual[i] != expected[i])
                    throw new Exception($"{what}: byte {i} is {actual[i]}, expected {expected[i]}");
        }

        /// <summary>Registers this group's cases with the harness's async test wrapper.</summary>
        /// <param name="testAsync">The suite's TestAsync(name, body) helper.</param>
        public static async Task Run(Func<string, Func<Task>, Task> testAsync)
        {
            await testAsync("PipeThrough CompressionStream / DecompressionStream gzip round trip", async () =>
            {
                var payload = Payload(256 * 1024);
                byte[] zipped;
                using (var blob = new Blob(new[] { payload }, new BlobOptions { Type = "application/octet-stream" }))
                using (var src = blob.Stream())
                using (var gzip = new CompressionStream("gzip"))
                using (var piped = src.PipeThrough(gzip))
                    zipped = await ReadAll(piped);
                if (zipped.Length == 0 || zipped.Length >= payload.Length)
                    throw new Exception($"gzip gave {zipped.Length} bytes for a {payload.Length}-byte compressible payload");
                if (zipped[0] != 0x1F || zipped[1] != 0x8B)
                    throw new Exception($"not a gzip stream: starts {zipped[0]:X2} {zipped[1]:X2}");
                using var zBlob = new Blob(new[] { zipped }, new BlobOptions { Type = "application/octet-stream" });
                using var zSrc = zBlob.Stream();
                using var gunzip = new DecompressionStream("gzip");
                using var back = zSrc.PipeThrough(gunzip);
                SameBytes(payload, await ReadAll(back), "gunzip");
            });

            await testAsync("PipeThrough CompressionStream with options (deflate-raw) round trip", async () =>
            {
                var payload = Payload(64 * 1024);
                byte[] packed;
                using (var blob = new Blob(new[] { payload }, new BlobOptions { Type = "application/octet-stream" }))
                using (var src = blob.Stream())
                using (var deflate = new CompressionStream("deflate-raw"))
                using (var piped = src.PipeThrough(deflate, new PipeThroughOptions { PreventCancel = false }))
                    packed = await ReadAll(piped);
                if (packed.Length >= payload.Length) throw new Exception($"deflate-raw gave {packed.Length} bytes");
                using var pBlob = new Blob(new[] { packed }, new BlobOptions { Type = "application/octet-stream" });
                using var pSrc = pBlob.Stream();
                using var inflate = new DecompressionStream("deflate-raw");
                using var back = pSrc.PipeThrough(inflate, new PipeThroughOptions { PreventCancel = false });
                SameBytes(payload, await ReadAll(back), "inflate");
            });

            await testAsync("PipeThrough TextDecoderStream then TextEncoderStream keeps the text", async () =>
            {
                const string text = "SpawnJS pipeThrough - grüße, 日本語, emoji 🖖 and plain ASCII";
                var utf8 = System.Text.Encoding.UTF8.GetBytes(text);
                using var blob = new Blob(new[] { utf8 }, new BlobOptions { Type = "text/plain" });
                using var src = blob.Stream();
                using var decoder = new TextDecoderStream("utf-8");
                using var strings = src.PipeThrough(decoder);
                using var encoder = new TextEncoderStream();
                if (encoder.Encoding != "utf-8") throw new Exception($"TextEncoderStream.Encoding is '{encoder.Encoding}'");
                using var bytes = strings.PipeThrough(encoder);
                var back = await ReadAll(bytes);
                SameBytes(utf8, back, "text round trip");
            });
        }
    }
}
