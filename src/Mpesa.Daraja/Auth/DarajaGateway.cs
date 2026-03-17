using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Mpesa.Daraja.Shared;
using Mpesa.Daraja.Shared.Exceptions;

namespace Mpesa.Daraja.Auth;

/// <summary>
///     The Entry point for all interactions with the Daraja API.
/// </summary>
public class DarajaGateway : IDisposable
{
    private readonly string _consumerKey;
    private readonly string _consumerSecret;
    private DarajaToken? _token;
    private readonly SemaphoreSlim _tokenRefreshLock = new(1, 1);

    /// <summary>
    ///     Defines whether the app is running in live or sandbox mode.
    /// </summary>
    public bool IsLive { get; }

    /// <summary>
    ///     The Daraja access token currently cached by the gateway.
    /// </summary>
    public DarajaToken? DarajaClient => _token;

    /// <summary>
    ///     The HTTP client used to communicate with the Daraja API.
    /// </summary>
    public HttpClient HttpClient { get; }

    /// <summary>
    ///     New instance of the <see cref="DarajaGateway"/> class.
    /// </summary>
    /// <param name="httpClientFactory">Factory used to create configured Daraja HTTP clients.</param>
    /// <param name="options">Gateway registration options.</param>
    public DarajaGateway(IHttpClientFactory httpClientFactory, IOptions<DarajaOptions> options)
    {
        ArgumentNullException.ThrowIfNull(httpClientFactory);
        ArgumentNullException.ThrowIfNull(options);

        var gatewayOptions = options.Value;
        _consumerKey = gatewayOptions.ConsumerKey;
        _consumerSecret = gatewayOptions.ConsumerSecret;
        IsLive = gatewayOptions.IsLive;
        HttpClient = httpClientFactory.CreateClient(DarajaServiceCollectionExtensions.HttpClientName);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="DarajaGateway"/> class with explicit credentials.
    /// </summary>
    /// <param name="consumerKey">The Daraja consumer key.</param>
    /// <param name="consumerSecret">The Daraja consumer secret.</param>
    /// <param name="isLive">Whether the production Daraja API should be used.</param>
    public DarajaGateway(string consumerKey, string consumerSecret, bool isLive)
    {
        _consumerKey = consumerKey;
        _consumerSecret = consumerSecret;
        IsLive = isLive;
        HttpClient = new HttpClient
        {
            BaseAddress = CreateBaseAddress(isLive)
        };
    }

    internal static Uri CreateBaseAddress(bool isLive) =>
        new(isLive ? Constants.PRODUCTION_BASE_URL : Constants.SANDBOX_BASE_URL);

    /// <summary>
    ///     Ensures that the gateway has a valid access token before sending authenticated requests.
    /// </summary>
    /// <param name="cancellationToken">Cancels the authentication operation.</param>
    public async Task EnsureAuthenticatedAsync(CancellationToken cancellationToken = default)
    {
        if (HasValidToken())
            return;

        await _tokenRefreshLock.WaitAsync(cancellationToken);

        try
        {
            if (HasValidToken())
                return;

            _token = await GetAccessTokenAsync(cancellationToken);
        }
        finally
        {
            _tokenRefreshLock.Release();
        }
    }

    /// <summary>
    ///     Initializes the Daraja access token if one is not already cached or has expired.
    /// </summary>
    /// <param name="cancellationToken">Cancels the authentication operation.</param>
    public Task InitializeDarajaAsync(CancellationToken cancellationToken = default) =>
        EnsureAuthenticatedAsync(cancellationToken);

    internal async Task<HttpResponseMessage> SendAuthenticatedRequestAsync(
        HttpMethod method,
        string relativeUri,
        HttpContent? content = null,
        CancellationToken cancellationToken = default)
    {
        await EnsureAuthenticatedAsync(cancellationToken);

        using var request = new HttpRequestMessage(method, relativeUri);
        request.Content = content;
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token!.AccessToken);

        return await HttpClient.SendAsync(request, cancellationToken);
    }

    private bool HasValidToken() => _token is not null && _token.IsTokenValid();

    private async Task<DarajaToken> GetAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        var basicAuthToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_consumerKey}:{_consumerSecret}"));

        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"oauth/v1/generate?grant_type={Constants.DEFAULT_GRANT_TYPE}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicAuthToken);

        using var response = await HttpClient.SendAsync(request, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return JsonSerializer.Deserialize<DarajaToken>(content)
                ?? throw new DarajaException("The access token response could not be parsed.");
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
        _tokenRefreshLock.Dispose();
    }
}
