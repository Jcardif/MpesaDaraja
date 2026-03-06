using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Mpesa.Daraja.Shared;
using Mpesa.Daraja.Shared.Exceptions;

namespace Mpesa.Daraja.Auth;

/// <summary>
///     The Entry point for all interactions with the Daraja API.
/// </summary>
public class DarajaGateway : IDisposable
{
    /// <summary>
    ///     The Daraja client
    /// </summary>
    public DarajaToken? DarajaClient { get; private set; }

    public HttpClient HttpClient { get; }

    /// <summary>
    ///     Defines whether the app is running in live or sandbox mode.
    /// </summary>
    public bool IsLive { get; }

    private readonly string _consumerKey;
    private readonly string _consumerSecret;

    /// <summary>
    ///     New instance of the <see cref="DarajaGateway"/> class, initialized with the consumer key and consumer secret.
    /// </summary>
    /// <param name="consumerKey"></param>
    /// <param name="consumerSecret"></param>
    /// <param name="isLive"></param>
    public DarajaGateway(string  consumerKey, string consumerSecret, bool isLive)
    {
        _consumerKey = consumerKey;
        _consumerSecret = consumerSecret;
        IsLive = isLive;

        HttpClient = new HttpClient()
        {
            BaseAddress = isLive ? new Uri(Constants.PRODUCTION_BASE_URL) : new Uri(Constants.SANDBOX_BASE_URL)
        };
    }


    /// <summary>
    ///     Initializes the Daraja API Token by making a request to the Daraja API.
    /// </summary>
    /// <exception cref="DarajaException"></exception>
    public async Task InitializeDarajaAsync()
    {
        var basicAuthToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_consumerKey}:{_consumerSecret}"));

        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuthToken);

        var response = await HttpClient.GetAsync($"oauth/v1/generate?grant_type={Constants.DEFAULT_GRANT_TYPE}");
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var darajaClient = JsonSerializer.Deserialize<DarajaToken>(content)!;
            DarajaClient = darajaClient;

            // Set the access token in the HttpClient for subsequent requests
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", darajaClient.AccessToken);

            return;
        }

        if (string.IsNullOrEmpty(content))
        {
            throw new DarajaException(
                "An error occurred while generating the access token, but no error details were provided.");
        }

        var darajaError = JsonSerializer.Deserialize<DarajaError>(content)!;
        throw new DarajaException(darajaError.ErrorMessage, darajaError);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        HttpClient.Dispose();
    }
}
