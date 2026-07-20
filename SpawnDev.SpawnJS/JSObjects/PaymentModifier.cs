using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://developer.mozilla.org/en-US/docs/Web/API/PaymentRequestEvent/modifiers#value
    /// </summary>
    public class PaymentModifier : SpawnJSObject
    {
        /// <inheritdoc/>
        public PaymentModifier(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public PaymentModifier() : base(JS.New("Object")) { }
        /// <summary>
        /// A payment method identifier. The members of the object only apply to the payment if the user selects this payment method.
        /// </summary>
        public string? SupportedMethods { get => JSRef!.Get<string?>("supportedMethods"); set => JSRef!.Set("supportedMethods", value); }
        /// <summary>
        /// A PaymentItem object
        /// </summary>
        public PaymentItem? Total { get => JSRef!.Get<PaymentItem?>("total"); set => JSRef!.Set("total", value); }
    }
}
