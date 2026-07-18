using SpawnDev.SpawnJS;
using System;

var JS = new SpawnJSRuntime();
SpawnJSRuntime.Verbose = true;


using var uint8Array = JS.New("Uint8Array", 100);

JS.Set("_uint8", uint8Array);

uint8Array.CallVoid("set", new byte[] { 1, 3, 5, 7, 9 });

JS.Log("uint8array:", uint8Array.Get<int>("length"), uint8Array);
