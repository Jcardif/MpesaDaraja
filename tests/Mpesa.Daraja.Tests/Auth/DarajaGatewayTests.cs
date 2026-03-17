using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mpesa.Daraja.Auth;
using Mpesa.Daraja.Shared;
using Mpesa.Daraja.Tests.Testing;
using Xunit;

namespace Mpesa.Daraja.Tests.Auth;

public class DarajaGatewayTests
{
    [Fact]
    public void Constructor_WhenSandbox_SetsSandboxBaseUrl()
    {
        using var gateway = new DarajaGateway("key", "secret", isLive: false);

        Assert.Equal(new Uri(Constants.SANDBOX_BASE_URL), gateway.HttpClient.BaseAddress);
    }

    [Fact]
    public void Constructor_WhenLive_SetsProductionBaseUrl()
    {
        using var gateway = new DarajaGateway("key", "secret", isLive: true);

        Assert.Equal(new Uri(Constants.PRODUCTION_BASE_URL), gateway.HttpClient.BaseAddress);
    }

    [Fact]
    public void Constructor_DarajaClientIsNull_BeforeInitialization()
    {
        using var gateway = new DarajaGateway("key", "secret", isLive: false);

        Assert.Null(gateway.DarajaClient);
    }

    [Theory]
    [InlineData(false, Constants.SANDBOX_BASE_URL)]
    [InlineData(true, Constants.PRODUCTION_BASE_URL)]
    public void AddMpesaDaraja_ConfiguresNamedHttpClientBaseAddress(bool isLive, string expectedBaseAddress)
    {
        var services = new ServiceCollection();
        services.AddMpesaDaraja(options =>
        {
            options.ConsumerKey = "key";
            options.ConsumerSecret = "secret";
            options.IsLive = isLive;
        });

        using var serviceProvider = services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        using var client = httpClientFactory.CreateClient(DarajaServiceCollectionExtensions.HttpClientName);

        Assert.Equal(new Uri(expectedBaseAddress), client.BaseAddress);
    }

    [Fact]
    public void AddMpesaDaraja_RegistersSingletonGateway()
    {
        var services = new ServiceCollection();
        services.AddMpesaDaraja(options =>
        {
            options.ConsumerKey = "key";
            options.ConsumerSecret = "secret";
        });

        using var serviceProvider = services.BuildServiceProvider();

        var firstGateway = serviceProvider.GetRequiredService<DarajaGateway>();
        var secondGateway = serviceProvider.GetRequiredService<DarajaGateway>();

        Assert.Same(firstGateway, secondGateway);
    }


    [Fact]
    public async Task SendAuthenticatedAsync_UsesBearerAuthorizationHeader()
    {
        var handler = CreateTokenAndBusinessHandler();
        using var gateway = CreateGateway(handler);

        using var response = await gateway.SendAuthenticatedRequestAsync(
            HttpMethod.Post,
            "mpesa/stkpush/v1/processrequest",
            new StringContent("{}", Encoding.UTF8, "application/json"));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var requests = handler.Requests.ToArray();
        Assert.Equal(2, requests.Length);
        Assert.Equal("Basic", requests[0].Authorization?.Scheme);
        Assert.Equal("Bearer", requests[1].Authorization?.Scheme);
        Assert.Equal("test-access-token", requests[1].Authorization?.Parameter);
    }

    [Fact]
    public async Task SendAuthenticatedAsync_WhenCalledConcurrently_RefreshesTokenOnce()
    {
        var tokenRequestCount = 0;
        var handler = new RecordingHttpMessageHandler(async (request, cancellationToken) =>
        {
            if (request.RequestUri?.PathAndQuery.StartsWith("/oauth/", StringComparison.OrdinalIgnoreCase) == true)
            {
                Interlocked.Increment(ref tokenRequestCount);
                await Task.Delay(50, cancellationToken);
                return CreateJsonResponse(new { access_token = "test-access-token", expires_in = "3600" });
            }

            return CreateJsonResponse(new
            {
                MerchantRequestID = "merchant",
                CheckoutRequestID = "checkout",
                ResponseCode = "0",
                ResponseDescription = "Accepted",
                CustomerMessage = "Prompt sent"
            });
        });

        using var gateway = CreateGateway(handler);
        var mpesaExpress = new Daraja.MpesaExpress(gateway, CreateOptions(passKey: "passkey"));

        await Task.WhenAll(
            mpesaExpress.InitiateStkPush(CreatePayload()),
            mpesaExpress.InitiateStkPush(CreatePayload()));

        Assert.Equal(1, tokenRequestCount);
    }

