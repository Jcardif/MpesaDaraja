﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MpesaDaraja.Models;

namespace MpesaDaraja.Interfaces
{

    // todo : handle nulls

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

        HttpClient Client { get; }

        void TokenRefreshed(string accessToken, long expiresIn);

        Task<PushResult?> SendSTKPushAsync(StkData mpesaStkData, string endpoint= "https://sandbox.safaricom.co.ke/mpesa/stkpush/v1/processrequest");

    }
}