using System.Text.Json.Serialization;

namespace Mpesa.Daraja;

/// <summary>
///     Identifies the type of transaction being made
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransactionType
{
    /// <summary>
    ///     Use for PayBill Numbers
    /// </summary>
    CustomerPayBillOnline,

    /// <summary>
    ///     Use for Till Numbers
    /// </summary>
    CustomerBuyGoodsOnline
}
