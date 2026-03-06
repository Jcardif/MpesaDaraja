using Mpesa.Daraja.Auth;

namespace Mpesa.Daraja.Shared;

/// <summary>
///     Class for extension methods related to the Daraja API.
/// </summary>
public static class DarajaExtensions
{
    /// <summary>
    ///      Checks if the Daraja client has a valid access token and initializes it if necessary.
    /// </summary>
    /// <param name="gateway"></param>
    public static async Task EnsureAuthenticatedAsync(this DarajaGateway gateway)
    {
        if (gateway.DarajaClient is null || !gateway.DarajaClient.IsTokenValid())
        {
            await gateway.InitializeDarajaAsync();
        }
    }
}
