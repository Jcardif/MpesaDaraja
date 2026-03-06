using System.Text.Json;

namespace Mpesa.Daraja.Tests.Reversal;

public class ReversalPayloadTests
{
    private static ReversalPayload CreateValidPayload() => new()
    {
        Initiator = "testapi",
        TransactionId = "OEI2AK4Q16",
        Amount = 100,
        ReceiverParty = 600978,
        ResultUrl = new Uri("https://example.com/result"),
        QueueTimeOutUrl = new Uri("https://example.com/timeout"),
        Remarks = "Test reversal"
    };

    [Fact]
    public void Properties_AreSetCorrectly()
    {
        var payload = CreateValidPayload();

        Assert.Equal("testapi", payload.Initiator);
        Assert.Equal("OEI2AK4Q16", payload.TransactionId);
        Assert.Equal(100, payload.Amount);
        Assert.Equal(600978, payload.ReceiverParty);
        Assert.Equal(new Uri("https://example.com/result"), payload.ResultUrl);
        Assert.Equal(new Uri("https://example.com/timeout"), payload.QueueTimeOutUrl);
        Assert.Equal("Test reversal", payload.Remarks);
    }

    [Fact]
    public void CommandId_IsAlwaysTransactionReversal()
    {
        var payload = CreateValidPayload();

        Assert.Equal("TransactionReversal", payload.CommandId);
    }

    [Fact]
    public void RecieverIdentifierType_DefaultsTo11()
    {
        var payload = CreateValidPayload();

        Assert.Equal(11, payload.RecieverIdentifierType);
    }

    [Fact]
    public void SecurityCredential_DefaultIsNull()
    {
        var payload = CreateValidPayload();

        Assert.Null(payload.SecurityCredential);
    }

    [Fact]
    public void Serialization_IncludesAllProperties()
    {
        var payload = CreateValidPayload();

        var json = JsonSerializer.Serialize(payload);

        Assert.Contains("\"Initiator\"", json);
        Assert.Contains("\"CommandId\"", json);
        Assert.Contains("\"TransactionId\"", json);
        Assert.Contains("\"Amount\"", json);
        Assert.Contains("\"ReceiverParty\"", json);
        Assert.Contains("\"RecieverIdentifierType\"", json);
        Assert.Contains("\"ResultUrl\"", json);
        Assert.Contains("\"QueueTimeOutUrl\"", json);
        Assert.Contains("\"Remarks\"", json);
    }

    [Fact]
    public void Serialization_CommandId_IsTransactionReversal()
    {
        var payload = CreateValidPayload();

        var json = JsonSerializer.Serialize(payload);

        Assert.Contains("\"TransactionReversal\"", json);
    }

    [Fact]
    public void Serialization_RecieverIdentifierType_Is11()
    {
        var payload = CreateValidPayload();

        var json = JsonSerializer.Serialize(payload);

        Assert.Contains("\"RecieverIdentifierType\":11", json);
    }

    [Fact]
    public void Deserialization_RoundTrip_PreservesValues()
    {
        var original = CreateValidPayload();

        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<ReversalPayload>(json)!;

        Assert.Equal(original.Initiator, deserialized.Initiator);
        Assert.Equal(original.TransactionId, deserialized.TransactionId);
        Assert.Equal(original.Amount, deserialized.Amount);
        Assert.Equal(original.ReceiverParty, deserialized.ReceiverParty);
        Assert.Equal(original.ResultUrl, deserialized.ResultUrl);
        Assert.Equal(original.QueueTimeOutUrl, deserialized.QueueTimeOutUrl);
        Assert.Equal(original.Remarks, deserialized.Remarks);
    }

    [Fact]
    public void Amount_SupportsDecimalValues()
    {
        var payload = CreateValidPayload();
        payload.Amount = 1500.50m;

        Assert.Equal(1500.50m, payload.Amount);
    }
}
