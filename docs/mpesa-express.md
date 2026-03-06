# M-Pesa Express (STK Push)

Merchant-initiated C2B payment. Sends a USSD payment prompt to the customer's phone.

## Initiate STK Push

```csharp
var mpesaExpress = new MpesaExpress(gateway, passKey);

var payload = new MpesaExpressPayload
{
    BusinessShortCode = 174379,
    Passkey = passKey,
    TransactionType = TransactionType.CustomerPayBillOnline,
    Amount = 1,
    PartyA = "254708374149",
    PartyB = "174379",
    PhoneNumber = "254708374149",
    CallBackURL = "https://mydomain.com/callback",
    AccountReference = "MyApp",
    TransactionDesc = "Payment"
};

var result = await mpesaExpress.InitiateStkPush(payload);

if (result.IsSuccess)
{
    Console.WriteLine($"CheckoutRequestID: {result.Value!.CheckoutRequestID}");
}
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
var queryResult = await mpesaExpress.QueryStkPushStatus(
    businessShortCode: 174379,
    checkoutRequestId: result.Value!.CheckoutRequestID
);

if (queryResult.IsSuccess)
{
    Console.WriteLine($"ResultCode: {queryResult.Value!.ResultCode}");
    Console.WriteLine($"ResultDesc: {queryResult.Value.ResultDesc}");
}
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
