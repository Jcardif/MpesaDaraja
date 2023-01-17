using System;
using System.Collections.Generic;

using System.Globalization;
using System.Net.Http.Headers;
using MpesaDaraja.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MpesaDaraja.Models
{
    public class DarajaClient : IDarajaClient
    {
        public string? AccessToken { get; set; }
        public long ExpiresIn { get; set; }
        public HttpClient Client { get; set; }

        public DarajaClient(string accessToken, long expiresIn)
        {
            AccessToken = accessToken;
            ExpiresIn = expiresIn;

            Client = new HttpClient();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
        }
        
    }
}
