using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpesaDaraja.Interfaces
{
    internal interface IDarajaClient
    {
        /// <summary>
        ///     Access token to access other APIs
        /// </summary>
        [JsonProperty("access_token")]
        string? AccessToken { get; set; }

        /// <summary>
        ///    Token expiry time in seconds
        /// </summary>
        [JsonProperty("expires_in")]
        long ExpiresIn { get; set; }

        HttpClient Client { get; set; }


        
    }
}
