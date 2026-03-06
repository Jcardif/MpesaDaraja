namespace Mpesa.Daraja;

/// <summary>
///     The response returned by the Daraja API when making a request to the Mpesa Express endpoint.
/// </summary>
public class MpesaExpressResponse
{
    /// <summary>
    ///     Global unique identifier for the transaction request returned by the API proxy upon successful request submission.
    /// </summary>
    public string MerchantRequestID { get; set; }

    /// <summary>
    ///     Global unique identifier for the transaction request returned by M-PESA upon successful request submission.
    /// </summary>
    public string CheckoutRequestID { get; set; }

    /// <summary>
    ///     Staus code indicating the status of the transaction submission.
    ///     0 means successful submission; any other code indicates an error.
    /// </summary>
    public string ResponseCode { get; set; }

    /// <summary>
    ///     Acknowledgment message from the API that gives the status of the request submission,
    ///     usually mapping to a specific ResponseCode value.
    /// </summary>
    public string ResponseDescription { get; set; }

    /// <summary>
    ///     Message intended for the customer, usually confirming the status of the request.
    /// </summary>
    public string CustomerMessage { get; set; }
}


