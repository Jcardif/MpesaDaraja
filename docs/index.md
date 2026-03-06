# M-Pesa Daraja SDK

A C# SDK for the Safaricom M-Pesa Daraja API 3.0. Supports .NET 8.0, 9.0, and 10.0.

## Supported APIs

- **M-Pesa Express (STK Push)** — Initiate payment prompts and query transaction status
- **Transaction Reversal** — Reverse C2B transactions with automatic security credential generation
- More APIs coming soon

## Features

- **Sandbox & Production** — Switch environments with a single constructor flag
- **Result types** — All API calls return `DarajaResult<T>` for clean error handling
- **Auto token refresh** — Tokens are refreshed automatically before expiry

## Installation

```bash
dotnet add package MpesaDarajaSDK
```

Get started with the [setup guide](getting-started.md).
