// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Options for CredentialsContainer.Get()<br/>
    /// CredentialsContainer.Get() will return a Promise that resolves with an OTPCredential object instance.
    /// https://developer.mozilla.org/en-US/docs/Web/API/CredentialsContainer/get#otp_object_structure
    /// </summary>
    public class CredentialGetOtp
    {
        /// <summary>
        /// An array of strings representing transport hints for how the OTP should ideally be transmitted. This will always contain a single hint — "sms". Unknown values will be ignored.
        /// </summary>
        public IEnumerable<string> Transport { get; set; }
    }
}
