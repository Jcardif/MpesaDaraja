using System.Text.Json;

namespace Mpesa.Daraja.Tests.MpesaExpress;

public class MpesaExpressQueryResponseTests
{
    [Fact]
    public void Deserialization_FromApiJson_SetsAllProperties()
    {
        const string json = """
            {
                "ResponseCode": "0",
                "ResponseDescription": "The service request has been accepted successfully",
                "MerchantRequestID": "22205-34066-1",
                "CheckoutRequestID": "ws_CO_13012021093521236",
                "ResultCode": "0",
                "ResultDesc": "The service request is processed successfully."
            }
            """;

        var response = JsonSerializer.Deserialize<MpesaExpressQueryResponse>(json)!;

        Assert.Equal("0", response.ResponseCode);
        Assert.Equal("The service request has been accepted successfully", response.ResponseDescription);
        Assert.Equal("22205-34066-1", response.MerchantRequestID);
        Assert.Equal("ws_CO_13012021093521236", response.CheckoutRequestID);
        Assert.Equal("0", response.ResultCode);
        Assert.Equal("The service request is processed successfully.", response.ResultDesc);
    }

    [Fact]
    public void Serialization_RoundTrip_PreservesValues()
    {
        var original = new MpesaExpressQueryResponse
        {
            ResponseCode = "0",
            ResponseDescription = "Accepted",
            MerchantRequestID = "m-1",
            CheckoutRequestID = "c-1",
            ResultCode = "0",
            ResultDesc = "Processed"
        };

        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<MpesaExpressQueryResponse>(json)!;

        Assert.Equal(original.ResponseCode, deserialized.ResponseCode);
        Assert.Equal(original.ResponseDescription, deserialized.ResponseDescription);
        Assert.Equal(original.MerchantRequestID, deserialized.MerchantRequestID);
        Assert.Equal(original.CheckoutRequestID, deserialized.CheckoutRequestID);
        Assert.Equal(original.ResultCode, deserialized.ResultCode);
        Assert.Equal(original.ResultDesc, deserialized.ResultDesc);
    }

    [Fact]
    public void Deserialization_FailedTransaction_SetsErrorResultCode()
    {
        const string json = """
            {
                "ResponseCode": "0",
                "ResponseDescription": "The service request has been accepted successfully",
                "MerchantRequestID": "22205-34066-1",
                "CheckoutRequestID": "ws_CO_13012021093521236",
                "ResultCode": "1032",
                "ResultDesc": "Request cancelled by user."
            }
            """;

        var response = JsonSerializer.Deserialize<MpesaExpressQueryResponse>(json)!;

        Assert.Equal("0", response.ResponseCode);
        Assert.Equal("1032", response.ResultCode);
        Assert.Equal("Request cancelled by user.", response.ResultDesc);
    }

    [Fact]
    public void Deserialization_WithMissingFields_SetsNulls()
    {
        const string json = """{"ResponseCode": "0"}""";

        var response = JsonSerializer.Deserialize<MpesaExpressQueryResponse>(json)!;

        Assert.Equal("0", response.ResponseCode);
        Assert.Null(response.ResponseDescription);
        Assert.Null(response.MerchantRequestID);
        Assert.Null(response.CheckoutRequestID);
        Assert.Null(response.ResultCode);
        Assert.Null(response.ResultDesc);
    }

    [Fact]
    public void Properties_CanBeSetAndRead()
    {
        var response = new MpesaExpressQueryResponse
        {
            ResponseCode = "1",
            ResponseDescription = "Error",
            MerchantRequestID = "m-err",
            CheckoutRequestID = "c-err",
            ResultCode = "1001",
            ResultDesc = "Insufficient balance"
        };

        Assert.Equal("1", response.ResponseCode);
        Assert.Equal("Error", response.ResponseDescription);
        Assert.Equal("m-err", response.MerchantRequestID);
        Assert.Equal("c-err", response.CheckoutRequestID);
        Assert.Equal("1001", response.ResultCode);
        Assert.Equal("Insufficient balance", response.ResultDesc);
    }
}
