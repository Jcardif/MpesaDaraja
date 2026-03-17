# Getting Started

<!-- markdownlint-configure-file { "MD013": false } -->

## Prerequisites

- .NET 8.0 or later
- Safaricom Daraja API credentials ([register here](https://developer.safaricom.co.ke/))

## Installation

```bash
dotnet add package MpesaDarajaSDK
```

## Register the SDK

```csharp
using Mpesa.Daraja;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>(optional: true);

builder.Services.AddOpenApi();
builder.Services
    .AddMpesaDaraja(options => builder.Configuration.GetSection("Daraja").Bind(options))
    .WithMpesaExpress()
    .WithReversal();
```

Use user secrets for local development:

```json
{
  "Daraja": {
    "ConsumerKey": "your-consumer-key",
    "ConsumerSecret": "your-consumer-secret",
    "IsLive": false,
    "PassKey": "your-passkey",
    "InitiatorPassword": "your-initiator-password"
  }
}
```

You can also place the same `Daraja` section in a local `appsettings.json`, but that is not recommended for secrets. The sample includes [`appsettings.example.json`](/Users/joshn/source/repos/Jcardif/MpesaDaraja/samples/Mpesa.Daraja.Sample/appsettings.example.json) as a template.

Set `Daraja:IsLive` to `true` for production. This switches the base URL from sandbox (`sandbox.safaricom.co.ke`) to live (`api.safaricom.co.ke`).

The SDK refreshes tokens automatically — when you call any API method, it checks token validity and re-authenticates if needed.

All public async APIs also accept an optional `CancellationToken`, so ASP.NET Core request cancellation flows through to outbound Daraja calls.

## Sample App

The minimal API sample in [`samples/Mpesa.Daraja.Sample`](/Users/joshn/source/repos/Jcardif/MpesaDaraja/samples/Mpesa.Daraja.Sample) includes:

- `POST /stkpush`
- `POST /stkpush/query`
- `POST /reversal`
- OpenAPI JSON at `/openapi/v1.json` in development
- Scalar API reference at `/scalar/v1` in development

Scalar is a sample-app concern, not an SDK feature. The sample wires it up with `Scalar.AspNetCore` and `app.MapScalarApiReference()`.

## Next Steps

- [M-Pesa Express (STK Push)](mpesa-express.md)
- [Transaction Reversal](reversal.md)
- [Error Handling](error-handling.md)
