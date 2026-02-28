using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Mpesa.Daraja.Models;
using Mpesa.Daraja.Services;

namespace Daraja.App
{
    internal class Program
    {
        private static readonly JsonSerializerOptions s_jsonOptions = new() { WriteIndented = true };

        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            var consumerKey = config["ConsumerKey"];
            var consumerSecret = config["ConsumerSecret"];
            var passKey = config["PassKey"];

            if (consumerKey == null || consumerSecret == null || passKey == null)
                return;

            var gateway = new DarajaGateway(consumerKey, consumerSecret, passKey, false);

            var darajaClient = await gateway.GetDarajaClientAsync(false);

            if (darajaClient == null)
                return;

            await MakeStkPush(gateway, darajaClient);
        }

        private static async Task MakeStkPush(DarajaGateway darajaGateway, DarajaClient darajaClient)
        {
            Console.WriteLine("Receiver for the stk push");
            var receiver = Convert.ToInt64(Console.ReadLine());

            var stkData = new StkData
            {
                BusinessShortCode = 174379,
                TransactionType = "CustomerPayBillOnline",
                Amount = 1,
                PartyA = receiver,
                PartyB = 174379,
                PhoneNumber = receiver,
                CallBackUrl = new Uri("https://mydomain.com/path"),
                AccountReference = "CompanyXLTD",
                TransactionDesc = "Payment of X"
            };

            stkData.Password = darajaGateway.GetStkPushPassword(stkData.BusinessShortCode, stkData.Timestamp!);

            var pushResponse = await darajaClient.SendStkPushAsync(stkData);

            Console.WriteLine(JsonSerializer.Serialize(pushResponse, s_jsonOptions));
            if (pushResponse is null) { return; }

            var isCompleted = false;
            PushQueryResponse? pushQueryResponse = new PushQueryResponse();

            while (!isCompleted)
            {
                (isCompleted, pushQueryResponse) = await darajaClient.QueryStkPushStatus(pushResponse, stkData);
                Console.WriteLine("The transaction is being processed");
            }

            Console.WriteLine(JsonSerializer.Serialize(pushQueryResponse, s_jsonOptions));
        }
    }
}
