# CSSRuleList

**Namespace:** `SpawnDev.SpawnJS.JSObjects`  
**Inherits:** `SpawnJSObject`  
**Source:** `JSObjects/CSSRuleList.cs`  
**MDN Reference:** [CSSRuleList on MDN](https://developer.mozilla.org/en-US/docs/Web/API/CSSRuleList)

> A CSSRuleList represents an ordered collection of read-only CSSRule objects. While the CSSRuleList object is read-only, and cannot be directly modified, it is considered a live object, as the content can change over time. To edit the underlying rules returned by CSSRule objects, use CSSStyleSheet.insertRule() and CSSStyleSheet.deleteRule(), which are methods of CSSStyleSheet. https://developer.mozilla.org/en-US/docs/Web/API/CSSRuleList

## Constructors

| Signature | Description |
|---|---|
| `CSSRuleList(SpawnJSObjectReference _ref)` | Deserialization constructor |

## Properties

| Property | Type | Access | Description |
|---|---|---|---|
| `Length` | `int` | get | Returns an integer representing the number of CSSRule objects in the collection. |

## Methods

| Method | Return Type | Description |
|---|---|---|
| `Item(int index)` | `CSSRule?` | Gets a single CSSRule. |
| `ToList()` | `List<CSSRule>` | Returns as a list |
| `ToArray()` | `CSSRule[]` | Returns as an array |

## Examples

> **SpawnDev.SpawnJS Mapping Note:** The JavaScript examples below show the standard Web API usage.
> In SpawnDev.SpawnJS, the same API is available with C# conventions:
> - Properties and methods use **PascalCase** (e.g., `readyState` becomes `ReadyState`)
> - Events use **ActionEvent** with `+=`/`-=` (e.g., `addEventListener("click", fn)` becomes `OnClick += handler`)
> - Async methods return `Task<T>` instead of `Promise<T>`
> - Objects should be disposed with `using` statements
> - Access via `JS.Get<CSSRuleList>(...)` or constructor `new CSSRuleList(...)`

### JavaScript

```js
let myRules = document.styleSheets[0].cssRules;
console.log(myRules);
console.log(myRules.length);
console.log(myRules[0]);
```

*[See full example on MDN](https://developer.mozilla.org/en-US/docs/Web/API/CSSRuleList)*

## Examples

### CSS

```css
body {
  font-family:
    system-ui,
    -apple-system,
    sans-serif;
  margin: 2em;
}

.container {
  display: grid;
  grid-template-columns: repeat(auto-fill, 200px);
}

.container > * {
  background-color: #3740ff;
  color: white;
}
```

### JavaScript

**JavaScript (MDN):**

```js
let myRules = document.styleSheets[0].cssRules;
console.log(myRules);
console.log(myRules.length);
console.log(myRules[0]);
```

**C# (SpawnDev.SpawnJS):**

```csharp
// Requires: builder.Services.AddSpawnJSRuntime();
// Inject SpawnJSRuntime in your component or service:
// [Inject] SpawnJSRuntime JS { get; set; }

var myRules = document.styleSheets[0].cssRules;
Console.WriteLine(myRules);
Console.WriteLine(myRules.Length);
Console.WriteLine(myRules[0]);
```

