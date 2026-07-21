# SpeechSynthesis

**Namespace:** `SpawnDev.SpawnJS.JSObjects.Speech`  
**Inheritance:** `SpawnJSObject` -> `EventTarget` -> `SpeechSynthesis`  
**MDN Reference:** [SpeechSynthesis - MDN](https://developer.mozilla.org/en-US/docs/Web/API/SpeechSynthesis)

> The `SpeechSynthesis` interface of the Web Speech API is the controller for the text-to-speech service. It can be used to get available voices, check speaking status, and speak/pause/resume/cancel utterances.

## Constructors

| Signature | Description |
|---|---|
| `SpeechSynthesis(SpawnJSObjectReference _ref)` | Deserialization constructor. Access via `JS.Get<SpeechSynthesis>("speechSynthesis")`. |

## Properties

| Property | Type | Description |
|---|---|---|
| `Speaking` | `bool` | `true` if the synthesis is currently speaking. |
| `Pending` | `bool` | `true` if utterances are queued. |
| `Paused` | `bool` | `true` if speech is paused. |

## Methods

| Method | Return Type | Description |
|---|---|---|
| `Speak(SpeechSynthesisUtterance utterance)` | `void` | Speaks the given utterance. |
| `Cancel()` | `void` | Cancels all speech. |
| `Pause()` | `void` | Pauses speech. |
| `Resume()` | `void` | Resumes paused speech. |
| `GetVoices()` | `SpeechSynthesisVoice[]` | Returns available voices. |

## Events

| Event | Type | Description |
|---|---|---|
| `OnVoicesChanged` | `ActionEvent<Event>` | Fired when the list of available voices changes. |

## Example

```csharp
using var synthesis = JS.Get<SpeechSynthesis>("speechSynthesis");

// Get voices
var voices = synthesis.GetVoices();
Console.WriteLine($"Available voices: {voices.Length}");

// Speak text
using var utterance = new SpeechSynthesisUtterance("Hello from SpawnDev SpawnJS!");
utterance.Rate = 1.0f;
utterance.Pitch = 1.0f;
synthesis.Speak(utterance);
```
