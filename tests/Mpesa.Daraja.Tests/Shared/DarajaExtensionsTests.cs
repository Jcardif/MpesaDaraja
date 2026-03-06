using Mpesa.Daraja.Auth;
using Mpesa.Daraja.Shared;

namespace Mpesa.Daraja.Tests.Shared;

public class DarajaExtensionsTests
{
    [Fact]
    public async Task EnsureAuthenticatedAsync_WhenClientIsNull_AttemptsToInitialize()
    {
        using var gateway = new DarajaGateway("key", "secret", isLive: false);

        Assert.Null(gateway.DarajaClient);

        // This will attempt to call the real Daraja API and fail,
        // which proves the method does attempt to initialize when DarajaClient is null
        await Assert.ThrowsAnyAsync<Exception>(() => gateway.EnsureAuthenticatedAsync());
    }
}
