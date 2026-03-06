using System.Text.Json;

namespace Mpesa.Daraja.Tests.Reversal;

public class ReversalResponseTests
{
    [Fact]
    public void Deserialization_FromApiJson_SetsAllProperties()
    {
        const string json = """
            {
                "OriginatorConversationId": "AG_20210101_0000111222333",
                "ConversationId": "AG_20210101_0000444555666",
                "ResponseCode": 0,
                "ResponseDescription": "Accept the service request successfully."
            }
            """;

        var response = JsonSerializer.Deserialize<ReversalResponse>(json)!;

        Assert.Equal("AG_20210101_0000111222333", response.OriginatorConversationId);
        Assert.Equal("AG_20210101_0000444555666", response.ConversationId);
        Assert.Equal(0, response.ResponseCode);
        Assert.Equal("Accept the service request successfully.", response.ResponseDescription);
    }

    [Fact]
    public void Serialization_RoundTrip_PreservesValues()
    {
        var original = new ReversalResponse
        {
            OriginatorConversationId = "orig-123",
            ConversationId = "conv-456",
            ResponseCode = 0,
            ResponseDescription = "Success"
        };

        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<ReversalResponse>(json)!;

        Assert.Equal(original.OriginatorConversationId, deserialized.OriginatorConversationId);
        Assert.Equal(original.ConversationId, deserialized.ConversationId);
        Assert.Equal(original.ResponseCode, deserialized.ResponseCode);
        Assert.Equal(original.ResponseDescription, deserialized.ResponseDescription);
    }

    [Fact]
    public void Properties_CanBeSetAndRead()
    {
        var response = new ReversalResponse
        {
            OriginatorConversationId = "orig-1",
            ConversationId = "conv-1",
            ResponseCode = 1,
            ResponseDescription = "Error occurred"
        };

        Assert.Equal("orig-1", response.OriginatorConversationId);
        Assert.Equal("conv-1", response.ConversationId);
        Assert.Equal(1, response.ResponseCode);
        Assert.Equal("Error occurred", response.ResponseDescription);
    }

    [Fact]
    public void ResponseCode_Zero_IndicatesSuccess()
    {
        var response = new ReversalResponse
        {
            OriginatorConversationId = "orig-1",
            ConversationId = "conv-1",
            ResponseCode = 0,
            ResponseDescription = "Accept the service request successfully."
        };

        Assert.Equal(0, response.ResponseCode);
    }

    [Fact]
    public void ResponseCode_NonZero_IndicatesError()
    {
        var response = new ReversalResponse
        {
            OriginatorConversationId = "orig-1",
            ConversationId = "conv-1",
            ResponseCode = 1,
            ResponseDescription = "Rejected"
        };

        Assert.NotEqual(0, response.ResponseCode);
    }
}
