// SpawnJS ships its interop as a JS initializer (*.lib.module.js). A browser wasm app gets that
// loaded automatically by the WebAssembly SDK; a console wasm app does not, so import it here
// before the runtime starts. It defines globalThis.SpawnJSInterop, which SpawnJSRuntime constructs.
import './SpawnDev.SpawnJS.lib.module.js'
import { dotnet } from './_framework/dotnet.js'
const { setModuleImports, getAssemblyExports, getConfig, runMainAndExit } = await dotnet.withDiagnosticTracing(false).create();
await runMainAndExit();
