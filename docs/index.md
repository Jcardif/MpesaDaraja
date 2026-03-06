# M-Pesa Daraja SDK

C# M-Pesa SDK leveraging the Daraja API 3.0 for easy integration of M-Pesa Payments into .NET applications.

## Features

- **M-Pesa Express (STK Push)** - Initiate payment prompts and query transaction status
- **Transaction Reversal** - Reverse completed C2B transactions with automatic security credential generation
- **Sandbox & Production** - Switch environments with a single flag
- **Result types** - All API calls return `DarajaResult<T>` for clean error handling

## Quick Start

```bash
dotnet add package MpesaDarajaSDK
```

```csharp
using Mpesa.Daraja;
using Mpesa.Daraja.Auth;

using var gateway = new DarajaGateway(consumerKey, consumerSecret, isLive: false);
await gateway.InitializeDarajaAsync();
```

See the [Getting Started](getting-started.md) guide for full setup instructions.
