using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Mpesa.Daraja.Auth;
using Mpesa.Daraja.Shared;

namespace Mpesa.Daraja;

/// <summary>
///     Service for the reversal API enabling the reversal of Customer-to-Business (C2B) transactions
/// </summary>
public class Reversal
{
    private readonly DarajaGateway _gateway;
    private readonly string _initiatorPassword;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Reversal"/> class for dependency injection scenarios.
    /// </summary>
    /// <param name="gateway">The authenticated Daraja gateway used to send requests.</param>
    /// <param name="options">The Daraja configuration containing the initiator password.</param>
    public Reversal(DarajaGateway gateway, IOptions<DarajaOptions> options)
    {
        _gateway = gateway;
        _initiatorPassword = options.Value.InitiatorPassword;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Reversal"/> class with an explicit initiator password.
    /// </summary>
    /// <param name="gateway">The authenticated Daraja gateway used to send requests.</param>
    /// <param name="initiatorPassword">The initiator password used to generate the security credential.</param>
    public Reversal(DarajaGateway gateway, string initiatorPassword)
    {
        _gateway = gateway;
        _initiatorPassword = initiatorPassword;
    }

    /// <summary>
    ///     Reverse a C2B transaction
    /// </summary>
    /// <param name="payload"></param>
    /// <param name="cancellationToken">Cancels the reversal operation.</param>
    public async Task<DarajaResult<ReversalResponse>> ReverseTransactionAsync(
        ReversalPayload payload,
        CancellationToken cancellationToken = default)
    {
        var securityCredential = GenerateSecurityCredential();
        payload.SecurityCredential = securityCredential;

        var jsonPayload = JsonSerializer.Serialize(payload);
        var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        using var response = await _gateway.SendAuthenticatedRequestAsync(
            HttpMethod.Post,
            "mpesa/reversal/v1/request",
            httpContent,
            cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var reversalResponse = JsonSerializer.Deserialize<ReversalResponse>(content)!;
            return DarajaResult<ReversalResponse>.Success(reversalResponse);
        }

        if (string.IsNullOrEmpty(content))
        {
            var errorResponse = new DarajaError
            {
                RequestId = null,
                ErrorCode = "NotProvided",
                ErrorMessage =
                    $"The request failed with status code {response.StatusCode} and no content was provided in the response."
            };

            return DarajaResult<ReversalResponse>.Failure(errorResponse);
        }

        try
        {
            var darajaError = JsonSerializer.Deserialize<DarajaError>(content)!;
            return DarajaResult<ReversalResponse>.Failure(darajaError);
        }
        catch (JsonException)
        {
            var errorResponse = new DarajaError
            {
                RequestId = null,
                ErrorCode = "InvalidResponse",
                ErrorMessage =
                    $"The request failed with status code {response.StatusCode} and the response content could not be parsed."
            };

            return DarajaResult<ReversalResponse>.Failure(errorResponse);
        }


    }

    /// <summary>
    ///     Generates the security credential by encrypting the initiator password with the
    ///     M-Pesa public key certificate using RSA with PKCS#1 v1.5 padding.
    /// </summary>
    /// <returns>Base64-encoded encrypted password</returns>
    private string GenerateSecurityCredential()
    {
        var resourceName = _gateway.IsLive
            ? "Mpesa.Daraja.Assets.ProductionCertificate.cer"
            : "Mpesa.Daraja.Assets.SandboxCertificate.cer";

        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Embedded certificate resource '{resourceName}' not found.");

        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        var certBytes = memoryStream.ToArray();

#if NET9_0_OR_GREATER
        using var certificate = X509CertificateLoader.LoadCertificate(certBytes);
#else
        using var certificate = new X509Certificate2(certBytes);
#endif
        using var rsa = certificate.GetRSAPublicKey()
            ?? throw new InvalidOperationException("Certificate does not contain an RSA public key.");

        var passwordBytes = Encoding.UTF8.GetBytes(_initiatorPassword);
        var encryptedBytes = rsa.Encrypt(passwordBytes, RSAEncryptionPadding.Pkcs1);

        return Convert.ToBase64String(encryptedBytes);
    }
}
