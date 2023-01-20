using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using MpesaDaraja;
using MpesaDaraja.Models;
using MpesaDaraja.Services;
using Newtonsoft.Json;

namespace Daraja.App
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            var config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            var consumerKey = config["ConsumerKey"];
            var consumerSecret = config["ConsumerSecret"];
            var endpoint = config["EndPoint"];
            var grantType = config["GrantType"];
            var passKey = config["PassKey"];

            var gateway = new DarajaGateway(endpoint, consumerKey, consumerSecret, passKey);

            var darajaClient = await gateway.GetDarajaClientAsync();

            if (darajaClient != null)
                await MakeStkPush(gateway, darajaClient);
        }

        private static async Task MakeStkPush(DarajaGateway darajaGateway, DarajaClient darajaClient)
        {
            Console.WriteLine("Receiver for the stk push");
            var receiver = Convert.ToInt64(Console.ReadLine());


            var stkData = new StkData
            {
                BusinessShortCode = 174379,
                Timestamp = "20230116043457",
                TransactionType = "CustomerPayBillOnline",
                Amount = 1,
                PartyA = receiver,
                PartyB = 174379,
                PhoneNumber = receiver,
                CallBackUrl = new Uri("https://mydomain.com/path"),
                AccountReference = "CompanyXLTD",
                TransactionDesc = "Payment of X"
            };

            stkData.Password = darajaGateway.GetStkPushPassword(stkData.BusinessShortCode, stkData.Timestamp);

            var result = await darajaClient.SendStkPushAsync(stkData);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        }
    }
}