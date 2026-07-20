// Common and custom (Blazor specific) definitions
// https://webidl.spec.whatwg.org/#common

// https://webidl.spec.whatwg.org/#ArrayBufferView
// typedef (TypedArray or DataView) ArrayBufferView;

global using ArrayBufferView = SpawnDev.SpawnJS.Union
    <
    SpawnDev.SpawnJS.JSObjects.TypedArray,
    SpawnDev.SpawnJS.JSObjects.DataView,
    byte[]
    >;

// https://html.spec.whatwg.org/multipage/imagebitmap-and-animations.html#imagedataarray

// https://webidl.spec.whatwg.org/#AllowSharedBufferSource
global using AllowSharedBufferSource = SpawnDev.SpawnJS.Union
    <
    SpawnDev.SpawnJS.JSObjects.ArrayBuffer,
    SpawnDev.SpawnJS.JSObjects.SharedArrayBuffer,
    SpawnDev.SpawnJS.JSObjects.TypedArray,
    SpawnDev.SpawnJS.JSObjects.DataView,
    byte[]
    >;
// https://webidl.spec.whatwg.org/#BufferSource
// typedef (ArrayBufferView or ArrayBuffer) BufferSource;
global using BufferSource = SpawnDev.SpawnJS.Union
    <
    SpawnDev.SpawnJS.JSObjects.ArrayBuffer,
    SpawnDev.SpawnJS.JSObjects.TypedArray,
    SpawnDev.SpawnJS.JSObjects.DataView,
    byte[]
    >;
global using CanvasImageSource = SpawnDev.SpawnJS.Union<
    SpawnDev.SpawnJS.JSObjects.HTMLImageElement,
    SpawnDev.SpawnJS.JSObjects.SVGImageElement,
    SpawnDev.SpawnJS.JSObjects.HTMLVideoElement,
    SpawnDev.SpawnJS.JSObjects.HTMLCanvasElement,
    SpawnDev.SpawnJS.JSObjects.ImageBitmap,
    SpawnDev.SpawnJS.JSObjects.OffscreenCanvas,
    SpawnDev.SpawnJS.JSObjects.VideoFrame
>;
global using ConstrainBoolean = SpawnDev.SpawnJS.Union<bool, SpawnDev.SpawnJS.JSObjects.ConstrainBooleanParameters>;
global using ConstrainDOMString = SpawnDev.SpawnJS.Union<string, string[], SpawnDev.SpawnJS.JSObjects.ConstrainDOMStringParameters>;
global using ConstrainDouble = SpawnDev.SpawnJS.Union<double, SpawnDev.SpawnJS.JSObjects.ConstrainDoubleRange>;
global using ConstrainULong = SpawnDev.SpawnJS.Union<ulong, SpawnDev.SpawnJS.JSObjects.ConstrainULongRange>;
global using ImageBitmapSource = SpawnDev.SpawnJS.Union<
    SpawnDev.SpawnJS.JSObjects.HTMLImageElement,
    SpawnDev.SpawnJS.JSObjects.SVGImageElement,
    SpawnDev.SpawnJS.JSObjects.HTMLVideoElement,
    SpawnDev.SpawnJS.JSObjects.HTMLCanvasElement,
    SpawnDev.SpawnJS.JSObjects.ImageBitmap,
    SpawnDev.SpawnJS.JSObjects.OffscreenCanvas,
    SpawnDev.SpawnJS.JSObjects.VideoFrame,
    SpawnDev.SpawnJS.JSObjects.Blob,
    SpawnDev.SpawnJS.JSObjects.ImageData
>;
// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
