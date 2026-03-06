# M-Pesa Daraja SDK

C# M-Pesa SDK leveraging the Daraja API 3.0 allowing easy integration of M-Pesa Payments into your .NET applications.

[![CI](https://github.com/Jcardif/MpesaDaraja/actions/workflows/ci.yml/badge.svg)](https://github.com/Jcardif/MpesaDaraja/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/MpesaDarajaSDK.svg)](https://www.nuget.org/packages/MpesaDarajaSDK/)

## Installation

```bash
dotnet add package MpesaDarajaSDK
```

## Quick Start

```csharp
using Mpesa.Daraja;
using Mpesa.Daraja.Auth;

using var gateway = new DarajaGateway(consumerKey, consumerSecret, isLive: false);
await gateway.InitializeDarajaAsync();

var mpesaExpress = new MpesaExpress(gateway, passKey);
var result = await mpesaExpress.InitiateStkPush(payload);
```

## Features

- **M-Pesa Express** - STK Push and transaction status queries
- **Transaction Reversal** - Reverse C2B transactions with automatic security credential generation
- **Sandbox & Production** - Switch environments with a single flag
- **Result types** - All API calls return `DarajaResult<T>` for clean error handling

## Documentation

Full documentation is available at **[jcardif.github.io/MpesaDaraja](https://jcardif.github.io/MpesaDaraja/)**.

### Serving docs locally

```bash
cd docs
python3 -m venv .venv
source .venv/bin/activate
pip install mkdocs-material
mkdocs serve --config-file mkdocs.yml
```

### Deploying docs

Docs are automatically deployed to GitHub Pages via the `docs.yml` workflow on push to `main`. To deploy manually:

```bash
cd docs && source .venv/bin/activate
mkdocs gh-deploy --force --config-file mkdocs.yml
```

## License

MIT - See [LICENSE](https://github.com/Jcardif/MpesaDaraja/blob/master/LICENSE.txt)
