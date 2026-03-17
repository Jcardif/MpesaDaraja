using System.Net.Http;

namespace Mpesa.Daraja.Tests.Testing;

internal sealed class SingleClientHttpClientFactory(HttpClient client) : IHttpClientFactory
{
    public HttpClient CreateClient(string name) => client;
}
