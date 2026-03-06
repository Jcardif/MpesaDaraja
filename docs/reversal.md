# Transaction Reversal

Reverse a completed C2B M-Pesa transaction.

## Usage

```csharp
var reversal = new Reversal(gateway, initiatorPassword);

var payload = new ReversalPayload
{
    Initiator = "testapi",
    TransactionId = "OEI2AK4Q16",
    Amount = 1,
    ReceiverParty = 600978,
    ResultUrl = new Uri("https://mydomain.com/reversal/result"),
    QueueTimeOutUrl = new Uri("https://mydomain.com/reversal/timeout"),
    Remarks = "Reversal request"
};

var result = await reversal.ReverseTransactionAsync(payload);

if (result.IsSuccess)
{
    Console.WriteLine($"ConversationId: {result.Value!.ConversationId}");
}
```

The SDK automatically generates the `SecurityCredential` by encrypting `initiatorPassword` with the M-Pesa public key certificate (sandbox or production, based on your gateway config) using RSA PKCS#1 v1.5.

## Payload Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `Initiator` | `string` | Yes | API username from the M-PESA portal |
| `TransactionId` | `string` | Yes | M-PESA receipt number to reverse |
| `Amount` | `decimal` | Yes | Amount to reverse |
| `ReceiverParty` | `long` | Yes | Organization shortcode |
| `ResultUrl` | `Uri` | No | URL for reversal result notification |
| `QueueTimeOutUrl` | `Uri` | No | URL for timeout notification |
| `Remarks` | `string` | No | Additional info (2–100 chars) |

`CommandId` (`TransactionReversal`), `RecieverIdentifierType` (`11`), and `SecurityCredential` are set automatically.

## Response Properties

| Property | Description |
|---|---|
| `OriginatorConversationId` | Unique ID from M-PESA |
| `ConversationId` | Global unique ID for the request |
| `ResponseCode` | `0` = success |
| `ResponseDescription` | Acknowledgment message |
