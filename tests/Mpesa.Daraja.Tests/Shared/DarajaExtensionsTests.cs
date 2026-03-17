using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Mpesa.Daraja.Auth;
using Mpesa.Daraja.Shared;
using Mpesa.Daraja.Tests.Testing;

namespace Mpesa.Daraja.Tests.Shared;

public class DarajaExtensionsTests
{
    [Fact]
    public async Task EnsureAuthenticatedAsync_WhenTokenIsMissing_InitializesGateway()
    {
        var handler = new RecordingHttpMessageHandler((request, _) =>
        {
            Assert.Equal(HttpMethod.Get, request.Method);
            Assert.Equal("Basic", request.Headers.Authorization?.Scheme);

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(new { access_token = "test-access-token", expires_in = "3600" }),
                    Encoding.UTF8,
                    "application/json")
            });
        });
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = DarajaGateway.CreateBaseAddress(false)
        };
        using var gateway = new DarajaGateway(
            new SingleClientHttpClientFactory(httpClient),
            Options.Create(new DarajaOptions
            {
                ConsumerKey = "key",
                ConsumerSecret = "secret"
            }));

        await gateway.EnsureAuthenticatedAsync();

        var request = Assert.Single(handler.Requests);
        Assert.Equal("Basic", request.Authorization?.Scheme);
        Assert.Equal(Convert.ToBase64String(Encoding.UTF8.GetBytes("key:secret")), request.Authorization?.Parameter);
    }
}
