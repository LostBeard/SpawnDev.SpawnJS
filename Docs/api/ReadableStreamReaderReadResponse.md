# ReadableStreamReaderReadResponse

**Namespace:** `SpawnDev.SpawnJS.JSObjects`  
**Inherits:** `SpawnJSObject`  
**Source:** `JSObjects/ReadableStreamReaderReadResponse.cs`  

> Returned by ReadableStreamDefaultReader.Read

## Constructors

| Signature | Description |
|---|---|
| `ReadableStreamReaderReadResponse(SpawnJSObjectReference _ref)` | Deserialization constructor |

## Properties

| Property | Type | Access | Description |
|---|---|---|---|
| `Done` | `bool` | get | True if there is no more data to read |
| `Value` | `Uint8Array?` | get | The current chunk if not done |

