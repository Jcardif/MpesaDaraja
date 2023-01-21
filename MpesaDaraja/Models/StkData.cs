using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpesaDaraja.Models
{
    /// <summary>
    ///     Defines the class for initiating online payment on behalf of a customer.
    /// </summary>
    public class StkData
    {
        /// <summary>
        ///     This is organizations short code (Paybill or Buygoods - A 5 to 7 digit account number) used to identify an organization and receive the transaction.
        /// </summary>
        [JsonProperty("BusinessShortCode")]
        public long BusinessShortCode { get; set; }

        /// <summary>
        ///     This is the password used for encrypting the request sent: A base64 encoded string. (The base64 string is a combination of Shortcode+Passkey+Timestamp)
        /// </summary>
        [JsonProperty("Password")]
        public string? Password { get; set; }

        /// <summary>
        ///     This is the Timestamp of the transaction, normally in the format of YEAR+MONTH+DATE+HOUR+MINUTE+SECOND (yyyyMMddHHmmss)
        ///     Each part should be at least two digits apart from the year which takes four digits.
        /// </summary>
        [JsonProperty("Timestamp")]
        public string? Timestamp { get; set; }

        /// <summary>
        ///     This is the transaction type that is used to identify the transaction when sending the request to M-Pesa. The transaction type for M-Pesa Express (stk push) is "CustomerPayBillOnline" 
        /// </summary>
        [JsonProperty("TransactionType")]
        public string? TransactionType { get; set; }

        /// <summary>
        ///     This is the Amount transacted normally a numeric value. Money that customer pays to the shortcode. Only whole numbers are supported.
        /// </summary>
        [JsonProperty("Amount")]
        public long Amount { get; set; }

        /// <summary>
        ///     The phone number sending money. The parameter expected is a Valid Safaricom Mobile Number that is M-Pesa registered in the format 2547XXXXXXXX
        /// </summary>
        [JsonProperty("PartyA")]
        public long PartyA { get; set; }

        /// <summary>
        ///     The organization receiving the funds. The parameter expected is a 5 to 7 digit as defined on the Shortcode description above. This can be the same as <see cref="BusinessShortCode"/>.
        /// </summary>
        [JsonProperty("PartyB")]
        public long PartyB { get; set; }

        /// <summary>
        ///     The Mobile Number to receive the STK Pin Prompt. This number can be the same as <see cref="PartyA"/>
        /// </summary>
        [JsonProperty("PhoneNumber")]
        public long PhoneNumber { get; set; }

        /// <summary>
        ///     A CallBack URL is a valid secure URL that is used to receive notifications from M-Pesa API. It is the endpoint to which the results will be sent by M-Pesa API.
        /// </summary>
        [JsonProperty("CallBackURL")]
        public Uri? CallBackUrl { get; set; }

        /// <summary>
        ///     Account Reference: This is an Alpha-Numeric parameter that is defined by your system as an Identifier of the transaction for CustomerPayBillOnline transaction type.
        ///     Along with the business name, this value is also displayed to the customer in the STK Pin Prompt message. Maximum of 12 characters.
        /// </summary>
        [JsonProperty("AccountReference")]
        public string? AccountReference { get; set; }

        /// <summary>
        ///     This is any additional information/comment that can be sent along with the request from your system. Maximum of 13 Characters.
        /// </summary>
        [JsonProperty("TransactionDesc")]
        public string? TransactionDesc { get; set; }

        /// <summary>
        ///     Creates an instance of the <see cref="StkData"/> class
        /// </summary>
        public StkData()
        {
            Timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }
}
