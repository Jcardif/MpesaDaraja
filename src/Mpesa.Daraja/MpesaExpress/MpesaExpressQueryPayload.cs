namespace Mpesa.Daraja;

/// <summary>
///     Defines the payload for querying the status of a previously initiated M-Pesa Express (STK Push) transaction.
/// </summary>
internal class MpesaExpressQueryPayload
{
    /// <summary>
    ///     This is the organization's shortcode (Paybill or Buygoods - a 5 to 7-digit account number)
    /// </summary>
    internal required long BusinessShortCode { get; set; }

    /// <summary>
    ///     Base64 encoded string used for encrypting the request. Format: base64.encode(Shortcode+Passkey+Timestamp).
    ///     This value is populated by the SDK immediately before the request is sent.
    /// </summary>
    internal required string Password { get;  set; }

    /// <summary>
    ///   This is the Timestamp of the transaction
    /// </summary>
    internal required string Timestamp { get; set; }

    /// <summary>
    ///     This is a global unique identifier of the processed checkout transaction request.
    /// </summary>
    internal required string CheckoutRequestID { get; set; }
}
