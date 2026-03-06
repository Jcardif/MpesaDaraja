# M-Pesa Express (STK Push)

M-Pesa Express is a Merchant/Business initiated C2B (Customer to Business) payment. It sends a payment prompt to the customer's phone via USSD.

## Initiate STK Push

```csharp
var mpesaExpress = new MpesaExpress(gateway, passKey);

var payload = new MpesaExpressPayload
{
    BusinessShortCode = 174379,
    TransactionType = TransactionType.CustomerPayBillOnline,
    Amount = 1,
    PartyA = "254708374149",
    PartyB = "174379",
    PhoneNumber = "254708374149",
    CallBackURL = "https://mydomain.com/callback",
    AccountReference = "MyApp",
    TransactionDesc = "Payment",
    Passkey = passKey
};

var result = await mpesaExpress.InitiateStkPush(payload);

if (result.IsSuccess)
{
    Console.WriteLine($"MerchantRequestID: {result.Value!.MerchantRequestID}");
    Console.WriteLine($"CheckoutRequestID: {result.Value.CheckoutRequestID}");
}
```

## Payload Properties

| Property | Description |
|---|---|
| `BusinessShortCode` | The M-PESA Shortcode (Paybill or Till number) |
| `TransactionType` | `CustomerPayBillOnline` for Paybill, `CustomerBuyGoodsOnline` for Till |
| `Amount` | Transaction amount |
| `PartyA` | Phone number sending money (format: `2547XXXXXXXX`) |
| `PartyB` | Organization receiving funds |
| `PhoneNumber` | Phone number to receive the USSD prompt |
| `CallBackURL` | URL for payment result notification |
| `AccountReference` | Identifier shown to customer (max 12 characters) |
| `TransactionDesc` | Additional info (max 13 characters) |
| `Passkey` | Passkey for password encryption |

## Query Transaction Status

After initiating an STK Push, you can check its status:

```csharp
var queryResult = await mpesaExpress.QueryStkPushStatus(
    businessShortCode: 174379,
    checkoutRequestId: "ws_CO_191220191020363925"
);

if (queryResult.IsSuccess)
{
    var response = queryResult.Value!;
    Console.WriteLine($"ResultCode: {response.ResultCode}");
    Console.WriteLine($"ResultDesc: {response.ResultDesc}");
}
```

## Query Response Properties

| Property | Description |
|---|---|
| `ResponseCode` | Status of the request submission (`0` = success) |
| `ResponseDescription` | Submission status message |
| `MerchantRequestID` | Unique identifier from the API proxy |
| `CheckoutRequestID` | Unique identifier from M-PESA |
| `ResultCode` | Transaction processing status (`0` = success) |
| `ResultDesc` | Processing result message |
