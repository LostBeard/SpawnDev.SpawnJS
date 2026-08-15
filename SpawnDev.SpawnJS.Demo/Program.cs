using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System;
using System.Linq;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Blob = SpawnDev.SpawnJS.JSObjects.Blob;
using Document = SpawnDev.SpawnJS.JSObjects.Document;

var JS = SpawnJSRuntime.Instance;
JS.Verbose = true;
{
    var t = new ArrayBuffer(500);
    using var blob = new Blob(new ArrayBuffer[] { t });

    var keyUsages = new string[] { "sign", "verify" };
    JS.Set("_art", keyUsages);
    var r = JS.Get<string[]>("_art");
    var nmt22 = true;
}
return;

var typeInfo = JS.TypeInfo();
if (typeInfo.TypeOf != "object" || typeInfo.ConstructorName != "Window") throw new Exception("Incorrect type info");
var constructorNames = JS.ConstructorNames();
if (!constructorNames.SequenceEqual(["Window", "EventTarget", "Object"])) throw new Exception("Incorrect constructor names.");
if (JS.Keys().Count == 0) throw new Exception("Expected more than 0 keys");
{
    var t = JSArrayBufferView.DataView;
    JS.Set("_art", t);
    var r = JS.Get<JSArrayBufferView>("_art");
    var nmt22 = true;
}

// ===== Marshaller round-trip tests (Tuple / ValueTuple / ValueTuple? / Union / Action / Func) =====
{
    int pass = 0, fail = 0;
    void Test(string name, Func<bool> body)
    {
        try
        {
            if (body()) { pass++; Console.WriteLine($"  PASS  {name}"); }
            else { fail++; Console.WriteLine($"  FAIL  {name}"); }
        }
        catch (Exception ex)
        {
            fail++;
            Console.WriteLine($"  FAIL  {name} -> {ex.GetType().Name}: {ex.Message}");
            Console.WriteLine(ex.ToString());
        }
    }

    Test("ValueTuple (string,string)", () =>
    {
        (string, string) v = ("Hello", "world!");
        JS.Set("_valueTuple", v);
        var r = JS.Get<(string, string)>("_valueTuple");
        return r.Item1 == "Hello" && r.Item2 == "world!";
    });
    Test("Tuple<string,int>", () =>
    {
        var v = new Tuple<string, int>("Hello", 42);
        JS.Set("_tuple", v);
        var r = JS.Get<Tuple<string, int>>("_tuple");
        return r != null && r.Item1 == "Hello" && r.Item2 == 42;
    });
    Test("ValueTuple? non-null", () =>
    {
        (int, string)? v = (7, "seven");
        JS.Set("_vtn", v);
        var r = JS.Get<(int, string)?>("_vtn");
        return r.HasValue && r.Value.Item1 == 7 && r.Value.Item2 == "seven";
    });
    Test("ValueTuple? null", () =>
    {
        (int, string)? v = null;
        JS.Set("_vtNull", v);
        var r = JS.Get<(int, string)?>("_vtNull");
        return !r.HasValue;
    });
    Test("Union<string,int> int arm", () =>
    {
        Union<string, int> u = 42;
        JS.Set("_ui", u);
        var r = JS.Get<Union<string, int>>("_ui");
        return r != null && r.Is<int>() && (int)r == 42;
    });
    Test("Union<string,int> string arm", () =>
    {
        Union<string, int> u = "hello";
        JS.Set("_us", u);
        var r = JS.Get<Union<string, int>>("_us");
        return r != null && r.Is<string>() && (string)r == "hello";
    });
    Test("Action -> JS function invoke", () =>
    {
        bool called = false;
        Action act = () => { called = true; };
        JS.Set("_act", act);
        JS.CallVoid("_act");
        return called;
    });
    Test("Action<int> -> JS function arg", () =>
    {
        int got = 0;
        Action<int> act = x => { got = x; };
        JS.Set("_act1", act);
        JS.CallVoid("_act1", 99);
        return got == 99;
    });
    Test("Func<int,int> -> JS function return", () =>
    {
        Func<int, int> dbl = x => x * 2;
        JS.Set("_dbl", dbl);
        var r = JS.Call<int, int>("_dbl", 21);
        return r == 42;
    });

    Console.WriteLine($"MARSHALLER TESTS: {pass} passed, {fail} failed");
}



