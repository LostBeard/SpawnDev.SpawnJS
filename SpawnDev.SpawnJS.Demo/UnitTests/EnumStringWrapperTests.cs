using System;
using System.Threading.Tasks;
using SpawnDev.SpawnJS.JSObjects;

namespace SpawnDev.SpawnJS.Demo.UnitTests
{
    /// <summary>
    /// Wrappers whose members are Javascript string enums must carry them as <see cref="EnumString{T}"/>.
    /// In 3.0.7 EncodedAudioChunkInit.Type, EncodedVideoChunkInit.Type and GainNodeOptions.ChannelCountMode /
    /// ChannelInterpretation were plain enums with [JsonConverter(typeof(JsonStringEnumConverter))] (or nothing),
    /// which SpawnJS writes as their NUMBER: the browser refused `new EncodedAudioChunk({ type: 0, ... })` and
    /// AubsCraft's voice chat had to build its own init class to send "key".
    /// </summary>
    public static class EnumStringWrapperTests
    {
        /// <summary>Registers this group's cases with the harness's async test wrapper.</summary>
        /// <param name="testAsync">The suite's TestAsync(name, body) helper.</param>
        public static async Task Run(Func<string, Func<Task>, Task> testAsync)
        {
            var js = SpawnJSRuntime.Instance;

            await testAsync("EnumString.EncodedAudioChunk type both ways", () =>
            {
                foreach (var type in new[] { EncodedAudioChunkType.Key, EncodedAudioChunkType.Delta })
                {
                    using var data = new ArrayBuffer(8);
                    // threw "TypeError: ... '0' is not a valid enum value of type EncodedAudioChunkType" when Type was a plain enum
                    using var chunk = new EncodedAudioChunk(new EncodedAudioChunkInit { Type = type, Timestamp = 40000, Duration = 20000, Data = data });
                    var expected = type == EncodedAudioChunkType.Key ? "key" : "delta";
                    var seen = chunk.JSRef!.Get<string>("type");
                    if (seen != expected) throw new Exception($"browser's chunk.type is '{seen}', expected '{expected}'");
                    if (chunk.Type != type) throw new Exception($"chunk.Type read back {chunk.Type}, expected {type}");
                    if (chunk.Timestamp != 40000 || chunk.ByteLength != 8) throw new Exception($"timestamp {chunk.Timestamp}, byteLength {chunk.ByteLength}");
                }
                return Task.CompletedTask;
            });

            await testAsync("EnumString.EncodedVideoChunk type both ways", () =>
            {
                foreach (var type in new[] { EncodedVideoChunkType.Key, EncodedVideoChunkType.Delta })
                {
                    using var data = new ArrayBuffer(16);
                    using var chunk = new EncodedVideoChunk(new EncodedVideoChunkInit { Type = type, Timestamp = 33333, Data = data });
                    var expected = type == EncodedVideoChunkType.Key ? "key" : "delta";
                    var seen = chunk.JSRef!.Get<string>("type");
                    if (seen != expected) throw new Exception($"browser's chunk.type is '{seen}', expected '{expected}'");
                    if (chunk.Type != type) throw new Exception($"chunk.Type read back {chunk.Type}, expected {type}");
                }
                return Task.CompletedTask;
            });

            await testAsync("EnumString.GainNodeOptions channel strings", () =>
            {
                const string key = "__enumStringGainOptions";
                try
                {
                    js.Set(key, new GainNodeOptions { Gain = 0.5f, ChannelCountMode = ChannelCountMode.ClampedMax, ChannelInterpretation = ChannelInterpretation.Discrete });
                    var mode = js.Get<string>($"{key}.channelCountMode");
                    var interpretation = js.Get<string>($"{key}.channelInterpretation");
                    if (mode != "clamped-max") throw new Exception($"channelCountMode is '{mode}', expected 'clamped-max'");
                    if (interpretation != "discrete") throw new Exception($"channelInterpretation is '{interpretation}', expected 'discrete'");
                    // unset stays absent (a null EnumString is not written as a string)
                    js.Set(key, new GainNodeOptions { Gain = 1f });
                    if (js.Get<string?>($"{key}.channelCountMode") != null) throw new Exception("an unset ChannelCountMode reached Javascript");
                }
                finally { js.Delete(key); }
                return Task.CompletedTask;
            });
        }
    }
}
