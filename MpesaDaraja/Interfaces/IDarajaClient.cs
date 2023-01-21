using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MpesaDaraja.Models;

namespace MpesaDaraja.Interfaces
{
    internal interface IDarajaClient
    {
        /// <summary>
        ///     Access token to access other APIs
        /// </summary>
        [JsonProperty("access_token")]
        string? AccessToken { get; }

        /// <summary>
        ///    Token expiry time in seconds
        /// </summary>
        [JsonProperty("expires_in")]
        long ExpiresIn { get; }

        /// <summary>
        ///  Client to handle http requests to the daraja API
        /// </summary>
        HttpClient Client { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="expiresIn"></param>
        void TokenRefreshed(string accessToken, long expiresIn);

        /// <summary>
        ///     Initiates online payment on behalf of a customer.
        /// </summary>
        /// <param name="mpesaStkData"></param>
        /// <returns></returns>
        Task<PushResponse?> SendStkPushAsync(StkData mpesaStkData);

        /// <summary>
        ///     Check the status of an Online M-Pesa Payment.
        /// </summary>
        /// <param name="pushResponse">Response from initiating the STK Push</param>
        /// <param name="stkData">The <see cref="StkData"/> object used to initiate the stk push</param>
        /// <returns></returns>
        Task<(bool isCompleted, PushQueryResponse response)> QueryStkPushStatus(PushResponse pushResponse,
            StkData stkData);

    }
}