// ===== Enum / Enum? round-trip tests (numeric enum <-> JS number, nullable maps null <-> JS null) =====
{
    int pass = 0, fail = 0;
    void Test(string name, Func<bool> body)
    {
        try
        {
            if (body()) { pass++; Console.WriteLine($"  PASS  {name}"); }
            else { fail++; Console.WriteLine($"  FAIL  {name}"); }
        }
        catch (Exception ex)
        {
            fail++;
            Console.WriteLine($"  FAIL  {name} -> {ex.GetType().Name}: {ex.Message}");
            Console.WriteLine(ex.ToString());
        }
    }

    Test("Enum crosses as number", () =>
    {
        const DemoFlags v = DemoFlags.CopySrc | DemoFlags.Storage;
        JS.Set("_enum", v);
        // JS must hold the NUMBER, not an opaque box - this is what a web API sees.
        var asNumber = JS.Get<double>("_enum");
        var back = JS.Get<DemoFlags>("_enum");
        return asNumber == (double)(uint)v && back == v;
    });
    Test("Enum? non-null crosses as number", () =>
    {
        DemoFlags? v = DemoFlags.CopyDst | DemoFlags.Storage;
        JS.Set("_enumN", v);
        var asNumber = JS.Get<double>("_enumN");
        var back = JS.Get<DemoFlags?>("_enumN");
        return asNumber == (double)(uint)v.Value && back.HasValue && back.Value == v.Value;
    });
    Test("Enum? null crosses as JS null", () =>
    {
        DemoFlags? v = null;
        JS.Set("_enumNull", v);
        // A null nullable-enum must land as JS null, NOT the number default(Enum)==0. Reading the raw
        // slot as double? tells them apart: JS null -> null, a mistakenly-written 0 -> 0.0.
        var raw = JS.Get<double?>("_enumNull");
        var back = JS.Get<DemoFlags?>("_enumNull");
        return raw == null && !back.HasValue;
    });

    Console.WriteLine($"ENUM TESTS: {pass} passed, {fail} failed");
}



// ===== TrustedTypes CreatePolicy repro (Callback-in-options marshalling) =====
{
    int pass = 0, fail = 0;
    void Test(string name, Action body)
    {
        try { body(); pass++; Console.WriteLine($"  PASS  {name}"); }
        catch (Exception ex) { fail++; Console.WriteLine($"  FAIL  {name} -> {ex.GetType().Name}: {ex.Message}"); }
    }

    // A string-keyed dictionary must cross as a JS record (plain object), NOT be property-walked as a POCO -
    // otherwise its Comparer/Keys/Values members marshal instead of its contents and it throws Arg_DlgtTargMeth.
    Test("Dictionary<string,int> round-trips as a record", () =>
    {
        var d = new System.Collections.Generic.Dictionary<string, int> { ["a"] = 1, ["b"] = 22, ["c"] = 333 };
        JS.Set("_recInt", d);
        if (JS.Get<int>("_recInt.b") != 22) throw new Exception("value not stored under its key on the JS object");
        var back = JS.Get<System.Collections.Generic.Dictionary<string, int>>("_recInt");
        if (back == null || back.Count != 3 || back["a"] != 1 || back["b"] != 22 || back["c"] != 333)
            throw new Exception($"round-trip mismatch: {(back == null ? "null" : string.Join(",", back.Select(kv => $"{kv.Key}={kv.Value}")))}");
    });

    // A record whose values are Callbacks is what trustedTypes.createPolicy takes - each value marshalled per-value.
    using var factory = JS.Get<SpawnDev.SpawnJS.JSObjects.TrustedTypePolicyFactory?>("trustedTypes");
    if (factory != null)
    {
        Test("trustedTypes.createPolicy with a Callback record", () =>
        {
            using var policy = factory.CreatePolicy("spawndev-demo-tt-" + Guid.NewGuid().ToString("N"),
                new SpawnDev.SpawnJS.JSObjects.TrustedTypePolicyOptions { CreateHTML = Callback.Create<string, string>(s => s) });
            using var html = policy.CreateHTML("<b>hi</b>");
        });
    }

    Console.WriteLine($"DICTIONARY/TT TESTS: {pass} passed, {fail} failed");
}




