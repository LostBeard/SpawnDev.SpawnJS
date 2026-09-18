# SpawnJS.TestRunner

Runs the SpawnJS test suite in a real browser and reports results on the command line.

```
dotnet run --project SpawnJS.TestRunner                      # everything
dotnet run --project SpawnJS.TestRunner -- JSToNet           # name contains "JSToNet"
dotnet run --project SpawnJS.TestRunner -- --headed          # visible browser
dotnet run --project SpawnJS.TestRunner -- --verbose         # page console too
dotnet run --project SpawnJS.TestRunner -- --url http://localhost:5012/   # reuse a server
```

Exit code is the failure count.

## How it works

1. Starts `dotnet run -c Release --project SpawnDev.SpawnJS.Demo` and waits for the app URL.
2. Launches Chrome through Playwright (installed Chrome first, bundled Chromium as fallback).
3. Navigates to `/?tests=[filter]` on `DOMContentLoaded` (not `NetworkIdle` - a SharedWorker test never goes idle).
4. Parses `TEST: Name|Result|DurationMs|Detail` lines and a final `RESULTS:` summary.

The app under test **is** the test host. Opening `SpawnDev.SpawnJS.Demo` and reading the console is the same output.

`?tests=` is handled at the top of `Program.cs` **before** scratch/demo code. That file is also the author's scratch host and ends in a bare `return`. If the suite block sits below that `return`, the harness times out with zero `TEST:` lines - that means the suite never started, not that a case hung.

A filtered run skips non-matching cases, so `RESULTS:` counts only what ran. Confirm scoping by count (unfiltered vs a three-case filter).

Every csproj in this repo references `SpawnDev.SpawnJS` with a `ProjectReference` in both Debug and Release, so `dotnet run -c Release` tests this working tree.

## Adding tests

Put cases in `SpawnDev.SpawnJS.Demo/UnitTests/` and add the class to the sequence in `MarshallerTests.Run()`. A class that is not in that list does not run and the `Ran:` count stays put.
