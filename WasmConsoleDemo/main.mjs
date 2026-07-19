import { dotnet } from './_framework/dotnet.js'
const { setModuleImports, getAssemblyExports, getConfig, runMainAndExit } = await dotnet.withDiagnosticTracing(false).create();
await runMainAndExit();