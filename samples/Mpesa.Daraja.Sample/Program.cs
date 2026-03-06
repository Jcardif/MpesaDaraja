using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Mpesa.Daraja;
using Mpesa.Daraja.Auth;
using Mpesa.Daraja.Shared.Exceptions;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var consumerKey = config["ConsumerKey"];
var consumerSecret = config["ConsumerSecret"];
var passkey = config["PassKey"];

if (string.IsNullOrEmpty(consumerKey) || string.IsNullOrEmpty(consumerSecret) || string.IsNullOrEmpty(passkey))
{
    Console.WriteLine("ConsumerKey, ConsumerSecret, and PassKey must be provided in user secrets.");
    return;
}

using var gateway = new DarajaGateway(consumerKey, consumerSecret, isLive: false);
await gateway.InitializeDarajaAsync();


var mpesaExpress = new MpesaExpress(gateway, passkey);

var payload = new MpesaExpressPayload
{
    BusinessShortCode = 174379,
    TransactionType = TransactionType.CustomerPayBillOnline,
    Amount = 1,
    PartyA = "254742197114",
    PartyB = "174379",
    PhoneNumber = "254742197114",
    CallBackURL = "https://mydomain.com/mpesa-express-simulate/",
    AccountReference = "Mpesa.Daraja.Sample",
    TransactionDesc = "Test Payment from Mpesa.Daraja Sample",
    Passkey = passkey
};

try
{
    var result = await mpesaExpress.InitiateStkPush(payload);

    if (result.IsSuccess)
    {
        Console.WriteLine("STK Push initiated successfully:");
        Console.WriteLine(JsonSerializer.Serialize(result.Value, new JsonSerializerOptions { WriteIndented = true }));
    }
    else
    {
        Console.WriteLine($"Error: {result.Error!.ErrorMessage}");
    }

    if (result.Value is null)
        return;

    await Task.Delay(3000);

    Console.WriteLine($"Begin checking STK Push status with the CheckoutRequestID {result.Value.CheckoutRequestID}");

    var queryResult = await mpesaExpress.QueryStkPushStatus(payload.BusinessShortCode, result.Value.CheckoutRequestID);

    if (queryResult.IsSuccess)
    {
        Console.WriteLine("STK Push status:");
        Console.WriteLine(JsonSerializer.Serialize(queryResult.Value, new JsonSerializerOptions { WriteIndented = true }));
    }
    else
    {
        Console.WriteLine($"Error querying STK Push status: {queryResult.Error!.ErrorMessage}");
    }
}
catch (DarajaException e)
{
    Console.WriteLine($"Daraja error: {e.Message}");
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}
