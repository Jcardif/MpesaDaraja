namespace Mpesa.Daraja;

/// <summary>
///     Defines the payload for a reversal request to the M-PESA API.
/// </summary>
public class ReversalPayload
{
    /// <summary>
    ///     Username of the API user created on M-PESA portal
    /// </summary>
    public required string Initiator { get; set; }

    /// <summary>
    ///     Encrypted password for the API user
    /// </summary>
    public string SecurityCredential { get; internal set; }

    /// <summary>
    ///     Only 'TransactionReversal' is allowed
    /// </summary>
    public string CommandId { get; } = "TransactionReversal";

    /// <summary>
    ///     M-PESA Receipt Number for the transaction being reversed
    /// </summary>
    public required string TransactionId { get; set; }

    /// <summary>
    ///     Tx Amount being reversed
    /// </summary>
    public required decimal Amount { get; set; }

    /// <summary>
    ///     Organization Short Code
    /// </summary>
    public required long ReceiverParty { get; set; }

    /// <summary>
    ///     Type of Organization (should be '11')
    /// </summary>
    public long RecieverIdentifierType { get; } = 11;

    /// <summary>
    ///     URL for result notification
    /// </summary>
    public Uri ResultUrl { get; set; }

    /// <summary>
    ///     URL for timeout notification
    /// </summary>
    public Uri QueueTimeOutUrl { get; set; }

    /// <summary>
    ///     Additional information (2-100 characters)
    /// </summary>
    public string Remarks { get; set; }
}
