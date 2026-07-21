# CSSRule

**Namespace:** `SpawnDev.SpawnJS.JSObjects`  
**Inherits:** `SpawnJSObject`  
**Source:** `JSObjects/CSSRule.cs`  
**MDN Reference:** [CSSRule on MDN](https://developer.mozilla.org/en-US/docs/Web/API/CSSRule)

> The CSSRule interface represents a single CSS rule. There are several types of rules which inherit properties from CSSRule. https://developer.mozilla.org/en-US/docs/Web/API/CSSRule

## Constructors

| Signature | Description |
|---|---|
| `CSSRule(SpawnJSObjectReference _ref)` | Deserialization constructor |

## Properties

| Property | Type | Access | Description |
|---|---|---|---|
| `CSSText` | `string` | get | Returns the textual representation of the rule. |
| `ParentRule` | `CSSRule?` | get | Returns the containing rule, if any. |
| `ParentStyleSheet` | `CSSStyleSheet?` | get | Returns the stylesheet object in which the rule is defined. |
| `Type` | `ushort` | get | Returns the type of the rule, as an unsigned short. |

## Examples

> **SpawnDev.SpawnJS Mapping Note:** The JavaScript examples below show the standard Web API usage.
> In SpawnDev.SpawnJS, the same API is available with C# conventions:
> - Properties and methods use **PascalCase** (e.g., `readyState` becomes `ReadyState`)
> - Events use **ActionEvent** with `+=`/`-=` (e.g., `addEventListener("click", fn)` becomes `OnClick += handler`)
> - Async methods return `Task<T>` instead of `Promise<T>`
> - Objects should be disposed with `using` statements
> - Access via `JS.Get<CSSRule>(...)` or constructor `new CSSRule(...)`

```js
let myRules = document.styleSheets[0].cssRules; // Returns a CSSRuleList
console.log(myRules);
```

*[See full example on MDN](https://developer.mozilla.org/en-US/docs/Web/API/CSSRule)*

## Examples

**JavaScript (MDN):**

```js
let myRules = document.styleSheets[0].cssRules; // Returns a CSSRuleList
console.log(myRules);
```

**C# (SpawnDev.SpawnJS):**

```csharp
// Requires: builder.Services.AddSpawnJSRuntime();
// Inject SpawnJSRuntime in your component or service:
// [Inject] SpawnJSRuntime JS { get; set; }

var myRules = document.styleSheets[0].cssRules; // Returns a CSSRuleList;
Console.WriteLine(myRules);
```

