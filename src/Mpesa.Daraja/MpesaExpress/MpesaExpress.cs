using System.Text;
using System.Text.Json;
using Mpesa.Daraja.Auth;
using Mpesa.Daraja.Shared;

namespace Mpesa.Daraja;

/// <inheritdoc />
public class MpesaExpress : IMpesaExpress
{
    private readonly DarajaGateway _gateway;
    private readonly string _passKey;

    /// <summary>
    ///     Initializes a new instance of the <see cref="MpesaExpress"/> class.
    /// </summary>
    /// <param name="gateway"></param>
    /// <param name="passKey"></param>
    public MpesaExpress(DarajaGateway gateway , string passKey)
    {
        _gateway = gateway;
        _passKey = passKey;
    }


    /// <inheritdoc />
    public async Task<DarajaResult<MpesaExpressResponse>> InitiateStkPush(MpesaExpressPayload payload)
    {
        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        var inputPassword = $"{payload.BusinessShortCode}{_passKey}{timestamp}";
        var password = Convert.ToBase64String(Encoding.UTF8.GetBytes(inputPassword));

        payload.Timestamp = timestamp;
        payload.Password = password;

        var jsonPayload = JsonSerializer.Serialize(payload);
        var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        await _gateway.EnsureAuthenticatedAsync();

        var response = await _gateway.HttpClient.PostAsync("mpesa/stkpush/v1/processrequest", httpContent);
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var mpesaExpressResponse = JsonSerializer.Deserialize<MpesaExpressResponse>(content)!;
            return DarajaResult<MpesaExpressResponse>.Success(mpesaExpressResponse);
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

            return DarajaResult<MpesaExpressResponse>.Failure(errorResponse);
        }

        try
        {
            var darajaError = JsonSerializer.Deserialize<DarajaError>(content)!;
            return DarajaResult<MpesaExpressResponse>.Failure(darajaError);
        }
        catch (JsonException)
        {
            return DarajaResult<MpesaExpressResponse>.Failure(new DarajaError
            {
                RequestId = null,
                ErrorCode = response.StatusCode.ToString(),
                ErrorMessage = content
            });
        }

    }

    /// <inheritdoc />
    public async Task<DarajaResult<MpesaExpressQueryResponse>> QueryStkPushStatus(long businessShortCode,
        string checkoutRequestId)
    {
        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        var passwordInput = $"{businessShortCode}{_passKey}{timestamp}";
        var password = Convert.ToBase64String(Encoding.UTF8.GetBytes(passwordInput));

        var payload = new MpesaExpressQueryPayload
        {
            BusinessShortCode = businessShortCode,
            Password = password,
            Timestamp = timestamp,
            CheckoutRequestID = checkoutRequestId
        };

        var jsonPayload = JsonSerializer.Serialize(payload);
        var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        await _gateway.EnsureAuthenticatedAsync();
        var response = await _gateway.HttpClient.PostAsync("mpesa/stkpushquery/v1/query", httpContent);
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var queryResponse = JsonSerializer.Deserialize<MpesaExpressQueryResponse>(content)!;
            return DarajaResult<MpesaExpressQueryResponse>.Success(queryResponse);
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

            return DarajaResult<MpesaExpressQueryResponse>.Failure(errorResponse);
        }

        try
        {
            var darajaError = JsonSerializer.Deserialize<DarajaError>(content)!;
            return DarajaResult<MpesaExpressQueryResponse>.Failure(darajaError);
        }
        catch (JsonException)
        {
            return DarajaResult<MpesaExpressQueryResponse>.Failure(new DarajaError
            {
                RequestId = null,
                ErrorCode = response.StatusCode.ToString(),
                ErrorMessage = content
            });
        }

    }
}

