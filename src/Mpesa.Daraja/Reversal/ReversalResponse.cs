namespace Mpesa.Daraja;

/// <summary>
///     Defines the response from the M-PESA API for a reversal request.
/// </summary>
public class ReversalResponse
{
    /// <summary>
    ///     Unique identifier for the transaction request from M-PESA
    /// </summary>
    public required string OriginatorConversationId { get; set; }

    /// <summary>
    ///     Unique global identifier for the transaction request
    /// </summary>
    public required string ConversationId { get; set; }

    /// <summary>
    ///     Status code (0 = success, others = error)
    /// </summary>
    public required long ResponseCode { get; set; }

    /// <summary>
    ///    Acknowledgment message
    /// </summary>
    public required string ResponseDescription { get; set; }
}
