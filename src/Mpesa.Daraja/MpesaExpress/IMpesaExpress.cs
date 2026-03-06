using Mpesa.Daraja.Shared;

namespace Mpesa.Daraja;

/// <summary>
///     Service for the Lipa Na M-Pesa Online which is a Merchant/Business initiated C2B (Customer to Business)
///     transaction
/// </summary>
public interface IMpesaExpress
{
    /// <summary>
    ///     Initiate a payment authorization prompt to a customer whose phone number is registered and active on M-PESA
    /// </summary>
    /// <returns></returns>
    Task<DarajaResult<MpesaExpressResponse>> InitiateStkPush(MpesaExpressPayload payload);

    /// <summary>
    ///     Check the status of a previously initiated M-Pesa Express (STK Push)
    /// </summary>
    /// <returns></returns>
    Task<DarajaResult<MpesaExpressQueryResponse>> QueryStkPushStatus(long businessShortCode, string checkoutRequestId);
}
