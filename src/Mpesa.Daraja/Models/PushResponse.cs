using System.Text.Json.Serialization;

namespace Mpesa.Daraja.Models
{
    /// <summary>
    ///     Result after initiating an stk push
    /// </summary>
    public class PushResponse
    {
        /// <summary>
        ///     This is a global unique Identifier for any submitted payment request.
        /// </summary>
        [JsonPropertyName("MerchantRequestID")]
        public string? MerchantRequestId { get; set; }

        /// <summary>
        ///     This is a global unique identifier of the processed checkout transaction request.
        /// </summary>
        [JsonPropertyName("CheckoutRequestID")]
        public string? CheckoutRequestId { get; set; }

        /// <summary>
        ///     This is a Numeric status code that indicates the status of the transaction submission. 0 means successful submission and any other code means an error occurred.
        /// </summary>
        [JsonPropertyName("ResponseCode")]
        public long ResponseCode { get; set; }

        /// <summary>
        ///     Response description is an acknowledgment message from the API that gives the status of the request submission
        ///     usually maps to a specific ResponseCode value. It can be a Success submission message or an error description.
        /// </summary>
        [JsonPropertyName("ResponseDescription")]
        public string? ResponseDescription { get; set; }

        /// <summary>
        ///     This is a message that your system can display to the Customer as an acknowledgement of the payment request submission.
        /// </summary>
        [JsonPropertyName("CustomerMessage")]
        public string? CustomerMessage { get; set; }
    }
}
