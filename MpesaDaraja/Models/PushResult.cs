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
        [JsonProperty("MerchantRequestID")]
        public string MerchantRequestId { get; set; }

        [JsonProperty("CheckoutRequestID")]
        public string CheckoutRequestId { get; set; }

        [JsonProperty("ResponseCode")]
        public long ResponseCode { get; set; }

        [JsonProperty("ResponseDescription")]
        public string ResponseDescription { get; set; }

        [JsonProperty("CustomerMessage")]
        public string CustomerMessage { get; set; }
    }
}
