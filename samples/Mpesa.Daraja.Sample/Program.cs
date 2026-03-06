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

var jsonOptions = new JsonSerializerOptions { WriteIndented = true };

using var gateway = new DarajaGateway(consumerKey, consumerSecret, isLive: false);
await gateway.InitializeDarajaAsync();

while (true)
{
    Console.WriteLine();
    Console.WriteLine("Select a service:");
    Console.WriteLine("  1. M-Pesa Express (STK Push & Query)");
    Console.WriteLine("  2. Transaction Reversal");
    Console.WriteLine("  0. Exit");
    Console.Write("> ");

    var choice = Console.ReadLine()?.Trim();

    try
    {
        switch (choice)
        {
            case "1":
                await RunMpesaExpress(gateway, passkey, jsonOptions);
                break;
            case "2":
                await RunReversal(gateway, config, jsonOptions);
                break;
            case "0":
                return;
            default:
                Console.WriteLine("Invalid selection. Try again.");
                break;
        }
    }
    catch (DarajaException e)
    {
        Console.WriteLine($"Daraja error: {e.Message}");
    }
    catch (Exception e)
    {
        Console.WriteLine($"Error: {e.Message}");
    }
}

static async Task RunMpesaExpress(DarajaGateway gateway, string passkey, JsonSerializerOptions jsonOptions)
{
    var mpesaExpress = new MpesaExpress(gateway, passkey);

    var payload = new MpesaExpressPayload
    {
        BusinessShortCode = 174379,
        TransactionType = TransactionType.CustomerPayBillOnline,
        Amount = 1,
        PartyA = "254708374149",
        PartyB = "174379",
        PhoneNumber = "254708374149",
        CallBackURL = "https://mydomain.com/mpesa-express-simulate/",
        AccountReference = "Mpesa.Daraja.Sample",
        TransactionDesc = "Test Payment from Mpesa.Daraja Sample",
        Passkey = passkey
    };

    var result = await mpesaExpress.InitiateStkPush(payload);

    if (result.IsSuccess)
    {
        Console.WriteLine("STK Push initiated successfully:");
        Console.WriteLine(JsonSerializer.Serialize(result.Value, jsonOptions));
    }
    else
    {
        Console.WriteLine($"Error: {result.Error!.ErrorMessage}");
        return;
    }

    if (result.Value is null)
        return;

    Console.WriteLine("Waiting 3 seconds before querying status...");
    await Task.Delay(3000);

    Console.WriteLine($"Checking STK Push status for CheckoutRequestID: {result.Value.CheckoutRequestID}");

    var queryResult = await mpesaExpress.QueryStkPushStatus(payload.BusinessShortCode, result.Value.CheckoutRequestID);

    if (queryResult.IsSuccess)
    {
        Console.WriteLine("STK Push status:");
        Console.WriteLine(JsonSerializer.Serialize(queryResult.Value, jsonOptions));
    }
    else
    {
        Console.WriteLine($"Error querying STK Push status: {queryResult.Error!.ErrorMessage}");
    }
}

static async Task RunReversal(DarajaGateway gateway, IConfiguration config, JsonSerializerOptions jsonOptions)
{
    var initiatorPassword = config["InitiatorPassword"];
    if (string.IsNullOrEmpty(initiatorPassword))
    {
        Console.WriteLine("InitiatorPassword must be provided in user secrets.");
        return;
    }

    var reversal = new Reversal(gateway, initiatorPassword);

    var payload = new ReversalPayload
    {
        Initiator = "testapi",
        TransactionId = "OEI2AK4Q16",
        Amount = 1,
        ReceiverParty = 600978,
        ResultUrl = new Uri("https://mydomain.com/reversal/result/"),
        QueueTimeOutUrl = new Uri("https://mydomain.com/reversal/queue/"),
        Remarks = "Reversal test from Mpesa.Daraja Sample"
    };

    var result = await reversal.ReverseTransactionAsync(payload);

    if (result.IsSuccess)
    {
        Console.WriteLine("Reversal initiated successfully:");
        Console.WriteLine(JsonSerializer.Serialize(result.Value, jsonOptions));
    }
    else
    {
        Console.WriteLine($"Error: {result.Error!.ErrorMessage}");
    }
}
