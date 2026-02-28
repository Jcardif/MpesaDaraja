using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Mpesa.Daraja.Interfaces;
using Mpesa.Daraja.Models;

namespace Mpesa.Daraja.Services
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

            var response = await Client.PostAsJsonAsync(endpoint, mpesaStkData);

            if (response.IsSuccessStatusCode)
            {
                var pushResult = await response.Content.ReadFromJsonAsync<PushResponse>();
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

            var response = await Client.PostAsJsonAsync(endpoint, stkQuery);

            if (response.IsSuccessStatusCode)
            {
                var pushQueryResponse = await response.Content.ReadFromJsonAsync<PushQueryResponse>();
                return (true, pushQueryResponse);
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                var error = await response.Content.ReadFromJsonAsync<DarajaError>();

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
