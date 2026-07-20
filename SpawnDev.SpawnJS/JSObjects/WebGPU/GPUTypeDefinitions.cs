// https://www.w3.org/TR/webgpu/#type-definitions

global using GPUBindingResource = SpawnDev.SpawnJS.Union
<
    SpawnDev.SpawnJS.JSObjects.GPUSampler,
    SpawnDev.SpawnJS.JSObjects.GPUTexture,
    SpawnDev.SpawnJS.JSObjects.GPUTextureView,
    SpawnDev.SpawnJS.JSObjects.GPUBuffer,
    SpawnDev.SpawnJS.JSObjects.GPUBufferBinding,
    SpawnDev.SpawnJS.JSObjects.GPUExternalTexture
>;
global using GPUBufferDynamicOffset = System.UInt32;
// typedef (sequence<double> or GPUColorDict) GPUColor;
global using GPUColor = SpawnDev.SpawnJS.Union<System.Collections.Generic.IEnumerable<System.Double>, SpawnDev.SpawnJS.JSObjects.GPUColorDict>;
// https://www.w3.org/TR/webgpu/#typedefdef-gpucopyexternalimagesource
// typedef (ImageBitmap or ImageData or HTMLImageElement or HTMLVideoElement or VideoFrame or HTMLCanvasElement or OffscreenCanvas) GPUCopyExternalImageSource;
global using GPUCopyExternalImageSource = SpawnDev.SpawnJS.Union
<
    SpawnDev.SpawnJS.JSObjects.ImageBitmap,
    SpawnDev.SpawnJS.JSObjects.ImageData,
    SpawnDev.SpawnJS.JSObjects.HTMLImageElement,
    SpawnDev.SpawnJS.JSObjects.HTMLVideoElement,
    SpawnDev.SpawnJS.JSObjects.VideoFrame,
    SpawnDev.SpawnJS.JSObjects.HTMLCanvasElement,
    SpawnDev.SpawnJS.JSObjects.OffscreenCanvas
>;
// typedef (sequence<GPUIntegerCoordinate> or GPUExtent3DDict) GPUExtent3D;
global using GPUExtent3D = SpawnDev.SpawnJS.Union<System.Collections.Generic.IEnumerable<System.UInt32>, SpawnDev.SpawnJS.JSObjects.GPUExtent3DDict>;
global using GPUFlagsConstant = System.UInt32;
global using GPUIndex32 = System.UInt32;
global using GPUIntegerCoordinate = System.UInt32;
global using GPUIntegerCoordinateOut = System.UInt32;
// typedef (sequence<GPUIntegerCoordinate> or GPUOrigin2DDict) GPUOrigin2D;
global using GPUOrigin2D = SpawnDev.SpawnJS.Union<System.Collections.Generic.IEnumerable<System.UInt32>, SpawnDev.SpawnJS.JSObjects.GPUOrigin2DDict>;
// https://www.w3.org/TR/webgpu/#typedefdef-gpuorigin3d
global using GPUOrigin3D = SpawnDev.SpawnJS.Union<System.Collections.Generic.IEnumerable<System.UInt32>, SpawnDev.SpawnJS.JSObjects.GPUOrigin3DDict>;
global using GPUSampleMask = System.UInt32;
global using GPUSignedOffset32 = System.Int32;
global using GPUSize32 = System.UInt32;
global using GPUSize32Out = System.UInt32;
global using GPUSize64 = System.UInt64;
global using GPUSize64Out = System.UInt64;
global using GPUStencilValue = System.UInt32;
// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
