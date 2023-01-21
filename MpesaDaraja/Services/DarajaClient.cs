using System.Net;
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

        private bool IsInProduction { get; }

        /// <inheritdoc />
        public HttpClient Client { get; private set; }

        /// <summary>
        ///     Initiates an instance of the <see cref="DarajaClient"/> class
        /// </summary>
        /// <param name="accessToken">Access token to access other APIs</param>
        /// <param name="expiresIn">	Token expiry time in seconds</param>
        /// <param name="isInProduction">Is the API usage in production (value true) or sandbox (value false)</param>
        public DarajaClient(string accessToken, long expiresIn, bool isInProduction)
        {
            AccessToken = accessToken;
            ExpiresIn = expiresIn;
            IsInProduction = isInProduction;


            Client = new HttpClient();

            InitialiseClient();
        }


        /// <inheritdoc />
        public void TokenRefreshed(string accessToken, long expiresIn)
        {
            AccessToken = accessToken;
            ExpiresIn = expiresIn;
            InitialiseClient();
        }

        private void InitialiseClient()
        {
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
        }

        /// <inheritdoc />
        public async Task<PushResponse?> SendStkPushAsync(StkData mpesaStkData)
        {
            var endpoint = IsInProduction
                ? "https://api.safaricom.co.ke/mpesa/stkpush/v1/processrequest"
                : "https://sandbox.safaricom.co.ke/mpesa/stkpush/v1/processrequest";


            var data = JsonConvert.SerializeObject(mpesaStkData);

            var stkContent = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await Client.PostAsync(endpoint, stkContent);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var pushResult = JsonConvert.DeserializeObject<PushResponse>(content);
                return pushResult;
            }

            //todo: handle when fails
            return null;
        }

        /// <inheritdoc />
        public async Task<(bool isCompleted, PushQueryResponse? pushQueryResponse)> QueryStkPushStatus(PushResponse pushResponse, StkData stkData)
        {
            var endpoint = IsInProduction
                ? "https://api.safaricom.co.ke/mpesa/stkpushquery/v1/query"
                : "https://sandbox.safaricom.co.ke/mpesa/stkpushquery/v1/query";

            // todo: handle nulls
            if (stkData.Password == null || stkData.Timestamp == null || pushResponse.CheckoutRequestId == null)
                throw new NotImplementedException("Method parameters are null");


            var stkQuery = new Dictionary<string, object>
            {
                { "BusinessShortCode", stkData.BusinessShortCode },
                { "Password", stkData.Password },
                { "Timestamp", stkData.Timestamp },
                { "CheckoutRequestID", pushResponse.CheckoutRequestId }
            };

            var data = JsonConvert.SerializeObject(stkQuery);
            var queryContent = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await Client.PostAsync(endpoint, queryContent);

            if (response.IsSuccessStatusCode)
            {
                var content=await response.Content.ReadAsStringAsync();
                var pushQueryResponse = JsonConvert.DeserializeObject<PushQueryResponse>(content);
                return (true, pushQueryResponse);
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                var content = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<DarajaError>(content);

                if (error?.ErrorMessage == "The transaction is being processed")
                {
                    return (false, null);
                }
            }

            //todo handle other fails
            throw new NotImplementedException("other status codes not handled");
        }
    }
}
