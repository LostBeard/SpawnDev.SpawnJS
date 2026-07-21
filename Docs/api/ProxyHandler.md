# ProxyHandler

**Namespace:** `SpawnDev.SpawnJS.JSObjects`  
**Inherits:** `IDisposable`  
**Source:** `JSObjects/ProxyHandler.cs`  

> An object whose properties are functions that define the behavior of the proxy when an operation is performed on it.

## Properties

| Property | Type | Access | Description |
|---|---|---|---|
| `Apply` | `Func<SpawnJSObject, SpawnJSObject, Array<SpawnJSObject?>, object?>?` | get |  |
| `ApplyCallback` | `FuncCallback<SpawnJSObject, SpawnJSObject, Array<SpawnJSObject?>, object?>?` | get |  |
| `Get` | `Func<SpawnJSObject, SpawnJSObject, SpawnJSObject, object?>?` | get |  |
| `GetCallback` | `FuncCallback<SpawnJSObject, SpawnJSObject, SpawnJSObject, object?>?` | get |  |
| `Construct` | `Func<SpawnJSObject, Array<SpawnJSObject?>, SpawnJSObject, object?>?` | get |  |
| `ConstructCallback` | `FuncCallback<SpawnJSObject, Array<SpawnJSObject?>, SpawnJSObject, object?>?` | get |  |
| `Set` | `Func<SpawnJSObject, SpawnJSObject, SpawnJSObject, bool>?` | get/set |  |
| `SetCallback` | `FuncCallback<SpawnJSObject, SpawnJSObject, SpawnJSObject, bool>?` | get |  |
| `Has` | `Func<SpawnJSObject, SpawnJSObject, bool>?` | get |  |
| `HasCallback` | `FuncCallback<SpawnJSObject, SpawnJSObject, bool>?` | get |  |
| `OwnKeys` | `Func<SpawnJSObject, IEnumerable<Union<Symbol, string>>>?` | get |  |
| `OwnKeysCallback` | `FuncCallback<SpawnJSObject, IEnumerable<Union<Symbol, string>>>?` | get |  |
| `DeleteProperty` | `Func<SpawnJSObject, SpawnJSObject, bool>?` | get |  |
| `DeletePropertyCallback` | `FuncCallback<SpawnJSObject, SpawnJSObject, bool>?` | get |  |

