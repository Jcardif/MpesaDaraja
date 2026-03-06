using Mpesa.Daraja.Auth;
using Mpesa.Daraja.Shared;
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
    public void Constructor_SetsIsLiveProperty()
    {
        using var sandboxGateway = new DarajaGateway("key", "secret", isLive: false);
        using var liveGateway = new DarajaGateway("key", "secret", isLive: true);

        Assert.False(sandboxGateway.IsLive);
        Assert.True(liveGateway.IsLive);
    }

    [Fact]
    public void Constructor_DarajaClientIsNull_BeforeInitialization()
    {
        using var gateway = new DarajaGateway("key", "secret", isLive: false);

        Assert.Null(gateway.DarajaClient);
    }

    [Fact]
    public void Constructor_HttpClientIsNotNull()
    {
        using var gateway = new DarajaGateway("key", "secret", isLive: false);

        Assert.NotNull(gateway.HttpClient);
    }

    [Fact]
    public void Dispose_DoesNotThrow()
    {
        var gateway = new DarajaGateway("key", "secret", isLive: false);

        var exception = Record.Exception(() => gateway.Dispose());

        Assert.Null(exception);
    }

    [Fact]
    public void Constructor_WithEmptyCredentials_DoesNotThrow()
    {
        var exception = Record.Exception(() =>
        {
            using var gateway = new DarajaGateway("", "", isLive: false);
        });

        Assert.Null(exception);
    }

    [Fact]
    public void ImplementsIDisposable()
    {
        using var gateway = new DarajaGateway("key", "secret", isLive: false);

        Assert.IsAssignableFrom<IDisposable>(gateway);
    }

}
