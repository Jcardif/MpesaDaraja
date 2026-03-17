# M-Pesa Daraja SDK

<!-- markdownlint-configure-file { "MD013": false } -->

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
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>(optional: true);
builder.Services.AddOpenApi();

builder.Services
    .AddMpesaDaraja(options => builder.Configuration.GetSection("Daraja").Bind(options))
    .WithMpesaExpress();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapPost("/stkpush", async (MpesaExpressPayload payload, IMpesaExpress mpesaExpress, CancellationToken cancellationToken) =>
{
    var result = await mpesaExpress.InitiateStkPush(payload, cancellationToken);
    return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
});

app.Run();
```

For local development, prefer user secrets. The sample includes [`appsettings.example.json`](/Users/joshn/source/repos/Jcardif/MpesaDaraja/samples/Mpesa.Daraja.Sample/appsettings.example.json) as a template if you need a local `appsettings.json`, but do not commit real Daraja credentials.

The sample app in [`samples/Mpesa.Daraja.Sample`](/Users/joshn/source/repos/Jcardif/MpesaDaraja/samples/Mpesa.Daraja.Sample) shows the full minimal API setup for STK push, STK query, and reversal. In development it exposes:

- OpenAPI JSON at `/openapi/v1.json`
- Scalar API reference at `/scalar/v1`

## Supported APIs

- **M-Pesa Express** — STK Push and transaction status queries
- **Transaction Reversal** — Reverse C2B transactions with automatic security credential generation
- More APIs coming soon

## Features

- **ASP.NET Core DI** — Register the SDK with `AddMpesaDaraja(...).WithMpesaExpress().WithReversal()`
- **Scalar-ready sample** — The sample app exposes OpenAPI and a Scalar API reference UI
- **Sandbox & Production** — Switch environments with a single configuration flag
- **Result types** — All API calls return `DarajaResult<T>` for clean error handling
- **Auto token refresh** — Tokens are refreshed automatically before expiry
- **Cancellation-aware APIs** — Outbound calls honor request cancellation tokens

## Documentation

Full docs at [jcardif.github.io/MpesaDaraja](https://jcardif.github.io/MpesaDaraja/).

## License

[MIT](LICENSE.txt)
