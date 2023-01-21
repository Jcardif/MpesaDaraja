using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpesaDaraja.Models
{
    /// <summary>
    ///     Error from daraja API when request is unsuccessful 
    /// </summary>
    public class DarajaError
    {
        /// <summary>
        ///     This is a unique requestID for the payment request
        /// </summary>
        [JsonProperty("requestId")]
        public string? RequestId { get; set; }

        /// <summary>
        ///     This is a predefined code that indicates the reason for request failure. This are defined in the Response Error Details below.
        ///     The error codes maps to specific error message as illustrated in the Response Error Details below.
        /// </summary>
        [JsonProperty("errorCode")]
        public string? ErrorCode { get; set; }

        /// <summary>
        ///     This is a short descriptive message of the failure reason.
        /// </summary>
        [JsonProperty("errorMessage")]
        public string? ErrorMessage { get; set; }
    }
}
