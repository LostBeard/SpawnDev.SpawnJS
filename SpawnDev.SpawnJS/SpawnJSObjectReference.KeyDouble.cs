using System.Diagnostics.CodeAnalysis;
using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS
{
    public partial class SpawnJSObjectReference
    {
        public string ConstructorName(double key) => TypeInfo(key).ConstructorName;
        public string TypeOf(double key) => TypeInfo(key).TypeOf;
        public (string TypeOf, string ConstructorName) TypeInfo(double key)
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
        /// <summary>
        /// Check if a property exists at key
        /// </summary>
        public bool Has(double key) => SpawnJSRuntime._propertyIn(Id, key);
        /// <summary>
        /// Delete the property at key
        /// </summary>
        public bool Delete(double key) => SpawnJSRuntime._propertyDelete(Id, key);
        #region Set
        public void Set<T>(double key, T value) => JS.InteropCall<double, double, T, VoidType>(InteropMethod.PropertySet, Id, key, value);
        #endregion
        #region Get
        public SpawnJSObjectReference? Get(double key) => JS.InteropCall<double, double, SpawnJSObjectReference>(InteropMethod.PropertyGet, Id, key);
        public T Get<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key) => JS.InteropCall<double, double, T>(InteropMethod.PropertyGet, Id, key);
        #endregion
        #region GetAsync
        public Task<T> GetAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key) => JS.InteropCallAsync<double, double, T>(InteropMethod.PropertyGet, Id, key);
        public Task<SpawnJSObjectReference> GetAsync(double key) => JS.InteropCallAsync<double, double, SpawnJSObjectReference>(InteropMethod.PropertyGet, Id, key);
        #endregion
        #region New
        public T NewApply<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, object?[]? args = null) 
            => JS.InteropCall<double, double, object?[]?, T>(InteropMethod.PropertyNewApply, Id, key, args);        
        public T New<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key)
            => JS.InteropCall<double, double, T>(InteropMethod.PropertyNew, Id, key);
        public T New<T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, T1 arg1)
            => JS.InteropCall<double, double, T1, T>(InteropMethod.PropertyNew, Id, key, arg1);
        public T New<T1, T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, T1 arg1, T2 arg2)
            => JS.InteropCall<double, double, T1, T2, T>(InteropMethod.PropertyNew, Id, key, arg1, arg2);
        public T New<T1, T2, T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, T1 arg1, T2 arg2, T3 arg3)
            => JS.InteropCall<double, double, T1, T2, T3, T>(InteropMethod.PropertyNew, Id, key, arg1, arg2, arg3);
        public T New<T1, T2, T3, T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            => JS.InteropCall<double, double, T1, T2, T3, T4, T>(InteropMethod.PropertyNew, Id, key, arg1, arg2, arg3, arg4);
        public T New<T1, T2, T3, T4, T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            => JS.InteropCall<double, double, T1, T2, T3, T4, T5, T>(InteropMethod.PropertyNew, Id, key, arg1, arg2, arg3, arg4, arg5);
        public T New<T1, T2, T3, T4, T5, T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
            => JS.InteropCall<double, double, T1, T2, T3, T4, T5, T6, T>(InteropMethod.PropertyNew, Id, key, arg1, arg2, arg3, arg4, arg5, arg6);
        public T New<T1, T2, T3, T4, T5, T6, T7, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
            => JS.InteropCall<double, double, T1, T2, T3, T4, T5, T6, T7, T>(InteropMethod.PropertyNew, Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        public T New<T1, T2, T3, T4, T5, T6, T7, T8, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
            => JS.InteropCall<double, double, T1, T2, T3, T4, T5, T6, T7, T8, T>(InteropMethod.PropertyNew, Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        public T New<T1, T2, T3, T4, T5, T6, T7, T8, T9, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
            => JS.InteropCall<double, double, T1, T2, T3, T4, T5, T6, T7, T8, T9, T>(InteropMethod.PropertyNew, Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        public T New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
            => JS.InteropCall<double, double, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T>(InteropMethod.PropertyNew, Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);

        public SpawnJSObjectReference NewApply(double key, object?[]? args = null) 
            => JS.InteropCall<double, double, object?[]?, SpawnJSObjectReference>(InteropMethod.PropertyNewApply, Id, key, args);
        public SpawnJSObjectReference New(double key) 
            => JS.InteropCall<double, double, SpawnJSObjectReference>(InteropMethod.PropertyNew, Id, key);
        public SpawnJSObjectReference New<T1>(double key, T1 arg1)
            => JS.InteropCall<double, double, T1, SpawnJSObjectReference>(InteropMethod.PropertyNew, Id, key, arg1);
        public SpawnJSObjectReference New<T1, T2>(double key, T1 arg1, T2 arg2)
            => JS.InteropCall<double, double, T1, T2, SpawnJSObjectReference>(InteropMethod.PropertyNew, Id, key, arg1, arg2);
        public SpawnJSObjectReference New<T1, T2, T3>(double key, T1 arg1, T2 arg2, T3 arg3)
            => JS.InteropCall<double, double, T1, T2, T3, SpawnJSObjectReference>(InteropMethod.PropertyNew, Id, key, arg1, arg2, arg3);
        public SpawnJSObjectReference New<T1, T2, T3, T4>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            => JS.InteropCall<double, double, T1, T2, T3, T4, SpawnJSObjectReference>(InteropMethod.PropertyNew, Id, key, arg1, arg2, arg3, arg4);
        public SpawnJSObjectReference New<T1, T2, T3, T4, T5>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            => JS.InteropCall<double, double, T1, T2, T3, T4, T5, SpawnJSObjectReference>(InteropMethod.PropertyNew, Id, key, arg1, arg2, arg3, arg4, arg5);
        public SpawnJSObjectReference New<T1, T2, T3, T4, T5, T6>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
            => JS.InteropCall<double, double, T1, T2, T3, T4, T5, T6, SpawnJSObjectReference>(InteropMethod.PropertyNew, Id, key, arg1, arg2, arg3, arg4, arg5, arg6);
        public SpawnJSObjectReference New<T1, T2, T3, T4, T5, T6, T7>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
            => JS.InteropCall<double, double, T1, T2, T3, T4, T5, T6, T7, SpawnJSObjectReference>(InteropMethod.PropertyNew, Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        public SpawnJSObjectReference New<T1, T2, T3, T4, T5, T6, T7, T8>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
            => JS.InteropCall<double, double, T1, T2, T3, T4, T5, T6, T7, T8, SpawnJSObjectReference>(InteropMethod.PropertyNew, Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        public SpawnJSObjectReference New<T1, T2, T3, T4, T5, T6, T7, T8, T9>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
            => JS.InteropCall<double, double, T1, T2, T3, T4, T5, T6, T7, T8, T9, SpawnJSObjectReference>(InteropMethod.PropertyNew, Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        public SpawnJSObjectReference New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
            => JS.InteropCall<double, double, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, SpawnJSObjectReference>(InteropMethod.PropertyNew, Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        #endregion
        #region Call
        public void CallApplyVoid(double key, object?[]? args = null) => JS.InteropCall<double, double, object?[]?, VoidType>(InteropMethod.PropertyCallApply, Id, key, args);
        public T CallApply<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, object?[]? args = null) => JS.InteropCall<double, double, object?[]?, T>(InteropMethod.PropertyCallApply, Id, key, args);
        // CallVoid
        public void CallVoid(double key)
            => JS.InteropCall<double, double, VoidType>(InteropMethod.PropertyCall, Id, key);
        public void CallVoid<T1>(double key, T1 arg1)
            => JS.InteropCall<double, double, T1, VoidType>(InteropMethod.PropertyCall, Id, key, arg1);
        public void CallVoid<T1, T2>(double key, T1 arg1, T2 arg2)
            => JS.InteropCall<double, double, T1, T2, VoidType>(InteropMethod.PropertyCall, Id, key, arg1, arg2);
        public void CallVoid<T1, T2, T3>(double key, T1 arg1, T2 arg2, T3 arg3)
            => JS.InteropCall<double, double, T1, T2, T3, VoidType>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3);
        public void CallVoid<T1, T2, T3, T4>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            => JS.InteropCall<double, double, T1, T2, T3, T4, VoidType>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3, arg4);
        public void CallVoid<T1, T2, T3, T4, T5>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            => JS.InteropCall<double, double, T1, T2, T3, T4, T5, VoidType>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3, arg4, arg5);
        public void CallVoid<T1, T2, T3, T4, T5, T6>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
            => JS.InteropCall<double, double, T1, T2, T3, T4, T5, T6, VoidType>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3, arg4, arg5, arg6);
        public void CallVoid<T1, T2, T3, T4, T5, T6, T7>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
            => JS.InteropCall<double, double, T1, T2, T3, T4, T5, T6, T7, VoidType>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        public void CallVoid<T1, T2, T3, T4, T5, T6, T7, T8>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
            => JS.InteropCall<double, double, T1, T2, T3, T4, T5, T6, T7, T8, VoidType>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        public void CallVoid<T1, T2, T3, T4, T5, T6, T7, T8, T9>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
            => JS.InteropCall<double, double, T1, T2, T3, T4, T5, T6, T7, T8, T9, VoidType>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        public void CallVoid<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
            => JS.InteropCall<double, double, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, VoidType>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        // Call
        public T Call<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key)
            => JS.InteropCall<double, double, T>(InteropMethod.PropertyCall, Id, key);
        public T Call<T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, T1 arg1)
            => JS.InteropCall<double, double, T1, T>(InteropMethod.PropertyCall, Id, key, arg1);
        public T Call<T1, T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, T1 arg1, T2 arg2)
            => JS.InteropCall<double, double, T1, T2, T>(InteropMethod.PropertyCall, Id, key, arg1, arg2);
        public T Call<T1, T2, T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, T1 arg1, T2 arg2, T3 arg3)
            => JS.InteropCall<double, double, T1, T2, T3, T>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3);
        public T Call<T1, T2, T3, T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            => JS.InteropCall<double, double, T1, T2, T3, T4, T>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3, arg4);
        public T Call<T1, T2, T3, T4, T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            => JS.InteropCall<double, double, T1, T2, T3, T4, T5, T>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3, arg4, arg5);
        public T Call<T1, T2, T3, T4, T5, T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
            => JS.InteropCall<double, double, T1, T2, T3, T4, T5, T6, T>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3, arg4, arg5, arg6);
        public T Call<T1, T2, T3, T4, T5, T6, T7, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
            => JS.InteropCall<double, double, T1, T2, T3, T4, T5, T6, T7, T>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        public T Call<T1, T2, T3, T4, T5, T6, T7, T8, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
            => JS.InteropCall<double, double, T1, T2, T3, T4, T5, T6, T7, T8, T>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        public T Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
            => JS.InteropCall<double, double, T1, T2, T3, T4, T5, T6, T7, T8, T9, T>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        public T Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
            => JS.InteropCall<double, double, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        #endregion
        #region CallAsync
        public Task CallApplyVoidAsync(double key, object?[]? args = null) => JS.InteropCallAsync<double, double, object?[]?, VoidType>(InteropMethod.PropertyCallApply, Id, key, args);
        public Task<T> CallApplyAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, object?[]? args = null) => JS.InteropCallAsync<double, double, object?[]?, T>(InteropMethod.PropertyCallApply, Id, key, args);
        // CallVoidAsync
        public Task CallVoidAsync(double key)
            => JS.InteropCallAsync<double, double, VoidType>(InteropMethod.PropertyCall, Id, key);
        public Task CallVoidAsync<T1>(double key, T1 arg1)
            => JS.InteropCallAsync<double, double, T1, VoidType>(InteropMethod.PropertyCall, Id, key, arg1);
        public Task CallVoidAsync<T1, T2>(double key, T1 arg1, T2 arg2)
            => JS.InteropCallAsync<double, double, T1, T2, VoidType>(InteropMethod.PropertyCall, Id, key, arg1, arg2);
        public Task CallVoidAsync<T1, T2, T3>(double key, T1 arg1, T2 arg2, T3 arg3)
            => JS.InteropCallAsync<double, double, T1, T2, T3, VoidType>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3);
        public Task CallVoidAsync<T1, T2, T3, T4>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            => JS.InteropCallAsync<double, double, T1, T2, T3, T4, VoidType>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3, arg4);
        public Task CallVoidAsync<T1, T2, T3, T4, T5>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            => JS.InteropCallAsync<double, double, T1, T2, T3, T4, T5, VoidType>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3, arg4, arg5);
        public Task CallVoidAsync<T1, T2, T3, T4, T5, T6>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
            => JS.InteropCallAsync<double, double, T1, T2, T3, T4, T5, T6, VoidType>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3, arg4, arg5, arg6);
        public Task CallVoidAsync<T1, T2, T3, T4, T5, T6, T7>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
            => JS.InteropCallAsync<double, double, T1, T2, T3, T4, T5, T6, T7, VoidType>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        public Task CallVoidAsync<T1, T2, T3, T4, T5, T6, T7, T8>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
            => JS.InteropCallAsync<double, double, T1, T2, T3, T4, T5, T6, T7, T8, VoidType>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        public Task CallVoidAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
            => JS.InteropCallAsync<double, double, T1, T2, T3, T4, T5, T6, T7, T8, T9, VoidType>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        public Task CallVoidAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
            => JS.InteropCallAsync<double, double, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, VoidType>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        // CallAsync
        public Task<T> CallAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key)
            => JS.InteropCallAsync<double, double, T>(InteropMethod.PropertyCall, Id, key);
        public Task<T> CallAsync<T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, T1 arg1)
            => JS.InteropCallAsync<double, double, T1, T>(InteropMethod.PropertyCall, Id, key, arg1);
        public Task<T> CallAsync<T1, T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, T1 arg1, T2 arg2)
            => JS.InteropCallAsync<double, double, T1, T2, T>(InteropMethod.PropertyCall, Id, key, arg1, arg2);
        public Task<T> CallAsync<T1, T2, T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, T1 arg1, T2 arg2, T3 arg3)
            => JS.InteropCallAsync<double, double, T1, T2, T3, T>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3);
        public Task<T> CallAsync<T1, T2, T3, T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            => JS.InteropCallAsync<double, double, T1, T2, T3, T4, T>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3, arg4);
        public Task<T> CallAsync<T1, T2, T3, T4, T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            => JS.InteropCallAsync<double, double, T1, T2, T3, T4, T5, T>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3, arg4, arg5);
        public Task<T> CallAsync<T1, T2, T3, T4, T5, T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
            => JS.InteropCallAsync<double, double, T1, T2, T3, T4, T5, T6, T>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3, arg4, arg5, arg6);
        public Task<T> CallAsync<T1, T2, T3, T4, T5, T6, T7, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
            => JS.InteropCallAsync<double, double, T1, T2, T3, T4, T5, T6, T7, T>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        public Task<T> CallAsync<T1, T2, T3, T4, T5, T6, T7, T8, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
            => JS.InteropCallAsync<double, double, T1, T2, T3, T4, T5, T6, T7, T8, T>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        public Task<T> CallAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
            => JS.InteropCallAsync<double, double, T1, T2, T3, T4, T5, T6, T7, T8, T9, T>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        public Task<T> CallAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
            => JS.InteropCallAsync<double, double, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T>(InteropMethod.PropertyCall, Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        #endregion
    }
}
