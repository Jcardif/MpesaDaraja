using Mpesa.Daraja;
using Mpesa.Daraja.Shared;
using Scalar.AspNetCore;
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>(optional: true)
    .AddJsonFile("appsettings.json", true, true)
    .AddEnvironmentVariables();

builder.Services.AddOpenApi();

builder.Services
    .AddMpesaDaraja(options => builder.Configuration.GetSection("Daraja").Bind(options))
    .WithMpesaExpress()
    .WithReversal();

var app = builder.Build();

app.MapGet("/", () => Results.Redirect("/scalar/v1"))
    .ExcludeFromDescription();
app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.Title = "M-Pesa Daraja Sample";
    options.AddDocument("v1");
});

app.MapPost(
    "/stkpush",
    async (MpesaExpressPayload payload, IMpesaExpress mpesaExpress, CancellationToken cancellationToken) =>
        ToResult(await mpesaExpress.InitiateStkPush(payload, cancellationToken)));

app.MapPost(
    "/stkpush/query",
    async (StkPushQueryRequest request, IMpesaExpress mpesaExpress, CancellationToken cancellationToken) =>
        ToResult(await mpesaExpress.QueryStkPushStatus(
            request.BusinessShortCode,
            request.CheckoutRequestId,
            cancellationToken)));

app.MapPost(
    "/reversal",
    async (ReversalPayload payload, Reversal reversal, CancellationToken cancellationToken) =>
        ToResult(await reversal.ReverseTransactionAsync(payload, cancellationToken)));

app.Run();

static IResult ToResult<T>(DarajaResult<T> result) =>
    result.IsSuccess
        ? Results.Ok(result)
        : Results.BadRequest(result);

internal sealed record StkPushQueryRequest(long BusinessShortCode, string CheckoutRequestId);
