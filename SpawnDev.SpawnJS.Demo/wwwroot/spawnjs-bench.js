// Fixture for the interop benchmark (?bench=). Loaded by index.html; the crossing counter is installed by
// main.js only when the page was opened with ?bench, so the unit test suite never runs through it.
(function () {
    const dto16 = {
        int1: 1, int2: 2, int3: 3, int4: 4,
        dbl1: 1.5, dbl2: 2.5, dbl3: 3.5, dbl4: 4.5,
        str1: 'setBindGroup', str2: 'rgba8unorm', str3: 'compute',
        flag1: true, flag2: false,
        ints: [1, 2, 3, 4],
        inner: { a: 7, b: 8.5, c: 'inner' },
        maybe: null,
    };
    const ints1000 = Array.from({ length: 1000 }, (_, i) => i);
    globalThis.__bench = {
        last: null,
        num: 42.5,
        layout: { kind: 'layout' },
        buf0: { kind: 'buffer0' },
        buf1: { kind: 'buffer1' },
        buf2: { kind: 'buffer2' },
        sink0() { this.last = 0; },
        sink1(a) { this.last = a; },
        sink5(a, b, c, d, e) { this.last = e; },
        makeDto16() { return dto16; },
        makeInts1000() { return ints1000; },
    };

    // Counts .Net -> JS boundary crossings. Every JSImport SpawnJS makes lands on a static member of
    // SpawnJSInterop, so each static is wrapped; a call counts only when it is entered from OUTSIDE the
    // interop layer (depth 0), which excludes SpawnJSInterop calling itself. Counting is exact no matter
    // how loaded the machine is, which timing is not.
    globalThis.__sjsInstallCrossingCounter = function () {
        const I = globalThis.SpawnJSInterop;
        if (!I || I.__crossingCounterInstalled) return;
        I.__crossingCounterInstalled = true;
        const stats = globalThis.__sjsCrossings = {
            count: 0,
            byName: {},
            snap: '',
            reset() { this.count = 0; this.byName = {}; },
            // records the breakdown AS OF ENTRY: this call's own crossings (its argument writes and the
            // dispatcher entry) are already in byName, so the caller subtracts a calibrated overhead
            snapshot() { this.snap = JSON.stringify(this.byName); },
        };
        let depth = 0;
        // detachedEventCheck also runs from a timer - counting it would add phantom crossings
        const skip = new Set(['detachedEventCheck', 'length', 'name', 'prototype']);
        for (const name of Object.getOwnPropertyNames(I)) {
            if (skip.has(name)) continue;
            const orig = I[name];
            if (typeof orig !== 'function') continue;
            I[name] = function () {
                if (depth === 0) {
                    stats.count++;
                    stats.byName[name] = (stats.byName[name] || 0) + 1;
                }
                depth++;
                try { return orig.apply(this, arguments); }
                finally { depth--; }
            };
        }
    };
})();