// ===== PocoMarshaller round-trip test (property-walk clone, honours Json attributes; no JSON serialization) =====
{
    var person = new TestPerson
    {
        FirstName = "Ada",      // [JsonPropertyName("given_name")]
        LastName = "Lovelace",
        Age = 36,
        Secret = "hidden",      // [JsonIgnore] - must NOT be written to JS
        Nickname = null,        // [JsonIgnore(WhenWritingNull)] + null - must NOT be written
        City = null,            // no ignore - written as null
    };
    JS.Set("__pocoTest", person);                       // NetToJS: property-walk into a new JS object
    using var raw = JS.Get("__pocoTest")!;              // the raw JS object, to inspect member names
    bool hasGiven = raw.Has("given_name");           // JsonPropertyName rename
    bool hasFirstName = raw.Has("firstName");        // original name should be gone
    bool hasSecret = raw.Has("secret");              // JsonIgnore -> absent
    bool hasNickname = raw.Has("nickname");          // WhenWritingNull + null -> absent
    bool hasCity = raw.Has("city");                  // null but written -> present
    int rawAge = raw.Get<int>("age");                   // camelCase default naming

    var back = JS.Get<TestPerson>("__pocoTest")!;       // JSToNet: property-walk back into a new POCO

    Console.WriteLine($"POCO names: given_name={hasGiven} firstName={hasFirstName} secret={hasSecret} nickname={hasNickname} city={hasCity} age={rawAge}");
    Console.WriteLine($"POCO back: FirstName={back.FirstName} LastName={back.LastName} Age={back.Age} Secret={back.Secret ?? "null"} Nickname={back.Nickname ?? "null"} City={back.City ?? "null"}");
    bool ok = hasGiven && !hasFirstName && !hasSecret && !hasNickname && hasCity && rawAge == 36
              && back.FirstName == "Ada" && back.LastName == "Lovelace" && back.Age == 36
              && back.Secret == null && back.Nickname == null && back.City == null;
    Console.WriteLine($"POCO TEST {(ok ? "PASS" : "FAIL")}");
}

using var document = JS.Get<Document>("document")!;

using var performance = JS.Get("performance");

double lastTime = performance!.Call<double>("now");
double frameCount = 0;
double fps = 0;
double scale = 1.0;

JS.Set("_growHeap", Callback.Create(() =>
{
    JS.GrowHeap();
}));


var runIt = false;
double width = 500;
double height = 500;
double maxIter = 80; // Balanced for real-time frame rates

var fpsDisplay = document.CreateElement<HTMLDivElement>("div");
var canvas = document.CreateElement<HTMLCanvasElement>("canvas");
canvas.Width = (int)width;
canvas.Height = (int)height;
using var ctx = canvas.Get2DContext();
ctx.FillStyle = "#000000";
ctx.FillRect(0, 0, width, height);

Callback? cb = null;
// image data
var data = new byte[(int)(width * height * 4)];
// direct heap view of image data
using var heapView = HeapView.Create(data);
// heap view as Uint8ClampedArray
using var uint8ArrayClamped = heapView.As<Uint8ClampedArray>();
ImageData? imgData = null;
JS.OnHeapGrow += (_, _) =>
{
    Console.WriteLine($"Rebuilding imgData");
    imgData = new ImageData(uint8ArrayClamped, width, height);
};
imgData = new ImageData(uint8ArrayClamped, width, height);

using var stopButton = document.CreateElement<HTMLButtonElement>("button");
stopButton.TextContent = "Stop";
stopButton.OnClick += () =>
{
    runIt = false;
    JS.CallVoid("stopJSAnimation");
};

// Union marshaller now handles the Append call below.
document.Body!.Append(stopButton);

