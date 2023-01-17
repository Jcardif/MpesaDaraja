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

            var gateway = new DarajaGateway(endpoint, consumerKey, consumerSecret);

            var darajaClient = await gateway.GetDarajaClientAsync();

            if (darajaClient != null)
                await MakeStkPush(darajaClient);
        }

        private static async Task MakeStkPush(DarajaClient darajaClient)
        {

            var stkData = new StkData();



            var result = await darajaClient.SendSTKPushAsync(stkData);

            Console.WriteLine(result);
        }
    }
}