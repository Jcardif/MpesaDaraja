using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpesaDaraja.Models
{
    public class StkData
    {
        [JsonProperty("BusinessShortCode")]
        public long BusinessShortCode { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }

        [JsonProperty("Timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("TransactionType")]
        public string TransactionType { get; set; }

        [JsonProperty("Amount")]
        public long Amount { get; set; }

        [JsonProperty("PartyA")]
        public long PartyA { get; set; }

        [JsonProperty("PartyB")]
        public long PartyB { get; set; }

        [JsonProperty("PhoneNumber")]
        public long PhoneNumber { get; set; }

        [JsonProperty("CallBackURL")]
        public Uri CallBackUrl { get; set; }

        [JsonProperty("AccountReference")]
        public string AccountReference { get; set; }

        [JsonProperty("TransactionDesc")]
        public string TransactionDesc { get; set; }
    }
}
