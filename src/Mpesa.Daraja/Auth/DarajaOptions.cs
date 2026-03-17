namespace Mpesa.Daraja.Auth;

/// <summary>
///     Options for registering the Daraja gateway.
/// </summary>
public sealed class DarajaOptions
{
    /// <summary>
    ///     Consumer key from the daraja portal
    /// </summary>
    public string ConsumerKey { get; set; } = string.Empty;

    /// <summary>
    ///     Consumer secret from the daraja portal
    /// </summary>
    public string ConsumerSecret { get; set; } = string.Empty;

    /// <summary>
    ///     Indicates whether the SDK should target the production Daraja API.
    /// </summary>
    public bool IsLive { get; set; }

    /// <summary>
    ///     Passkey for encryption of request parameters.
    /// </summary>
    public string PassKey { get; set; } = string.Empty;

    /// <summary>
    ///     Initiator password used in encoding the security credential
    /// </summary>
    public string InitiatorPassword { get; set; } = string.Empty;
}
