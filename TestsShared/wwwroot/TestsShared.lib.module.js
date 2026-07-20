// an async method uses setTimeout to sleep then return 42
globalThis.sleep = async function (/* double */ duration) {
    await new Promise((resolve) => setTimeout(resolve, duration));
    return 42;
}
// awaits the passed in Promise and returns its value
globalThis.waitForTask = async function (/* Promise */ testTask) {
    return await testTask;
}
// this method alternates between returning a sync results and async (Promise) result for testing handling
var syncASyncToggle = false;
globalThis.syncAndAsyncReturnsTypes = function () {
    syncASyncToggle = !syncASyncToggle;
    if (syncASyncToggle) {
        return 42;
    } else {
        return new Promise(async (resolve, reject) => {
            await sleep(1000)
            return 42;
        });
    }
}