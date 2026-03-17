using System.Collections.Concurrent;
using System.Net.Http.Headers;

namespace Mpesa.Daraja.Tests.Testing;

internal sealed class RecordingHttpMessageHandler(
    Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> responder)
    : HttpMessageHandler
{
    private readonly ConcurrentQueue<RecordedRequest> _requests = new();

    public IReadOnlyCollection<RecordedRequest> Requests => _requests.ToArray();

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        _requests.Enqueue(await CreateSnapshotAsync(request, cancellationToken));
        return await responder(request, cancellationToken);
    }

    private static async Task<RecordedRequest> CreateSnapshotAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var content = request.Content is null
            ? null
            : await request.Content.ReadAsStringAsync(cancellationToken);

        return new RecordedRequest(
            request.Method,
            request.RequestUri,
            request.Headers.Authorization,
            content);
    }
}

internal sealed record RecordedRequest(
    HttpMethod Method,
    Uri? RequestUri,
    AuthenticationHeaderValue? Authorization,
    string? Content);
