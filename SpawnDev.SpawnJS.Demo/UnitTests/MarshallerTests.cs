using SpawnDev.SpawnJS.Marshallers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace SpawnDev.SpawnJS.Demo.UnitTests
{
    public static class MarshallerTests
    {
        static SpawnJSRuntime JS => SpawnJSRuntime.Instance;
        static readonly string _key = $"{nameof(MarshallerTests)}_1";
        static void ValueTypeGetSetCheck<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(T value)
        {
            var marshaller = JS.GetMarshaller<T>();
            JS.Set(_key, value);
            var mismatch = false;
            try
            {
                var valueReadBack = JS.Get<T>(_key);
                var match = valueReadBack?.Equals(value) == true;
                if (!match)
                {
                    mismatch = true;
                    throw new Exception($"FAILED Marshaller: {marshaller.GetType().Name} readback did not match");
                }
            }
            catch(Exception ex) 
            {
                if (mismatch) throw;
                throw new Exception($"FAILED Marshaller: {marshaller.GetType().Name} {ex.ToString()}");
            }
            finally
            {
                JS.Delete(_key);
            }
        }

        public static async Task Run()
        {
            // test the value type marshallers
            ValueTypeGetSetCheck("test");

            ValueTypeGetSetCheck((byte)42);
            ValueTypeGetSetCheck((ushort)42);
            ValueTypeGetSetCheck((uint)42);
            ValueTypeGetSetCheck((ulong)42);

            ValueTypeGetSetCheck((sbyte)42);
            ValueTypeGetSetCheck((short)42);
            ValueTypeGetSetCheck((int)42);
            ValueTypeGetSetCheck((long)42);

            ValueTypeGetSetCheck((Half)42);
            ValueTypeGetSetCheck((float)42);
            ValueTypeGetSetCheck((double)42);


            ValueTypeGetSetCheck((byte?)42);
            ValueTypeGetSetCheck((ushort?)42);
            ValueTypeGetSetCheck((uint?)42);
            ValueTypeGetSetCheck((ulong?)42);

            ValueTypeGetSetCheck((sbyte?)42);
            ValueTypeGetSetCheck((short?)42);
            ValueTypeGetSetCheck((int?)42);
            ValueTypeGetSetCheck((long?)42);

            ValueTypeGetSetCheck((Half?)42);
            ValueTypeGetSetCheck((float?)42);
            ValueTypeGetSetCheck((double?)42);

        }
    }
}
