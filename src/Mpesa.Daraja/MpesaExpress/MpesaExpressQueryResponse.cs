namespace Mpesa.Daraja;

/// <summary>
///     Defines the response structure for querying the status of an M-Pesa Express (STK Push) transaction.
/// </summary>
public class MpesaExpressQueryResponse
{
    /// <summary>
    ///     This is a numeric status code that indicates the status of the transaction submission.
    ///     O means successful submission and any other code means an error occurred.
    /// </summary>
    public string ResponseCode { get; set; }


    /// <summary>
    ///     Response description is an acknowledgment message from the API that gives the status of the
    ///     request submission usually maps to a specific ResponseCode value.
    ///     It can be a "Success" submission message or an error description.
    /// </summary>
    public string ResponseDescription { get; set; }

    /// <summary>
    ///     This is a global unique Identifier for any submitted payment request.
    /// </summary>
    public string MerchantRequestID { get; set; }

    /// <summary>
    ///     This is a global unique identifier of the processed checkout transaction request.
    /// </summary>
    public string CheckoutRequestID { get; set; }

    /// <summary>
    ///     This is a numeric status code that indicates the status of the transaction processing.
    ///     0 means successful processing and any other code means an error occurred or the transaction failed.
    /// </summary>
    public string ResultCode { get; set; }

    /// <summary>
    ///     Result description is a message from the API that gives the status of the request processing,
    ///     usually maps to a specific ResultCode value.
    ///     It can be a success processing message or an error description message.
    /// </summary>
    public string ResultDesc { get; set; }
}
