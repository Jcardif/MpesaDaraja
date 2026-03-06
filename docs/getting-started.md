# Getting Started

## Prerequisites

- .NET 8.0 or later
- Safaricom Daraja API credentials ([register here](https://developer.safaricom.co.ke/))

## Installation

```bash
dotnet add package MpesaDarajaSDK
```

## Initialize the Gateway

```csharp
using Mpesa.Daraja;
using Mpesa.Daraja.Auth;

using var gateway = new DarajaGateway(consumerKey, consumerSecret, isLive: false);
await gateway.InitializeDarajaAsync();
```

Set `isLive: true` for production. This switches the base URL from sandbox (`sandbox.safaricom.co.ke`) to live (`api.safaricom.co.ke`).

The SDK refreshes tokens automatically — when you call any API method, it checks token validity and re-authenticates if needed.

## Next Steps

- [M-Pesa Express (STK Push)](mpesa-express.md)
- [Transaction Reversal](reversal.md)
- [Error Handling](error-handling.md)
