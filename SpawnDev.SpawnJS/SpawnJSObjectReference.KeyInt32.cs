using System.Diagnostics.CodeAnalysis;
using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS
{
    public partial class SpawnJSObjectReference
    {
        public string ConstructorName(int key) => TypeInfo(key).ConstructorName;
        public string TypeOf(int key) => TypeInfo(key).TypeOf;
        public (string TypeOf, string ConstructorName) TypeInfo(int key)
        {
            string? typeOf = null;
            string? constructorName = null;
            try
            {
                var tmp = SpawnJSRuntime._propertyTypeInfo(Id, key);
                var parts = tmp.Split(" ");
                typeOf = parts[0];
                constructorName = parts.Length > 1 ? parts[1] : "";
            }
            catch { }
            if (string.IsNullOrEmpty(typeOf)) typeOf = "undefined";
            if (string.IsNullOrEmpty(constructorName)) constructorName = "";
            return (typeOf, constructorName);
        }
        public bool Exists(int key) => SpawnJSRuntime._propertyIn(Id, key);
        public bool Delete(int key) => SpawnJSRuntime._propertyDelete(Id, key);
        #region Set
        public void Set<T>(int key, T value) => JS.InteropCall<double, int, T, VoidType>("propertySet", Id, key, value);
        #endregion
        #region Get
        public SpawnJSObjectReference? Get(int key) => JS.InteropCall<double, int, SpawnJSObjectReference>("propertyGet", Id, key);
        public T Get<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key) => JS.InteropCall<double, int, T>("propertyGet", Id, key);
        #endregion
        #region GetAsync
        public Task<T> GetAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key) => JS.InteropCallAsync<double, int, T>("propertyGet", Id, key);
        public Task<SpawnJSObjectReference> GetAsync(int key) => JS.InteropCallAsync<double, int, SpawnJSObjectReference>("propertyGet", Id, key);
        #endregion
        #region New
        public T NewApply<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, object?[]? args = null) 
            => JS.InteropCall<double, int, object?[]?, T>("propertyNewApply", Id, key, args);
        public T New<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key)
            => JS.InteropCall<double, int, T>("propertyNew", Id, key);
        public T New<T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, T1 arg1)
            => JS.InteropCall<double, int, T1, T>("propertyNew", Id, key, arg1);
        public T New<T1, T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, T1 arg1, T2 arg2)
            => JS.InteropCall<double, int, T1, T2, T>("propertyNew", Id, key, arg1, arg2);
        public T New<T1, T2, T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, T1 arg1, T2 arg2, T3 arg3)
            => JS.InteropCall<double, int, T1, T2, T3, T>("propertyNew", Id, key, arg1, arg2, arg3);
        public T New<T1, T2, T3, T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            => JS.InteropCall<double, int, T1, T2, T3, T4, T>("propertyNew", Id, key, arg1, arg2, arg3, arg4);
        public T New<T1, T2, T3, T4, T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            => JS.InteropCall<double, int, T1, T2, T3, T4, T5, T>("propertyNew", Id, key, arg1, arg2, arg3, arg4, arg5);
        public T New<T1, T2, T3, T4, T5, T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
            => JS.InteropCall<double, int, T1, T2, T3, T4, T5, T6, T>("propertyNew", Id, key, arg1, arg2, arg3, arg4, arg5, arg6);
        public T New<T1, T2, T3, T4, T5, T6, T7, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
            => JS.InteropCall<double, int, T1, T2, T3, T4, T5, T6, T7, T>("propertyNew", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        public T New<T1, T2, T3, T4, T5, T6, T7, T8, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
            => JS.InteropCall<double, int, T1, T2, T3, T4, T5, T6, T7, T8, T>("propertyNew", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        public T New<T1, T2, T3, T4, T5, T6, T7, T8, T9, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
            => JS.InteropCall<double, int, T1, T2, T3, T4, T5, T6, T7, T8, T9, T>("propertyNew", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        public T New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
            => JS.InteropCall<double, int, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T>("propertyNew", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);

        public SpawnJSObjectReference NewApply(int key, object?[]? args = null) 
            => JS.InteropCall<double, int, object?[]?, SpawnJSObjectReference>("propertyNewApply", Id, key, args);
        public SpawnJSObjectReference New(int key) 
            => JS.InteropCall<double, int, SpawnJSObjectReference>("propertyNew", Id, key);
        public SpawnJSObjectReference New<T1>(int key, T1 arg1)
          => JS.InteropCall<double, int, T1, SpawnJSObjectReference>("propertyNew", Id, key, arg1);
        public SpawnJSObjectReference New<T1, T2>(int key, T1 arg1, T2 arg2)
            => JS.InteropCall<double, int, T1, T2, SpawnJSObjectReference>("propertyNew", Id, key, arg1, arg2);
        public SpawnJSObjectReference New<T1, T2, T3>(int key, T1 arg1, T2 arg2, T3 arg3)
            => JS.InteropCall<double, int, T1, T2, T3, SpawnJSObjectReference>("propertyNew", Id, key, arg1, arg2, arg3);
        public SpawnJSObjectReference New<T1, T2, T3, T4>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            => JS.InteropCall<double, int, T1, T2, T3, T4, SpawnJSObjectReference>("propertyNew", Id, key, arg1, arg2, arg3, arg4);
        public SpawnJSObjectReference New<T1, T2, T3, T4, T5>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            => JS.InteropCall<double, int, T1, T2, T3, T4, T5, SpawnJSObjectReference>("propertyNew", Id, key, arg1, arg2, arg3, arg4, arg5);
        public SpawnJSObjectReference New<T1, T2, T3, T4, T5, T6>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
            => JS.InteropCall<double, int, T1, T2, T3, T4, T5, T6, SpawnJSObjectReference>("propertyNew", Id, key, arg1, arg2, arg3, arg4, arg5, arg6);
        public SpawnJSObjectReference New<T1, T2, T3, T4, T5, T6, T7>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
            => JS.InteropCall<double, int, T1, T2, T3, T4, T5, T6, T7, SpawnJSObjectReference>("propertyNew", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        public SpawnJSObjectReference New<T1, T2, T3, T4, T5, T6, T7, T8>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
            => JS.InteropCall<double, int, T1, T2, T3, T4, T5, T6, T7, T8, SpawnJSObjectReference>("propertyNew", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        public SpawnJSObjectReference New<T1, T2, T3, T4, T5, T6, T7, T8, T9>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
            => JS.InteropCall<double, int, T1, T2, T3, T4, T5, T6, T7, T8, T9, SpawnJSObjectReference>("propertyNew", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        public SpawnJSObjectReference New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
            => JS.InteropCall<double, int, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, SpawnJSObjectReference>("propertyNew", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        #endregion
        #region Call
        public void CallApplyVoid(int key, object?[]? args = null) => JS.InteropCall<double, int, object?[]?, VoidType>("propertyCallApply", Id, key, args);
        public T CallApply<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, object?[]? args = null) => JS.InteropCall<double, int, object?[]?, T>("propertyCallApply", Id, key, args);
        // CallVoid
        public void CallVoid(int key)
            => JS.InteropCall<double, int, VoidType>("propertyCall", Id, key);
        public void CallVoid<T1>(int key, T1 arg1)
            => JS.InteropCall<double, int, T1, VoidType>("propertyCall", Id, key, arg1);
        public void CallVoid<T1, T2>(int key, T1 arg1, T2 arg2)
            => JS.InteropCall<double, int, T1, T2, VoidType>("propertyCall", Id, key, arg1, arg2);
        public void CallVoid<T1, T2, T3>(int key, T1 arg1, T2 arg2, T3 arg3)
            => JS.InteropCall<double, int, T1, T2, T3, VoidType>("propertyCall", Id, key, arg1, arg2, arg3);
        public void CallVoid<T1, T2, T3, T4>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            => JS.InteropCall<double, int, T1, T2, T3, T4, VoidType>("propertyCall", Id, key, arg1, arg2, arg3, arg4);
        public void CallVoid<T1, T2, T3, T4, T5>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            => JS.InteropCall<double, int, T1, T2, T3, T4, T5, VoidType>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5);
        public void CallVoid<T1, T2, T3, T4, T5, T6>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
            => JS.InteropCall<double, int, T1, T2, T3, T4, T5, T6, VoidType>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6);
        public void CallVoid<T1, T2, T3, T4, T5, T6, T7>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
            => JS.InteropCall<double, int, T1, T2, T3, T4, T5, T6, T7, VoidType>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        public void CallVoid<T1, T2, T3, T4, T5, T6, T7, T8>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
            => JS.InteropCall<double, int, T1, T2, T3, T4, T5, T6, T7, T8, VoidType>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        public void CallVoid<T1, T2, T3, T4, T5, T6, T7, T8, T9>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
            => JS.InteropCall<double, int, T1, T2, T3, T4, T5, T6, T7, T8, T9, VoidType>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        public void CallVoid<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
            => JS.InteropCall<double, int, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, VoidType>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        // Call
        public T Call<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key)
            => JS.InteropCall<double, int, T>("propertyCall", Id, key);
        public T Call<T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, T1 arg1)
            => JS.InteropCall<double, int, T1, T>("propertyCall", Id, key, arg1);
        public T Call<T1, T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, T1 arg1, T2 arg2)
            => JS.InteropCall<double, int, T1, T2, T>("propertyCall", Id, key, arg1, arg2);
        public T Call<T1, T2, T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, T1 arg1, T2 arg2, T3 arg3)
            => JS.InteropCall<double, int, T1, T2, T3, T>("propertyCall", Id, key, arg1, arg2, arg3);
        public T Call<T1, T2, T3, T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            => JS.InteropCall<double, int, T1, T2, T3, T4, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4);
        public T Call<T1, T2, T3, T4, T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            => JS.InteropCall<double, int, T1, T2, T3, T4, T5, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5);
        public T Call<T1, T2, T3, T4, T5, T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
            => JS.InteropCall<double, int, T1, T2, T3, T4, T5, T6, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6);
        public T Call<T1, T2, T3, T4, T5, T6, T7, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
            => JS.InteropCall<double, int, T1, T2, T3, T4, T5, T6, T7, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        public T Call<T1, T2, T3, T4, T5, T6, T7, T8, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
            => JS.InteropCall<double, int, T1, T2, T3, T4, T5, T6, T7, T8, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        public T Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
            => JS.InteropCall<double, int, T1, T2, T3, T4, T5, T6, T7, T8, T9, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        public T Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
            => JS.InteropCall<double, int, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        #endregion
        #region CallAsync
        public Task CallApplyVoidAsync(int key, object?[]? args = null) => JS.InteropCallAsync<double, int, object?[]?, VoidType>("propertyCallApply", Id, key, args);
        public Task<T> CallApplyAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, object?[]? args = null) => JS.InteropCallAsync<double, int, object?[]?, T>("propertyCallApply", Id, key, args);
        // CallVoidAsync
        public Task CallVoidAsync(int key)
            => JS.InteropCallAsync<double, int, VoidType>("propertyCall", Id, key);
        public Task CallVoidAsync<T1>(int key, T1 arg1)
            => JS.InteropCallAsync<double, int, T1, VoidType>("propertyCall", Id, key, arg1);
        public Task CallVoidAsync<T1, T2>(int key, T1 arg1, T2 arg2)
            => JS.InteropCallAsync<double, int, T1, T2, VoidType>("propertyCall", Id, key, arg1, arg2);
        public Task CallVoidAsync<T1, T2, T3>(int key, T1 arg1, T2 arg2, T3 arg3)
            => JS.InteropCallAsync<double, int, T1, T2, T3, VoidType>("propertyCall", Id, key, arg1, arg2, arg3);
        public Task CallVoidAsync<T1, T2, T3, T4>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            => JS.InteropCallAsync<double, int, T1, T2, T3, T4, VoidType>("propertyCall", Id, key, arg1, arg2, arg3, arg4);
        public Task CallVoidAsync<T1, T2, T3, T4, T5>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            => JS.InteropCallAsync<double, int, T1, T2, T3, T4, T5, VoidType>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5);
        public Task CallVoidAsync<T1, T2, T3, T4, T5, T6>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
            => JS.InteropCallAsync<double, int, T1, T2, T3, T4, T5, T6, VoidType>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6);
        public Task CallVoidAsync<T1, T2, T3, T4, T5, T6, T7>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
            => JS.InteropCallAsync<double, int, T1, T2, T3, T4, T5, T6, T7, VoidType>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        public Task CallVoidAsync<T1, T2, T3, T4, T5, T6, T7, T8>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
            => JS.InteropCallAsync<double, int, T1, T2, T3, T4, T5, T6, T7, T8, VoidType>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        public Task CallVoidAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
            => JS.InteropCallAsync<double, int, T1, T2, T3, T4, T5, T6, T7, T8, T9, VoidType>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        public Task CallVoidAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
            => JS.InteropCallAsync<double, int, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, VoidType>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        // CallAsync
        public Task<T> CallAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key)
            => JS.InteropCallAsync<double, int, T>("propertyCall", Id, key);
        public Task<T> CallAsync<T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, T1 arg1)
            => JS.InteropCallAsync<double, int, T1, T>("propertyCall", Id, key, arg1);
        public Task<T> CallAsync<T1, T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, T1 arg1, T2 arg2)
            => JS.InteropCallAsync<double, int, T1, T2, T>("propertyCall", Id, key, arg1, arg2);
        public Task<T> CallAsync<T1, T2, T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, T1 arg1, T2 arg2, T3 arg3)
            => JS.InteropCallAsync<double, int, T1, T2, T3, T>("propertyCall", Id, key, arg1, arg2, arg3);
        public Task<T> CallAsync<T1, T2, T3, T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            => JS.InteropCallAsync<double, int, T1, T2, T3, T4, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4);
        public Task<T> CallAsync<T1, T2, T3, T4, T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            => JS.InteropCallAsync<double, int, T1, T2, T3, T4, T5, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5);
        public Task<T> CallAsync<T1, T2, T3, T4, T5, T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
            => JS.InteropCallAsync<double, int, T1, T2, T3, T4, T5, T6, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6);
        public Task<T> CallAsync<T1, T2, T3, T4, T5, T6, T7, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
            => JS.InteropCallAsync<double, int, T1, T2, T3, T4, T5, T6, T7, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        public Task<T> CallAsync<T1, T2, T3, T4, T5, T6, T7, T8, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
            => JS.InteropCallAsync<double, int, T1, T2, T3, T4, T5, T6, T7, T8, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        public Task<T> CallAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
            => JS.InteropCallAsync<double, int, T1, T2, T3, T4, T5, T6, T7, T8, T9, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        public Task<T> CallAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
            => JS.InteropCallAsync<double, int, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        #endregion
    }
}
