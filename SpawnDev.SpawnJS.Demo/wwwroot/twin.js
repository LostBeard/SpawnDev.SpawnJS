// Boots TWO independent .Net runtimes of this app in one page - the shape of a SpawnJS custom element dropped
// onto a page that already runs a SpawnJS app. The second runtime gets its own copies of the runtime's Javascript
// modules (the ?twin query makes each a separate module instance), so it has its own WebAssembly memory, its own
// SpawnJSRuntime and its own call tape. SpawnJSInterop is shared, exactly as it is between two real apps.
const url = import.meta.resolve('./_framework/dotnet.js');
const { dotnet: dotnetA } = await import(url);
const { dotnet: dotnetB } = await import(url + '?twin');
// loadBootResource passes type "dotnetjs" for every Javascript module; which module it is comes in behavior
const separateModules = (type, name, defaultUri, integrity, behavior) =>
    (behavior ?? '').startsWith('js-module') ? defaultUri + (defaultUri.includes('?') ? '&' : '?') + 'twin' : defaultUri;
const a = await dotnetA.withApplicationArguments('twin', 'A').create();
const b = await dotnetB.withApplicationArguments('twin', 'B').withResourceLoader(separateModules).create();
const results = await Promise.allSettled([a.runMain(), b.runMain()]);
console.log(`RESULTS: twin ${results.map((r, i) => `${'AB'[i]} ${r.status}${r.reason ? ' ' + r.reason : ''}`).join(', ')}`);