using var startJSButton = document.CreateElement<HTMLButtonElement>("button");
startJSButton.TextContent = "Start JS";
startJSButton.OnClick += () =>
{
    runIt = false;
    JS.CallVoid("startJSAnimation");
};
document.Body!.Append(startJSButton);

using var startCSButton = document.CreateElement<HTMLButtonElement>("button");
startCSButton.TextContent = "Start CS";
startCSButton.OnClick += () =>
{
    if (runIt) return;
    JS.CallVoid("stopJSAnimation");
    startAnimation();
};
document.Body!.Append(startCSButton);

document.Body!.Append(fpsDisplay);
document.Body!.Append(canvas);

// update fps display
fpsDisplay.TextContent = $"C#";

JS.CallVoid("initJS");

// create the animationFrame callback
cb = Callback.Create((double currentTime) =>
{
    var frameStart = performance.Call<double>("now");
    // Smoothly scale the view boundary over time
    scale = 1.0 + Math.Sin(currentTime * 0.0005) * 0.4;

    int p = 0;
    for (double y = 0; y < height; y++)
    {
        // Map y to complex imaginary plane, scaled dynamically
        double ci = ((y / height) * 2.5 - 1.25) * scale - 0.1;

        for (double x = 0; x < width; x++)
        {
            // Map x to complex real plane, scaled dynamically
            double cr = ((x / width) * 3.5 - 2.5) * scale - 0.7;

            double zr = 0; double zi = 0; double n = 0;
            // Escape time algorithm loop
            while (n < maxIter && (zr * zr + zi * zi) <= 4)
            {
                double temp = zr * zr - zi * zi + cr;
                zi = 2 * zr * zi + ci;
                zr = temp;
                n++;
            }

            // Color mapping based on escape iteration
            double r = 0, g = 0, b = 0;
            if (n < maxIter)
            {
                // Dynamic color shifting using the current time
                double wave = Math.Sin(n * 0.1 + currentTime * 0.002);
                r = Math.Floor((wave + 1) * 127.5);
                g = Math.Floor((Math.Cos(n * 0.05) + 1) * 127.5);
                b = Math.Floor((n / maxIter) * 255);
            }

            unsafe
            {
                byte* pDest = (byte*)(heapView.Pointer.ToPointer()) + p; // p should be an int, not a double
                *pDest = (byte)r;
                pDest[1] = (byte)g;
                pDest[2] = (byte)b;
                pDest[3] = 255;
            }

            //data[(int)p] = (byte)r;
            //data[(int)p + 1] = (byte)g;
            //data[(int)p + 2] = (byte)b;
            //data[(int)p + 3] = 255;
            p += 4;
        }
    }

    // get an ImageData view of our data to give to the canvas 2d context
    ctx.PutImageData(imgData, 0, 0);

    // Performance Tracking
    double frameEnd = performance.Call<double>("now");
    double duration = frameEnd - frameStart;
    frameCount++;

    if (currentTime > lastTime + 1000)
    {
        fps = Math.Round((frameCount * 1000) / (currentTime - lastTime));
        frameCount = 0;
        lastTime = currentTime;
    }
    // update fps display
    fpsDisplay.TextContent = $"C# FPS: {fps} | Frame Time: {Math.Round(duration, 1)}ms";
    /// request next frame
    if (runIt) JS.CallVoid("requestAnimationFrame", cb);
});

void startAnimation()
{
    if (runIt) return;
    runIt = true;
    /// request first frame
    JS.CallVoid("requestAnimationFrame", cb!);
}

// keep the using app alive
await new TaskCompletionSource().Task;

// POCO used to exercise PocoMarshaller. Accessors are referenced above (initializer + reads) so they
// survive trimming; the Json attributes drive naming / ignore behavior.
public class TestPerson
{
    [JsonPropertyName("given_name")]
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int Age { get; set; }
    [JsonIgnore]
    public string? Secret { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Nickname { get; set; }
    public string? City { get; set; }
}

// Numeric flags enum used to exercise EnumMarshaller / EnumNullableMarshaller.
[Flags]
public enum DemoFlags : uint
{
    None = 0,
    CopySrc = 0x0004,
    CopyDst = 0x0008,
    Storage = 0x0080,
}