using System.Net.Http.Headers;
using System.Text;
using MpesaDaraja.Interfaces;
using MpesaDaraja.Models;
using Newtonsoft.Json;

namespace MpesaDaraja.Services
{
    /// <inheritdoc />
    public class DarajaClient : IDarajaClient
    {
        /// <inheritdoc />
        public string? AccessToken { get; private set; }

        /// <inheritdoc />
        public long ExpiresIn { get; private set; }

        /// <inheritdoc />
        public HttpClient Client { get; private set; }

        /// <inheritdoc />
        public void TokenRefreshed(string accessToken, long expiresIn)
        {
            AccessToken = accessToken;
            ExpiresIn = expiresIn;
            ClientSetAuth();
        }


        /// <summary>
        ///     Initiates an instance of the <see cref="DarajaClient"/> class
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="expiresIn"></param>
        public DarajaClient(string accessToken, long expiresIn)
        {
            AccessToken = accessToken;
            ExpiresIn = expiresIn;


            Client = new HttpClient();

            ClientSetAuth();
        }

        private void ClientSetAuth() => Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

        /// <inheritdoc />
        public async Task<PushResult?> SendStkPushAsync(StkData mpesaStkData, string endpoint = "https://sandbox.safaricom.co.ke/mpesa/stkpush/v1/processrequest")
        {
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var data = JsonConvert.SerializeObject(mpesaStkData);

            var stkContent = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await Client.PostAsync(endpoint, stkContent);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var pushResult = JsonConvert.DeserializeObject<PushResult>(content);
                return pushResult;
            }

            //todo: handle when fails
            return null;
        }
    }
}
