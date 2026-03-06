# Getting Started

## Prerequisites

- .NET 10.0 or later
- Safaricom Daraja API credentials ([register here](https://developer.safaricom.co.ke/))

## Installation

```bash
dotnet add package MpesaDarajaSDK
```

## Configuration

You need three credentials from the [Daraja portal](https://developer.safaricom.co.ke/MyApps):

- **Consumer Key**
- **Consumer Secret**
- **Pass Key** (for M-Pesa Express)

### Initialize the Gateway

```csharp
using Mpesa.Daraja;
using Mpesa.Daraja.Auth;

using var gateway = new DarajaGateway(consumerKey, consumerSecret, isLive: false);
await gateway.InitializeDarajaAsync();
```

Set `isLive: true` when deploying to production. This switches the base URL from the sandbox to the live Safaricom API.

### Token Management

The SDK handles token refresh automatically. When you call any API method, it checks if the current token is still valid and re-authenticates if needed.

## Next Steps

- [M-Pesa Express (STK Push)](mpesa-express.md)
- [Transaction Reversal](reversal.md)
- [Error Handling](error-handling.md)
