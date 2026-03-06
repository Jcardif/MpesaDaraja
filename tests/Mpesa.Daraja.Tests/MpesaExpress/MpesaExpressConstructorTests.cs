using Mpesa.Daraja.Auth;

namespace Mpesa.Daraja.Tests.MpesaExpress;

public class MpesaExpressConstructorTests
{
    [Fact]
    public void Constructor_DoesNotThrow()
    {
        using var gateway = new DarajaGateway("key", "secret", isLive: false);

        var exception = Record.Exception(() => new Daraja.MpesaExpress(gateway, "passkey"));

        Assert.Null(exception);
    }

    [Fact]
    public void Constructor_WithEmptyPassKey_DoesNotThrow()
    {
        using var gateway = new DarajaGateway("key", "secret", isLive: false);

        var exception = Record.Exception(() => new Daraja.MpesaExpress(gateway, ""));

        Assert.Null(exception);
    }

    [Fact]
    public void ImplementsIMpesaExpress()
    {
        using var gateway = new DarajaGateway("key", "secret", isLive: false);

        var mpesaExpress = new Daraja.MpesaExpress(gateway, "passkey");

        Assert.IsAssignableFrom<IMpesaExpress>(mpesaExpress);
    }
}
