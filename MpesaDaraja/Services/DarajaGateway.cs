using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MpesaDaraja.Interfaces;
using Newtonsoft.Json;

namespace MpesaDaraja.Services
{
    public class DarajaGateway : IDarajaGateway
    {
        private string EndPoint { get; set; }
        private string GrantType { get; set; }
        private string ConsumerKey { get; set; }
        private string ConsumerSecret { get; set; }
        private string PassKey { get; set; }


        public DarajaGateway(string endPoint, string consumerKey, string consumerSecret, string passKey, string grantType= "client_credentials")
        {
            EndPoint=endPoint;
            GrantType=grantType;
            ConsumerKey=consumerKey;
            ConsumerSecret=consumerSecret;
            PassKey=passKey;
        }

        public async Task<DarajaClient?> GetDarajaClientAsync()
        {
            var client=new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ConsumerKey}:{ConsumerSecret}")));

            var response = await client.GetAsync($"{EndPoint}?grant_type={GrantType}");

            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();

            if (!string.IsNullOrEmpty(content))
            {
                var data = JsonConvert.DeserializeObject<dynamic>(content);

                // ToDO: Handle when null
                string accessToken = data?["access_token"].ToString() ?? throw new InvalidOperationException();
                long expiresIn = data?["expires_in"];


                return new DarajaClient(accessToken, expiresIn);
            }


            // ToDo: handle when null
            return null;
        }

        public string GetStkPushPassword(long shortCode, string timestamp) =>
            Convert.ToBase64String(Encoding.UTF8.GetBytes($"{shortCode}{PassKey}{timestamp}"));


        public Task<DarajaClient?> RefreshTokenAsync()
        {
            throw new NotImplementedException();
        }

        public bool IsTokenValid(string token)
        {
            throw new NotImplementedException();
        }


    }
}
