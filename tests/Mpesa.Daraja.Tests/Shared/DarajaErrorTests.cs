using System.Text.Json;
using Mpesa.Daraja.Shared;

namespace Mpesa.Daraja.Tests.Shared;

public class DarajaErrorTests
{
    [Fact]
    public void Deserialization_FromJson_SetsAllProperties()
    {
        const string json = """
            {
                "requestId": "abc-123",
                "errorCode": "500.001.1001",
                "errorMessage": "Invalid credentials"
            }
            """;

        var error = JsonSerializer.Deserialize<DarajaError>(json)!;

        Assert.Equal("abc-123", error.RequestId);
        Assert.Equal("500.001.1001", error.ErrorCode);
        Assert.Equal("Invalid credentials", error.ErrorMessage);
    }

    [Fact]
    public void Deserialization_WithNullRequestId_SetsNull()
    {
        const string json = """
            {
                "requestId": null,
                "errorCode": "400",
                "errorMessage": "Bad request"
            }
            """;

        var error = JsonSerializer.Deserialize<DarajaError>(json)!;

        Assert.Null(error.RequestId);
    }

    [Fact]
    public void Serialization_ProducesCorrectJsonPropertyNames()
    {
        var error = new DarajaError
        {
            RequestId = "req-1",
            ErrorCode = "500",
            ErrorMessage = "Server error"
        };

        var json = JsonSerializer.Serialize(error);

        Assert.Contains("\"requestId\"", json);
        Assert.Contains("\"errorCode\"", json);
        Assert.Contains("\"errorMessage\"", json);
    }

    [Fact]
    public void Serialization_DoesNotUsePascalCase()
    {
        var error = new DarajaError
        {
            RequestId = "req-1",
            ErrorCode = "500",
            ErrorMessage = "Server error"
        };

        var json = JsonSerializer.Serialize(error);

        Assert.DoesNotContain("\"RequestId\"", json);
        Assert.DoesNotContain("\"ErrorCode\"", json);
        Assert.DoesNotContain("\"ErrorMessage\"", json);
    }

    [Fact]
    public void Serialization_RoundTrip_PreservesValues()
    {
        var original = new DarajaError
        {
            RequestId = "req-round-trip",
            ErrorCode = "404.001",
            ErrorMessage = "Not found"
        };

        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<DarajaError>(json)!;

        Assert.Equal(original.RequestId, deserialized.RequestId);
        Assert.Equal(original.ErrorCode, deserialized.ErrorCode);
        Assert.Equal(original.ErrorMessage, deserialized.ErrorMessage);
    }
}