    [Fact]
    public async Task SendAuthenticatedAsync_ReusesValidCachedToken()
    {
        var tokenRequestCount = 0;
        var handler = new RecordingHttpMessageHandler((request, _) =>
        {
            if (request.RequestUri?.PathAndQuery.StartsWith("/oauth/", StringComparison.OrdinalIgnoreCase) == true)
            {
                Interlocked.Increment(ref tokenRequestCount);
                return Task.FromResult(CreateJsonResponse(new { access_token = "test-access-token", expires_in = "3600" }));
            }

            return Task.FromResult(CreateJsonResponse(new
            {
                MerchantRequestID = "merchant",
                CheckoutRequestID = "checkout",
                ResponseCode = "0",
                ResponseDescription = "Accepted",
                CustomerMessage = "Prompt sent"
            }));
        });

        using var gateway = CreateGateway(handler);
        var mpesaExpress = new Daraja.MpesaExpress(gateway, CreateOptions(passKey: "passkey"));

        await mpesaExpress.InitiateStkPush(CreatePayload());
        await mpesaExpress.InitiateStkPush(CreatePayload());

        Assert.Equal(1, tokenRequestCount);
    }

    [Fact]
    public async Task InitializeDarajaAsync_PopulatesDarajaClient()
    {
        var handler = CreateTokenHandler();
        using var gateway = CreateGateway(handler);

        await gateway.InitializeDarajaAsync();

        Assert.NotNull(gateway.DarajaClient);
        Assert.Equal("test-access-token", gateway.DarajaClient!.AccessToken);
    }

    [Fact]
    public void Dispose_DoesNotThrow()
    {
        using var gateway = CreateGateway(new RecordingHttpMessageHandler((_, _) =>
            Task.FromResult(CreateJsonResponse(new { access_token = "test-access-token", expires_in = "3600" }))));

        var exception = Record.Exception(() => gateway.Dispose());

        Assert.Null(exception);
    }

    [Fact]
    public void ImplementsIDisposable()
    {
        using var gateway = CreateGateway(new RecordingHttpMessageHandler((_, _) =>
            Task.FromResult(CreateJsonResponse(new { access_token = "test-access-token", expires_in = "3600" }))));

        Assert.IsAssignableFrom<IDisposable>(gateway);
    }

    private static DarajaGateway CreateGateway(RecordingHttpMessageHandler handler, bool isLive = false)
    {
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = DarajaGateway.CreateBaseAddress(isLive)
        };

        return new DarajaGateway(
            new SingleClientHttpClientFactory(httpClient),
            Options.Create(new DarajaOptions
            {
                ConsumerKey = "key",
                ConsumerSecret = "secret",
                IsLive = isLive
            }));
    }

    private static IOptions<DarajaOptions> CreateOptions(
        string consumerKey = "key",
        string consumerSecret = "secret",
        bool isLive = false,
        string passKey = "",
        string initiatorPassword = "") =>
        Options.Create(new DarajaOptions
        {
            ConsumerKey = consumerKey,
            ConsumerSecret = consumerSecret,
            IsLive = isLive,
            PassKey = passKey,
            InitiatorPassword = initiatorPassword
        });

    private static RecordingHttpMessageHandler CreateTokenHandler() =>
        new((request, _) =>
        {
            Assert.Equal("Basic", request.Headers.Authorization?.Scheme);
            return Task.FromResult(CreateJsonResponse(new { access_token = "test-access-token", expires_in = "3600" }));
        });

    private static RecordingHttpMessageHandler CreateTokenAndBusinessHandler() =>
        new((request, _) =>
        {
            if (request.RequestUri?.PathAndQuery.StartsWith("/oauth/", StringComparison.OrdinalIgnoreCase) == true)
            {
                return Task.FromResult(CreateJsonResponse(new { access_token = "test-access-token", expires_in = "3600" }));
            }

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{}", Encoding.UTF8, "application/json")
            });
        });

    private static HttpResponseMessage CreateJsonResponse(object payload) =>
        new(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
        };

    private static MpesaExpressPayload CreatePayload() =>
        new()
        {
            BusinessShortCode = 174379,
            TransactionType = TransactionType.CustomerPayBillOnline,
            Amount = 1,
            PartyA = "254708374149",
            PartyB = "174379",
            PhoneNumber = "254708374149",
            CallBackURL = "https://example.com/callback",
            AccountReference = "TestRef",
            TransactionDesc = "Test"
        };

}
