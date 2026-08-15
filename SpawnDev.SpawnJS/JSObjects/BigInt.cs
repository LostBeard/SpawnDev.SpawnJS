using System.Numerics;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// BigInt values represent numeric values which are too large to be represented by the number primitive.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/BigInt
    /// </summary>
    public class BigInt : SpawnJSObject
    {
        /// <inheritdoc/>
        public BigInt(BigInteger value) : base(JS.Call<string, SpawnJSObjectReference>("BigInt", value.ToString())) { }
        /// <inheritdoc/>
        public BigInt(SpawnJSObjectReference jsRef) : base(jsRef) { }
        /// <summary>
        /// Return the BigInt as a BigInteger
        /// </summary>
        /// <returns></returns>
        public BigInteger ToBigInteger() => JSRef!.As<BigInteger>();
    }
}
