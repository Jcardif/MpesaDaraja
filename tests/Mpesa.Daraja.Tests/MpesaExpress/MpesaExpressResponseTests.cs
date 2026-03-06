using System.Text.Json;

namespace Mpesa.Daraja.Tests.MpesaExpress;

public class MpesaExpressResponseTests
{
    [Fact]
    public void Deserialization_FromApiJson_SetsAllProperties()
    {
        const string json = """
            {
                "MerchantRequestID": "29115-34620561-1",
                "CheckoutRequestID": "ws_CO_191220191020363925",
                "ResponseCode": "0",
                "ResponseDescription": "Success. Request accepted for processing",
                "CustomerMessage": "Success. Request accepted for processing"
            }
            """;

        var response = JsonSerializer.Deserialize<MpesaExpressResponse>(json)!;

        Assert.Equal("29115-34620561-1", response.MerchantRequestID);
        Assert.Equal("ws_CO_191220191020363925", response.CheckoutRequestID);
        Assert.Equal("0", response.ResponseCode);
        Assert.Equal("Success. Request accepted for processing", response.ResponseDescription);
        Assert.Equal("Success. Request accepted for processing", response.CustomerMessage);
    }

    [Fact]
    public void Serialization_RoundTrip_PreservesValues()
    {
        var original = new MpesaExpressResponse
        {
            MerchantRequestID = "merchant-1",
            CheckoutRequestID = "checkout-1",
            ResponseCode = "0",
            ResponseDescription = "Success",
            CustomerMessage = "Accepted"
        };

        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<MpesaExpressResponse>(json)!;

        Assert.Equal(original.MerchantRequestID, deserialized.MerchantRequestID);
        Assert.Equal(original.CheckoutRequestID, deserialized.CheckoutRequestID);
        Assert.Equal(original.ResponseCode, deserialized.ResponseCode);
        Assert.Equal(original.ResponseDescription, deserialized.ResponseDescription);
        Assert.Equal(original.CustomerMessage, deserialized.CustomerMessage);
    }

    [Fact]
    public void Properties_CanBeSetAndRead()
    {
        var response = new MpesaExpressResponse
        {
            MerchantRequestID = "m-123",
            CheckoutRequestID = "c-456",
            ResponseCode = "1",
            ResponseDescription = "Failed",
            CustomerMessage = "Error occurred"
        };

        Assert.Equal("m-123", response.MerchantRequestID);
        Assert.Equal("c-456", response.CheckoutRequestID);
        Assert.Equal("1", response.ResponseCode);
        Assert.Equal("Failed", response.ResponseDescription);
        Assert.Equal("Error occurred", response.CustomerMessage);
    }

    [Fact]
    public void Deserialization_WithMissingFields_SetsNulls()
    {
        const string json = """{"MerchantRequestID": "123"}""";

        var response = JsonSerializer.Deserialize<MpesaExpressResponse>(json)!;

        Assert.Equal("123", response.MerchantRequestID);
        Assert.Null(response.CheckoutRequestID);
        Assert.Null(response.ResponseCode);
        Assert.Null(response.ResponseDescription);
        Assert.Null(response.CustomerMessage);
    }
}
