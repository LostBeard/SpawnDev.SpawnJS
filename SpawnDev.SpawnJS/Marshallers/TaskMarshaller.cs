using System.Diagnostics.CodeAnalysis;
using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls Task as Promise
    /// </summary>
    public class TaskMarshaller<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T> : JSMarshallerFromSpawnJSObjectReference<Task<T>?>
    {
        public override Task<T>? JSToNet(SpawnJSObjectReference value)
        {
            if (value == null) return null;
            var tcs = new TaskCompletionSource<T>();
            Callback? onResolve = null;
            Callback? onReject = null;
            onResolve = Callback.CreateOne<T>((value) =>
            {
                onReject?.Dispose();
                tcs.TrySetResult(value);
            });
            onReject = Callback.CreateOne((string error) =>
            {
                onResolve?.Dispose();
                tcs.TrySetException(new Exception(error));
            });
            value.CallVoid("then", onResolve, onReject);
            return tcs.Task;
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, Task<T>? value)
        {
            if (value == null)
            {
                jsParent.PropertySetNull(jsKey);
                return;
            }
            if (value.IsCompleted)
            {
                if (value.IsCompletedSuccessfully)
                {
                    var returnValue = value.Result;
                    JS.InteropCall<double, int, T, VoidType>("propertySetResolvedPromise", jsParent.Id, jsKey, returnValue);
                }
                else
                {
                    var error = value.Exception?.ToString() ?? "Unknown error";
                    JS.InteropCall<double, int, string, VoidType>("propertySetRejectedPromise", jsParent.Id, jsKey, error);
                }
                return;
            }
            var promise = JS.InteropCall<double, int, SpawnJSObjectReference>("propertySetNewPromise", jsParent.Id, jsKey);
            value.ContinueWith((t) =>
            {
                using (promise)
                {
                    if (t.IsCompletedSuccessfully)
                    {
                        var returnValue = value.Result;
                        promise.CallVoid("resolve", returnValue);
                    }
                    else
                    {
                        var error = t.Exception?.ToString() ?? "Unknown error";
                        promise.CallVoid("reject", error);
                    }
                }
            });
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, Task<T>? value)
        {
            if (value == null)
            {
                jsParent.PropertySetNull(jsKey);
                return;
            }
            if (value.IsCompleted)
            {
                if (value.IsCompletedSuccessfully)
                {
                    JS.InteropCall<double, string, VoidType>("propertySetResolvedPromise", jsParent.Id, jsKey);
                }
                else
                {
                    var error = value.Exception?.ToString() ?? "Unknown error";
                    JS.InteropCall<double, string, string, VoidType>("propertySetRejectedPromise", jsParent.Id, jsKey, error);
                }
                return;
            }
            var promise = JS.InteropCall<double, string, SpawnJSObjectReference>("propertySetNewPromise", jsParent.Id, jsKey);
            value.ContinueWith((t) =>
            {
                using (promise)
                {
                    if (t.IsCompletedSuccessfully)
                    {
                        var returnValue = value.Result;
                        promise.CallVoid("resolve", returnValue);
                    }
                    else
                    {
                        var error = t.Exception?.ToString() ?? "Unknown error";
                        promise.CallVoid("reject", error);
                    }
                }
            });
        }
    }
    /// <summary>
    /// Marshalls Task as Promise
    /// </summary>
    public class TaskMarshaller : JSMarshallerFromSpawnJSObjectReference<Task?>
    {
        public override bool CanMarshal(Type type)
        {
            if (type == typeof(Task)) return true;
            var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
            if (genericType == typeof(Task<>)) return true;
            return false;
        }
        [UnconditionalSuppressMessage("Trimming", "IL2076", Justification = "See IL2055.")]
        [UnconditionalSuppressMessage("Trimming", "IL2055",
            Justification = "GetMarshaller specializes to the Task result type obtained by reflection (GetGenericArguments), which cannot carry DynamicallyAccessedMembers. Built-in wrapper result ctors are preserved by the embedded ILLink.Descriptors.xml; a consumer awaiting a Task of a custom SpawnJSObject wrapper in a trimmed app must preserve that type's ctor itself (reflection boundary).")]
        public override JSMarshaller<T> GetMarshaller<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        {
            if (this is JSMarshaller<T> _this) return _this;
            var typeT = typeof(T);
            var returnType = typeT.GetGenericArguments()[0];
            var marshallerTyped = typeof(TaskMarshaller<>).MakeGenericType(returnType);
            return (JSMarshaller<T>)Activator.CreateInstance(marshallerTyped)!;
        }
        public override Task? JSToNet(SpawnJSObjectReference value)
        {
            if (value == null) return null;
            var tcs = new TaskCompletionSource();
            Callback? onResolve = null;
            Callback? onReject = null;
            onResolve = Callback.CreateOne(() =>
            {
                onReject?.Dispose();
                tcs.TrySetResult();
            });
            onReject = Callback.CreateOne((string error) =>
            {
                onResolve?.Dispose();
                tcs.TrySetException(new Exception(error));
            });
            value.CallVoid("then", onResolve, onReject);
            return tcs.Task;
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, Task? value)
        {
            if (value == null)
            {
                jsParent.PropertySetNull(jsKey);
                return;
            }
            if (value.IsCompleted)
            {
                if (value.IsCompletedSuccessfully)
                {
                    JS.InteropCall<double, int, VoidType>("propertySetResolvedPromise", jsParent.Id, jsKey);
                }
                else
                {
                    var error = value.Exception?.ToString() ?? "Unknown error";
                    JS.InteropCall<double, int, string, VoidType>("propertySetRejectedPromise", jsParent.Id, jsKey, error);
                }
                return;
            }
            var promise = JS.InteropCall<double, int, SpawnJSObjectReference>("propertySetNewPromise", jsParent.Id, jsKey);
            value.ContinueWith((t) =>
            {
                using (promise)
                {
                    if (t.IsCompletedSuccessfully)
                    {
                        promise.CallVoid("resolve");
                    }
                    else
                    {
                        var error = t.Exception?.ToString() ?? "Unknown error";
                        promise.CallVoid("reject", error);
                    }
                }
            });
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, Task? value)
        {
            if (value == null)
            {
                jsParent.PropertySetNull(jsKey);
                return;
            }
            if (value.IsCompleted)
            {
                if (value.IsCompletedSuccessfully)
                {
                    JS.InteropCall<double, string, VoidType>("propertySetResolvedPromise", jsParent.Id, jsKey);
                }
                else
                {
                    var error = value.Exception?.ToString() ?? "Unknown error";
                    JS.InteropCall<double, string, string, VoidType>("propertySetRejectedPromise", jsParent.Id, jsKey, error);
                }
                return;
            }
            var promise = JS.InteropCall<double, string, SpawnJSObjectReference>("propertySetNewPromise", jsParent.Id, jsKey);
            value.ContinueWith((t) =>
            {
                using (promise)
                {
                    if (t.IsCompletedSuccessfully)
                    {
                        promise.CallVoid("resolve");
                    }
                    else
                    {
                        var error = t.Exception?.ToString() ?? "Unknown error";
                        promise.CallVoid("reject", error);
                    }
                }
            });
        }
    }
}
