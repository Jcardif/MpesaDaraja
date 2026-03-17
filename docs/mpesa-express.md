# M-Pesa Express (STK Push)

<!-- markdownlint-configure-file { "MD013": false, "MD060": false } -->

Merchant-initiated C2B payment. Sends a USSD payment prompt to the customer's phone.

## Initiate STK Push

```csharp
app.MapPost("/stkpush", async (MpesaExpressPayload payload, IMpesaExpress mpesaExpress, CancellationToken cancellationToken) =>
{
    var result = await mpesaExpress.InitiateStkPush(payload, cancellationToken);
    return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
});
```

## Payload Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `BusinessShortCode` | `int` | Yes | Paybill or Till number |
| `Passkey` | `string` | Yes | Passkey from the Daraja portal |
| `TransactionType` | `TransactionType` | Yes | `CustomerPayBillOnline` (Paybill) or `CustomerBuyGoodsOnline` (Till) |
| `Amount` | `decimal` | Yes | Transaction amount |
| `PartyA` | `string` | Yes | Phone number sending money (`2547XXXXXXXX`) |
| `PartyB` | `string` | Yes | Organization receiving funds |
| `PhoneNumber` | `string` | Yes | Phone number to receive the USSD prompt (`2547XXXXXXXX`) |
| `CallBackURL` | `string` | Yes | URL for payment result notification |
| `AccountReference` | `string` | Yes | Identifier shown to customer (max 12 chars) |
| `TransactionDesc` | `string` | No | Additional info (max 13 chars) |

`Password` and `Timestamp` are set automatically by the SDK.

## Query Transaction Status

```csharp
app.MapPost("/stkpush/query", async (StkPushQueryRequest request, IMpesaExpress mpesaExpress, CancellationToken cancellationToken) =>
{
    var result = await mpesaExpress.QueryStkPushStatus(
        request.BusinessShortCode,
        request.CheckoutRequestId,
        cancellationToken);

    return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
});
```

## Response Properties

### STK Push Response

| Property | Description |
|---|---|
| `MerchantRequestID` | Unique ID from the API proxy |
| `CheckoutRequestID` | Unique ID from M-PESA |
| `ResponseCode` | `0` = successful submission |
| `ResponseDescription` | Submission status message |
| `CustomerMessage` | Message for the customer |

### Query Response

| Property | Description |
|---|---|
| `ResponseCode` | `0` = successful submission |
| `ResponseDescription` | Submission status message |
| `MerchantRequestID` | Unique ID from the API proxy |
| `CheckoutRequestID` | Unique ID from M-PESA |
| `ResultCode` | `0` = successful processing |
| `ResultDesc` | Processing result message |
