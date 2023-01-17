using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpesaDaraja.Models
{
    public class PushResult
    {
        /// <summary>
        ///     This is a global unique Identifier for any submitted payment request.
        /// </summary>
        [JsonProperty("MerchantRequestID")]
        public string? MerchantRequestId { get; set; }

        /// <summary>
        ///     This is a global unique identifier of the processed checkout transaction request.
        /// </summary>
        [JsonProperty("CheckoutRequestID")]
        public string? CheckoutRequestId { get; set; }

        /// <summary>
        ///     This is a Numeric status code that indicates the status of the transaction submission. 0 means successful submission and any other code means an error occurred. 
        /// </summary>
        [JsonProperty("ResponseCode")]
        public long ResponseCode { get; set; }

        /// <summary>
        ///     Response description is an acknowledgment message from the API that gives the status of the request submission
        ///     usually maps to a specific ResponseCode value. It can be a Success submission message or an error description.
        /// </summary>
        [JsonProperty("ResponseDescription")]
        public string? ResponseDescription { get; set; }

        /// <summary>
        ///     This is a message that your system can display to the Customer as an acknowledgement of the payment request submission.
        /// </summary>
        [JsonProperty("CustomerMessage")]
        public string? CustomerMessage { get; set; }
    }
}
