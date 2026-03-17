# Transaction Reversal

<!-- markdownlint-configure-file { "MD013": false, "MD060": false } -->

Reverse a completed C2B M-Pesa transaction.

## Usage

```csharp
app.MapPost("/reversal", async (ReversalPayload payload, Reversal reversal, CancellationToken cancellationToken) =>
{
    var result = await reversal.ReverseTransactionAsync(payload, cancellationToken);
    return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
});
```

The SDK automatically generates the `SecurityCredential` by encrypting `InitiatorPassword` from the root `Daraja` configuration with the M-Pesa public key certificate (sandbox or production, based on your gateway config) using RSA PKCS#1 v1.5. Enable reversal support during registration with `.WithReversal()`.

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
