# SpawnJS.TestRunner

Runs the SpawnJS test suite in a real browser and reports the results on the command line.

```
dotnet run --project SpawnJS.TestRunner                      # everything
dotnet run --project SpawnJS.TestRunner -- JSToNet           # only tests whose name contains "JSToNet"
dotnet run --project SpawnJS.TestRunner -- --headed          # watch it in a visible browser
dotnet run --project SpawnJS.TestRunner -- --verbose         # also print console output from the page
dotnet run --project SpawnJS.TestRunner -- --url http://localhost:5012/   # reuse a running dev server
```

Exit code is the number of failed tests, so it works as a gate.

## How it works

1. Starts `dotnet run -c Release --project WasmBrowserDemo` and waits for it to print its app url.
2. Launches Chrome through Playwright (installed Chrome first, bundled chromium as fallback).
3. Navigates to the app. `WasmBrowserDemo` runs the suite on startup and writes one
   `TEST: Name|Result|DurationMs|Detail` line per test plus a final `RESULTS:` summary.
4. Parses those lines, prints them, and shuts the server down.

There is no separate test host: the app under test *is* the test host, so the tests run against the
real runtime in the real browser. Opening the app in your own browser and reading the console gives
you the identical output - `?filter=Name` scopes the run from the address bar.

## Adding tests

Put a `[SpawnJSTest]` method on a class listed in `TestsShared.TestSuiteRunner.TestTypes`. The method
takes no arguments and returns `Task`. Pass by returning, fail by throwing, skip by throwing
`SkipTestException`. The test class constructor takes the `SpawnJSRuntime`, and a fresh instance is
created per test so no test can inherit state from another.
