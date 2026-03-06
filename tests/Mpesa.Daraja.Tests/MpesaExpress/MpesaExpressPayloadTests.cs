using System.Text.Json;

namespace Mpesa.Daraja.Tests.MpesaExpress;

public class MpesaExpressPayloadTests
{
    private static MpesaExpressPayload CreateValidPayload() => new()
    {
        BusinessShortCode = 174379,
        Passkey = "bfb279f9aa9bdbcf158e97dd71a467cd2e0c893059b10f78e6b72ada1ed2c919",
        TransactionType = TransactionType.CustomerPayBillOnline,
        Amount = 1,
        PartyA = "254708374149",
        PartyB = "174379",
        PhoneNumber = "254708374149",
        CallBackURL = "https://example.com/callback",
        AccountReference = "TestRef",
        TransactionDesc = "Test Payment"
    };

    [Fact]
    public void Properties_AreSetCorrectly()
    {
        var payload = CreateValidPayload();

        Assert.Equal(174379, payload.BusinessShortCode);
        Assert.Equal("bfb279f9aa9bdbcf158e97dd71a467cd2e0c893059b10f78e6b72ada1ed2c919", payload.Passkey);
        Assert.Equal(TransactionType.CustomerPayBillOnline, payload.TransactionType);
        Assert.Equal(1, payload.Amount);
        Assert.Equal("254708374149", payload.PartyA);
        Assert.Equal("174379", payload.PartyB);
        Assert.Equal("254708374149", payload.PhoneNumber);
        Assert.Equal("https://example.com/callback", payload.CallBackURL);
        Assert.Equal("TestRef", payload.AccountReference);
        Assert.Equal("Test Payment", payload.TransactionDesc);
    }

    [Fact]
    public void Password_DefaultIsNull()
    {
        var payload = CreateValidPayload();

        Assert.Null(payload.Password);
    }

    [Fact]
    public void Timestamp_DefaultIsNull()
    {
        var payload = CreateValidPayload();

        Assert.Null(payload.Timestamp);
    }

    [Fact]
    public void Serialization_IncludesRequiredProperties()
    {
        var payload = CreateValidPayload();

        var json = JsonSerializer.Serialize(payload);

        Assert.Contains("\"BusinessShortCode\"", json);
        Assert.Contains("\"Passkey\"", json);
        Assert.Contains("\"TransactionType\"", json);
        Assert.Contains("\"Amount\"", json);
        Assert.Contains("\"PartyA\"", json);
        Assert.Contains("\"PartyB\"", json);
        Assert.Contains("\"PhoneNumber\"", json);
        Assert.Contains("\"CallBackURL\"", json);
        Assert.Contains("\"AccountReference\"", json);
        Assert.Contains("\"TransactionDesc\"", json);
    }

    [Fact]
    public void TransactionType_CustomerPayBillOnline_SerializesAsString()
    {
        var payload = CreateValidPayload();
        payload.TransactionType = TransactionType.CustomerPayBillOnline;

        var json = JsonSerializer.Serialize(payload);

        Assert.Contains("\"CustomerPayBillOnline\"", json);
    }

    [Fact]
    public void TransactionType_CustomerBuyGoodsOnline_SerializesAsString()
    {
        var payload = CreateValidPayload();
        payload.TransactionType = TransactionType.CustomerBuyGoodsOnline;

        var json = JsonSerializer.Serialize(payload);

        Assert.Contains("\"CustomerBuyGoodsOnline\"", json);
    }

    [Fact]
    public void Amount_SupportsDecimalValues()
    {
        var payload = CreateValidPayload();
        payload.Amount = 99.99m;

        Assert.Equal(99.99m, payload.Amount);
    }

    [Fact]
    public void Deserialization_RoundTrip_PreservesRequiredProperties()
    {
        var original = CreateValidPayload();

        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<MpesaExpressPayload>(json)!;

        Assert.Equal(original.BusinessShortCode, deserialized.BusinessShortCode);
        Assert.Equal(original.Passkey, deserialized.Passkey);
        Assert.Equal(original.TransactionType, deserialized.TransactionType);
        Assert.Equal(original.Amount, deserialized.Amount);
        Assert.Equal(original.PartyA, deserialized.PartyA);
        Assert.Equal(original.PartyB, deserialized.PartyB);
        Assert.Equal(original.PhoneNumber, deserialized.PhoneNumber);
        Assert.Equal(original.CallBackURL, deserialized.CallBackURL);
        Assert.Equal(original.AccountReference, deserialized.AccountReference);
        Assert.Equal(original.TransactionDesc, deserialized.TransactionDesc);
    }

    [Fact]
    public void PasswordAndTimestamp_AreNullByDefault_SinceTheyAreSetInternally()
    {
        // Password and Timestamp have internal setters - they are set by MpesaExpress.InitiateStkPush
        var payload = CreateValidPayload();

        Assert.Null(payload.Password);
        Assert.Null(payload.Timestamp);
    }
}
