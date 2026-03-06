using System.Text.Json.Serialization;

namespace Mpesa.Daraja.Shared;

/// <summary>
///     Defines the structure of an error response from the Daraja API.
/// </summary>
public class DarajaError
{
    /// <summary>
    ///     Unique identifier for the request that resulted in the error.
    /// </summary>
    [JsonPropertyName("requestId")]
    public required string? RequestId { get; set; }

    /// <summary>
    ///     Daraja API response error code.
    /// </summary>
    [JsonPropertyName("errorCode")]
    public required string ErrorCode { get; set; }

    /// <summary>
    ///     Description of the error that occurred during the API request.
    /// </summary>
    [JsonPropertyName("errorMessage")]
    public required string ErrorMessage { get; set; }
}

