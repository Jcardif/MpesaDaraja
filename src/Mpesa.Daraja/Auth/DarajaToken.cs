using System.Text.Json.Serialization;

namespace Mpesa.Daraja.Auth;

/// <summary>
///     Defines the response for access token request
/// </summary>
public class DarajaToken
{
    private string _expiresIn;

    /// <summary>
    ///     Access token to access the APIs
    /// </summary>
    [JsonPropertyName("access_token")]
    public required string AccessToken { get; init; }

    /// <summary>
    ///     String value for the token expiry time in seconds.
    /// </summary>
    [JsonPropertyName("expires_in")]
    public required string ExpiresIn
    {
        get => _expiresIn;
        init
        {
            _expiresIn = value;
            var tokenExpirySeconds = int.TryParse(value, out var seconds) ? seconds : 0;

            // set the next token refresh time to be 10% before the actual expiry time
            NextTokenRefreshTime = DateTime.UtcNow.AddSeconds(tokenExpirySeconds * 0.9);
        }
    }

    private DateTime NextTokenRefreshTime { get; init; }


    /// <summary>
    ///     Checks if the current access token is still valid based on the next token refresh time.
    /// </summary>
    /// <returns></returns>
    public bool IsTokenValid()
    {
        return DateTime.UtcNow < NextTokenRefreshTime;
    }
}
