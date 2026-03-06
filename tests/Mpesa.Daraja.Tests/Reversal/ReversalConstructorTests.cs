using Mpesa.Daraja.Auth;

namespace Mpesa.Daraja.Tests.Reversal;

public class ReversalConstructorTests
{
    [Fact]
    public void Constructor_DoesNotThrow()
    {
        using var gateway = new DarajaGateway("key", "secret", isLive: false);

        var exception = Record.Exception(() => new Daraja.Reversal(gateway, "password"));

        Assert.Null(exception);
    }

    [Fact]
    public void Constructor_WithEmptyPassword_DoesNotThrow()
    {
        using var gateway = new DarajaGateway("key", "secret", isLive: false);

        var exception = Record.Exception(() => new Daraja.Reversal(gateway, ""));

        Assert.Null(exception);
    }
}
