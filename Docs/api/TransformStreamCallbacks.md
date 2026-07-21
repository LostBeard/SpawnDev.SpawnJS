# TransformStreamCallbacks

**Namespace:** `SpawnDev.SpawnJS.JSObjects`  
**Inherits:** `IDisposable`  
**Source:** `JSObjects/TransformStreamCallbacks.cs`  

> Options for the TransformStream constructor.

## Constructors

| Signature | Description |
|---|---|
| `TransformStreamCallbacks(Func<TransformStreamDefaultController, Task>? start, Func<SpawnJSObject, TransformStreamDefaultController, Task>? transform, Func<TransformStreamDefaultController, Task>? flush)` | Creates a new instance of TransformerAsync with the specified callbacks. |
| `TransformStreamCallbacks(Func<TransformStreamDefaultController, Task>? start, Func<VideoFrame, TransformStreamDefaultController, Task>? transform, Func<TransformStreamDefaultController, Task>? flush)` | Creates a new instance of TransformerAsync with the specified callbacks. |
| `TransformStreamCallbacks(Action<TransformStreamDefaultController>? start, Action<SpawnJSObject, TransformStreamDefaultController>? transform, Action<TransformStreamDefaultController>? flush)` | Creates a new instance of TransformerAsync with the specified callbacks. |
| `TransformStreamCallbacks(Action<TransformStreamDefaultController>? start, Action<VideoFrame, TransformStreamDefaultController>? transform, Action<TransformStreamDefaultController>? flush)` | Creates a new instance of TransformerAsync with the specified callbacks. |
| `TransformStreamCallbacks(Func<TransformStreamDefaultController, Task>? start, Func<T, TransformStreamDefaultController, Task>? transform, Func<TransformStreamDefaultController, Task>? flush)` | Creates a new instance of TransformerAsync with the specified callbacks. |
| `TransformStreamCallbacks(Action<TransformStreamDefaultController>? start, Action<T, TransformStreamDefaultController>? transform, Action<TransformStreamDefaultController>? flush)` | Creates a new instance of TransformerAsync with the specified callbacks. |

## Properties

| Property | Type | Access | Description |
|---|---|---|---|
| `Start` | `Callback?` | get |  |
| `Transform` | `Callback?` | get |  |
| `Flush` | `Callback?` | get |  |

