using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Mpesa.Daraja.Interfaces;

namespace Mpesa.Daraja.Services
{
    /// <inheritdoc />
    public class DarajaGateway : IDarajaGateway
    {
        private string EndPoint { get; set; }
        private string GrantType { get; set; }
        private string ConsumerKey { get; set; }
        private string ConsumerSecret { get; set; }
        private string PassKey { get; set; }

        /// <summary>
        ///     Initialise an instance of the <see cref="DarajaGateway"/> class
        /// </summary>
        /// <param name="consumerKey"></param>
        /// <param name="consumerSecret"></param>
        /// <param name="passKey"></param>
        /// <param name="inProduction"></param>
        /// <param name="grantType"></param>
        public DarajaGateway(string consumerKey, string consumerSecret, string passKey, bool inProduction, string grantType = "client_credentials")
        {
            EndPoint = inProduction
                ? "https://api.safaricom.co.ke/oauth/v1/generate"
                : "https://sandbox.safaricom.co.ke/oauth/v1/generate";
            GrantType = grantType;
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
            PassKey = passKey;
        }

        /// <inheritdoc />
        public async Task<DarajaClient?> GetDarajaClientAsync(bool isInProduction)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ConsumerKey}:{ConsumerSecret}")));

            var response = await client.GetAsync($"{EndPoint}?grant_type={GrantType}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(content))
                {
                    using var doc = JsonDocument.Parse(content);
                    var root = doc.RootElement;

                    string accessToken = root.GetProperty("access_token").GetString()
                        ?? throw new InvalidOperationException("access_token is null");
                    long expiresIn = root.GetProperty("expires_in").TryGetInt64(out var exp)
                        ? exp
                        : long.Parse(root.GetProperty("expires_in").GetString()!);

                    return new DarajaClient(accessToken, expiresIn, isInProduction);
                }

                // ToDo: handle when null
                return null;
            }

            // handle when status code is not success
            throw new NotImplementedException(await response.Content.ReadAsStringAsync());
        }

        /// <inheritdoc />
        public string GetStkPushPassword(long shortCode, string timestamp) =>
            Convert.ToBase64String(Encoding.UTF8.GetBytes($"{shortCode}{PassKey}{timestamp}"));

        /// <inheritdoc />
        public Task<DarajaClient?> RefreshTokenAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool IsTokenValid(string token)
        {
            throw new NotImplementedException();
        }
    }
}
