using Microsoft.Extensions.Configuration;
using MpesaDaraja.Services;

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

            var auth = new DarajaGateway(endpoint, consumerKey, consumerSecret);

            var token = await auth.GetDarajaClientAsync();

            Console.WriteLine(token.AccessToken);

        }
    }

 
}