// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The PaymentResponse interface of the Payment Request API is returned after a user selects a payment method and approves a payment request.
    /// </summary>
    public class PaymentResponse : EventTarget
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        public PaymentResponse(SpawnJSObjectReference _ref) : base(_ref) { }

        /// <summary>
        /// Returns the payment request ID.
        /// </summary>
        public string RequestId => JSRef!.Get<string>("requestId");

        /// <summary>
        /// Returns the payment method identifier.
        /// </summary>
        public string MethodName => JSRef!.Get<string>("methodName");

        /// <summary>
        /// Returns a JSON-serializable object that provides a payment method specific message used by the merchant to process the transaction and determine successful fund transfer.
        /// </summary>
        public T Details<T>() => JSRef!.Get<T>("details");

        /// <summary>
        /// Returns a JSON-serializable object that provides a payment method specific message used by the merchant to process the transaction and determine successful fund transfer.
        /// </summary>
        /// <summary>
        /// Returns a JSON-serializable object that provides a payment method specific message used by the merchant to process the transaction and determine successful fund transfer.
        /// </summary>
        public SpawnJSObject Details() => JSRef!.Get<SpawnJSObject>("details");

        /// <summary>
        /// Returns the shipping address chosen by the user.
        /// </summary>
        public PaymentAddress? ShippingAddress => JSRef!.Get<PaymentAddress?>("shippingAddress");

        /// <summary>
        /// Returns the shipping option chosen by the user.
        /// </summary>
        public string? ShippingOption => JSRef!.Get<string?>("shippingOption");

        /// <summary>
        /// Returns the payer's name.
        /// </summary>
        public string? PayerName => JSRef!.Get<string?>("payerName");

        /// <summary>
        /// Returns the payer's email.
        /// </summary>
        public string? PayerEmail => JSRef!.Get<string?>("payerEmail");

        /// <summary>
        /// Returns the payer's phone.
        /// </summary>
        public string? PayerPhone => JSRef!.Get<string?>("payerPhone");

        /// <summary>
        /// Signals that the transaction has completed.
        /// </summary>
        public Task Complete(string result = "unknown") => JSRef!.CallVoidAsync("complete", result);

        /// <summary>
        /// Signals that the user has retried the payment.
        /// </summary>
        public Task Retry(PaymentValidationErrors errorFields) => JSRef!.CallVoidAsync("retry", errorFields);
    }
}
