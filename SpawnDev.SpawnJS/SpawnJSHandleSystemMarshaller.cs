//using System.Runtime.InteropServices.JavaScript;
//using System.Runtime.InteropServices.Marshalling;

//namespace SpawnDev.SpawnJS
//{
//    public static unsafe class SpawnJSHandleMarshaler
//    {
//        // Extracted out of JS into C#
//        public static void ToManaged(ref this JSMarshalerArgument arg, out SpawnJSHandle value)
//        {
//            // Use the native JSObject extractor built into the runtime
//            arg.ToManaged(out JSObject? jsObj);
//            value = jsObj == null ? null! : new SpawnJSHandle(jsObj);
//        }

//        // Pushed from C# into JS
//        public static void ToJS(ref this JSMarshalerArgument arg, SpawnJSHandle value)
//        {
//            // Use the native JSObject injection built into the runtime
//            arg.ToJS(value?.JSObject);
//        }
//    }
//    public static unsafe class SpawnJSInteropBoundary
//    {
//        // Example: A method called FROM JavaScript passing a SpawnJSHandle to C#
//        public static void ReceiveHandleFromJS(JSMarshalerArgument* arguments_buffer)
//        {
//            ref JSMarshalerArgument exception_arg = ref arguments_buffer[0];
//            ref JSMarshalerArgument return_arg = ref arguments_buffer[1];
//            ref JSMarshalerArgument param1_arg = ref arguments_buffer[2];

//            try
//            {
//                // Uses your custom extension method to resolve the argument
//                param1_arg.ToManaged(out SpawnJSHandle handle);

//                // Execute logic
//                Console.WriteLine($"Received handle! Valid: {handle.JSObject != null}");

//                return_arg.ToJS(true); // Return true to JS
//            }
//            catch (Exception ex)
//            {
//                exception_arg.ToJS(ex);
//            }
//        }
//    }
//}
