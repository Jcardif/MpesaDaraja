namespace Mpesa.Daraja;

/// <summary>
///     Defines the payload for querying the status of a previously initiated M-Pesa Express (STK Push) transaction.
/// </summary>
internal class MpesaExpressQueryPayload
{
    /// <summary>
    ///     This is the organization's shortcode (Paybill or Buygoods - a 5 to 7-digit account number)
    /// </summary>
    public required long BusinessShortCode { get; set; }

    /// <summary>
    ///     This is the password used for encrypting the request sent: a base64 encoded string.
    ///     (The base64 string is a combination of Shortcode+Passkey+Timestamp).
    /// </summary>
    public required string Password { get; set; }

    /// <summary>
    ///   This is the Timestamp of the transaction
    /// </summary>
    public required string Timestamp { get; set; }

    /// <summary>
    ///     This is a global unique identifier of the processed checkout transaction request.
    /// </summary>
    public required string CheckoutRequestID { get; set; }
}
