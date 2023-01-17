using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
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

            var stkData = new StkData();

            stkData.BusinessShortCode = 174379;
            
            stkData.Timestamp = "20230116043457";
            stkData.TransactionType = "CustomerPayBillOnline";
            stkData.Amount = 1;

            Console.WriteLine("Receiver for the stk push");
            var receiver= Convert.ToInt64(Console.ReadLine());

            stkData.PartyA = receiver;
            stkData.PartyB = 174379;

            stkData.PhoneNumber = receiver;
            stkData.CallBackUrl = new Uri("https://mydomain.com/path");
            stkData.AccountReference = "CompanyXLTD";
            stkData.TransactionDesc = "Payment of X";

            stkData.Password = darajaGateway.GetStkPushPassword(stkData.BusinessShortCode, stkData.Timestamp);

            var result = await darajaClient.SendStkPushAsync(stkData);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        }
    }
}