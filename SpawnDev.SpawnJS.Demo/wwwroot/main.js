import { dotnet } from './_framework/dotnet.js'
const { setModuleImports, getAssemblyExports, getConfig, runMain } = await dotnet.withApplicationArguments("start").create();
// ?bench= runs the interop benchmark; wrap the interop entry points BEFORE .Net binds its JSImports
if (new URLSearchParams(location.search).has('bench')) globalThis.__sjsInstallCrossingCounter();
await runMain();
