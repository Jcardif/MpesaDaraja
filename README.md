# M-Pesa Daraja SDK

[![CI](https://github.com/Jcardif/MpesaDaraja/actions/workflows/ci.yml/badge.svg)](https://github.com/Jcardif/MpesaDaraja/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/MpesaDarajaSDK.svg)](https://www.nuget.org/packages/MpesaDarajaSDK/)

A C# SDK for the Safaricom M-Pesa Daraja API 3.0. Supports .NET 8.0, 9.0, and 10.0.

## Installation

```bash
dotnet add package MpesaDarajaSDK
```

## Quick Start

```csharp
using Mpesa.Daraja;
using Mpesa.Daraja.Auth;

// Initialize gateway (isLive: true for production)
using var gateway = new DarajaGateway(consumerKey, consumerSecret, isLive: false);
await gateway.InitializeDarajaAsync();

// STK Push
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
    Console.WriteLine($"CheckoutRequestID: {result.Value!.CheckoutRequestID}");
else
    Console.WriteLine($"Error: {result.Error!.ErrorMessage}");
```

## Supported APIs

- **M-Pesa Express** — STK Push and transaction status queries
- **Transaction Reversal** — Reverse C2B transactions with automatic security credential generation
- More APIs coming soon

## Features

- **Sandbox & Production** — Switch with a single constructor flag
- **Result types** — All API calls return `DarajaResult<T>` for clean error handling
- **Auto token refresh** — Tokens are refreshed automatically before expiry

## Documentation

Full docs at [jcardif.github.io/MpesaDaraja](https://jcardif.github.io/MpesaDaraja/).

## License

[MIT](LICENSE.txt)
