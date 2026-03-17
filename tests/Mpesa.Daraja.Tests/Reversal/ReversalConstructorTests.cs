using Microsoft.Extensions.Options;
using Mpesa.Daraja.Auth;
using Mpesa.Daraja.Tests.Testing;

namespace Mpesa.Daraja.Tests.Reversal;

public class ReversalConstructorTests
{
    [Fact]
    public void Constructor_DoesNotThrow()
    {
        using var gateway = CreateGateway();

        var exception = Record.Exception(() => new Daraja.Reversal(gateway, CreateOptions("password")));

        Assert.Null(exception);
    }

    [Fact]
    public void Constructor_WithEmptyPassword_DoesNotThrow()
    {
        using var gateway = CreateGateway();

        var exception = Record.Exception(() => new Daraja.Reversal(gateway, CreateOptions(string.Empty)));

        Assert.Null(exception);
    }

    [Fact]
    public void LegacyConstructor_DoesNotThrow()
    {
        using var gateway = new DarajaGateway("key", "secret", isLive: false);

        var exception = Record.Exception(() => new Daraja.Reversal(gateway, "password"));

        Assert.Null(exception);
    }

    private static DarajaGateway CreateGateway()
    {
        var httpClient = new HttpClient(new RecordingHttpMessageHandler((_, _) =>
            Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK))))
        {
            BaseAddress = DarajaGateway.CreateBaseAddress(false)
        };

        return new DarajaGateway(
            new SingleClientHttpClientFactory(httpClient),
            Options.Create(new DarajaOptions
            {
                ConsumerKey = "key",
                ConsumerSecret = "secret",
                IsLive = false
            }));
    }

    private static IOptions<DarajaOptions> CreateOptions(string initiatorPassword) =>
        Options.Create(new DarajaOptions
        {
            InitiatorPassword = initiatorPassword
        });
}
