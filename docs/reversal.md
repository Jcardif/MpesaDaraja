# Transaction Reversal

The Reversal API enables you to reverse a completed C2B (Customer to Business) M-Pesa transaction.

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
    Console.WriteLine($"ResponseDescription: {result.Value.ResponseDescription}");
}
```

## Security Credentials

The SDK automatically generates the security credential by:

1. Loading the correct M-Pesa public key certificate (sandbox or production) from embedded resources
2. Encrypting the initiator password using RSA with PKCS#1 v1.5 padding
3. Base64-encoding the result

You only need to provide the plain-text `initiatorPassword` when constructing the `Reversal` instance.

## Payload Properties

| Property | Description |
|---|---|
| `Initiator` | Username of the API user from the M-PESA portal |
| `TransactionId` | M-PESA receipt number of the transaction to reverse |
| `Amount` | Amount to reverse |
| `ReceiverParty` | Organization shortcode |
| `ResultUrl` | URL for the reversal result notification |
| `QueueTimeOutUrl` | URL for timeout notification |
| `Remarks` | Additional information (2-100 characters) |

## Fixed Properties

| Property | Value | Description |
|---|---|---|
| `CommandId` | `TransactionReversal` | Always set to this value |
| `RecieverIdentifierType` | `11` | Organization identifier type |
| `SecurityCredential` | Auto-generated | Set automatically by the SDK |

## Response Properties

| Property | Description |
|---|---|
| `OriginatorConversationId` | Unique identifier from M-PESA |
| `ConversationId` | Global unique identifier for the request |
| `ResponseCode` | Status code (`0` = success) |
| `ResponseDescription` | Acknowledgment message |
