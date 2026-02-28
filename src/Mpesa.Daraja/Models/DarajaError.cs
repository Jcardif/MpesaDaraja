using System.Text.Json.Serialization;

namespace Mpesa.Daraja.Models
{
    /// <summary>
    ///     Error from daraja API when request is unsuccessful
    /// </summary>
    public class DarajaError
    {
        /// <summary>
        ///     This is a unique requestID for the payment request
        /// </summary>
        [JsonPropertyName("requestId")]
        public string? RequestId { get; set; }

        /// <summary>
        ///     This is a predefined code that indicates the reason for request failure. This are defined in the Response Error Details below.
        ///     The error codes maps to specific error message as illustrated in the Response Error Details below.
        /// </summary>
        [JsonPropertyName("errorCode")]
        public string? ErrorCode { get; set; }

        /// <summary>
        ///     This is a short descriptive message of the failure reason.
        /// </summary>
        [JsonPropertyName("errorMessage")]
        public string? ErrorMessage { get; set; }
    }
}
