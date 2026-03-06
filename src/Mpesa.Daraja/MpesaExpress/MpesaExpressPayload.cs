namespace Mpesa.Daraja;

/// <summary>
///     Request body for the Mpesa Express (STK Push) API endpoint.
/// </summary>
public class MpesaExpressPayload
{
    /// <summary>
    ///     The M-PESA Shortcode assigned to the Business.
    /// </summary>
    public required int BusinessShortCode { get; set; }

    /// <summary>
    ///     Passkey for encryption of the password parameter.
    /// </summary>
    public required string Passkey { get; set; }

    /// <summary>
    ///
    /// </summary>
    public required TransactionType TransactionType { get; set; }

    /// <summary>
    ///     The transaction amount.
    /// </summary>
    public required decimal Amount { get; set; }

    /// <summary>
    ///     The phone number sending money. Must be a valid Safaricom M-PESA number in the format 2547XXXXXXXX
    /// </summary>
    public required string PartyA { get; set; }

    /// <summary>
    ///     The organization receiving the funds (credit party)
    /// </summary>
    public required string PartyB { get; set; }

    /// <summary>
    ///     The mobile number to receive the USSD prompt. Can be the same as PartyA. Format: 2547XXXXXXXX.
    /// </summary>
    public required string PhoneNumber { get; set; }

    /// <summary>
    ///     The URL where the payment gateway will send the result.
    /// </summary>
    public required string CallBackURL { get; set; }

    /// <summary>
    ///     Alpha-numeric identifier for the transaction, defined by your system.
    ///     Displayed to the customer in the USSD prompt. Max 12 characters
    /// </summary>
    public required string AccountReference { get; set; }

    /// <summary>
    ///     Additional information/comment for the request. Max 13 characters.
    /// </summary>
    public string TransactionDesc { get; set; }

    /// <summary>
    ///     Base64 encoded string used for encrypting the request. Format: base64.encode(Shortcode+Passkey+Timestamp).
    /// </summary>
    public string Password { get; internal set; }

    /// <summary>
    ///     Timestamp of the transaction in the format YYYYMMDDHHmmss.
    /// </summary>
    public string Timestamp { get; internal set; }
}

