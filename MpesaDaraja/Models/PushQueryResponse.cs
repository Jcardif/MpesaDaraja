using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpesaDaraja.Models
{
    /// <summary>
    ///     Response post querying status of an M-Pesa Online Payment.
    /// </summary>
    public class PushQueryResponse
    {
        [JsonProperty("ResponseCode")]
        public string? ResponseCode { get; set; }

        [JsonProperty("ResponseDescription")]
        public string? ResponseDescription { get; set; }

        [JsonProperty("MerchantRequestID")]
        public string? MerchantRequestId { get; set; }

        [JsonProperty("CheckoutRequestID")]
        public string? CheckoutRequestId { get; set; }

        [JsonProperty("ResultCode")]
        public string? ResultCode { get; set; }

        [JsonProperty("ResultDesc")]
        public string? ResultDesc { get; set; }
    }
}
