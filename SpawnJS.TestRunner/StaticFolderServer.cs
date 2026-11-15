using System.Net;
using System.Net.Sockets;

/// <summary>
/// Serves a published wwwroot folder on http://localhost:&lt;free port&gt;/ (index.html for unknown paths). Used by
/// --trimmed, which runs the suite against a Release publish of the demo: trimmed, the way an app ships it. A dev
/// server run never trims, so it cannot show what the trimmer removed.
/// </summary>
sealed class StaticFolderServer : IDisposable
{
    static readonly Dictionary<string, string> Types = new(StringComparer.OrdinalIgnoreCase)
    {
        [".html"] = "text/html", [".js"] = "text/javascript", [".mjs"] = "text/javascript", [".css"] = "text/css",
        [".json"] = "application/json", [".wasm"] = "application/wasm", [".ico"] = "image/x-icon", [".png"] = "image/png",
        [".dat"] = "application/octet-stream", [".svg"] = "image/svg+xml",
    };

    readonly HttpListener listener = new();
    readonly string root;
    readonly CancellationTokenSource stop = new();

    public string Url { get; }

    public StaticFolderServer(string root)
    {
        this.root = root;
        var probe = new TcpListener(IPAddress.Loopback, 0);
        probe.Start();
        int port = ((IPEndPoint)probe.LocalEndpoint).Port;
        probe.Stop();
        Url = $"http://localhost:{port}/";
        listener.Prefixes.Add(Url);
        listener.Start();
        _ = Task.Run(ServeAsync);
    }

    async Task ServeAsync()
    {
        while (!stop.IsCancellationRequested)
        {
            HttpListenerContext context;
            try { context = await listener.GetContextAsync(); }
            catch { return; }
            _ = Task.Run(async () =>
            {
                try
                {
                    var path = Uri.UnescapeDataString(context.Request.Url!.AbsolutePath).TrimStart('/');
                    var file = Path.GetFullPath(Path.Combine(root, path.Replace('/', Path.DirectorySeparatorChar)));
                    if (path.Length == 0 || !file.StartsWith(root, StringComparison.OrdinalIgnoreCase) || !File.Exists(file))
                        file = Path.Combine(root, "index.html");
                    var bytes = await File.ReadAllBytesAsync(file);
                    context.Response.ContentType = Types.GetValueOrDefault(Path.GetExtension(file), "application/octet-stream");
                    context.Response.ContentLength64 = bytes.Length;
                    await context.Response.OutputStream.WriteAsync(bytes);
                }
                catch { context.Response.StatusCode = 500; }
                finally { try { context.Response.Close(); } catch { } }
            });
        }
    }

    public void Dispose()
    {
        stop.Cancel();
        try { listener.Stop(); listener.Close(); } catch { }
    }
}
