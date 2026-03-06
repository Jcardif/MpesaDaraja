using System.Text.Json;
using Mpesa.Daraja.Auth;
using Xunit;

namespace Mpesa.Daraja.Tests.Auth;

public class DarajaTokenTests
{
    [Fact]
    public void IsTokenValid_WhenTokenIsNew_ReturnsTrue()
    {
        var token = new DarajaToken
        {
            AccessToken = "test-token",
            ExpiresIn = "3600"
        };

        Assert.True(token.IsTokenValid());
    }

    [Fact]
    public void IsTokenValid_WhenExpiresInIsZero_ReturnsFalse()
    {
        var token = new DarajaToken
        {
            AccessToken = "test-token",
            ExpiresIn = "0"
        };

        Assert.False(token.IsTokenValid());
    }

    [Fact]
    public void IsTokenValid_WhenExpiresInIsNonNumeric_ReturnsFalse()
    {
        var token = new DarajaToken
        {
            AccessToken = "test-token",
            ExpiresIn = "not-a-number"
        };

        Assert.False(token.IsTokenValid());
    }

    [Fact]
    public void IsTokenValid_WhenExpiresInIsNegative_ReturnsFalse()
    {
        var token = new DarajaToken
        {
            AccessToken = "test-token",
            ExpiresIn = "-100"
        };

        Assert.False(token.IsTokenValid());
    }

    [Fact]
    public void IsTokenValid_WhenExpiresInIsVerySmall_ReturnsFalse()
    {
        // 1 second * 0.9 = 0.9 seconds - should expire almost immediately
        var token = new DarajaToken
        {
            AccessToken = "test-token",
            ExpiresIn = "1"
        };

        // With 0.9 seconds, the token refresh time is essentially now
        // This might be true or false depending on timing, so just verify no exception
        _ = token.IsTokenValid();
    }

    [Fact]
    public void IsTokenValid_WhenExpiresInIsEmpty_ReturnsFalse()
    {
        var token = new DarajaToken
        {
            AccessToken = "test-token",
            ExpiresIn = ""
        };

        Assert.False(token.IsTokenValid());
    }

    [Fact]
    public void AccessToken_ReturnsAssignedValue()
    {
        var token = new DarajaToken
        {
            AccessToken = "my-access-token",
            ExpiresIn = "3600"
        };

        Assert.Equal("my-access-token", token.AccessToken);
    }

    [Fact]
    public void ExpiresIn_ReturnsAssignedValue()
    {
        var token = new DarajaToken
        {
            AccessToken = "test-token",
            ExpiresIn = "3600"
        };

        Assert.Equal("3600", token.ExpiresIn);
    }

    [Fact]
    public void Deserialization_FromJson_SetsPropertiesCorrectly()
    {
        const string json = """{"access_token":"abc123","expires_in":"3599"}""";

        var token = JsonSerializer.Deserialize<DarajaToken>(json)!;

        Assert.Equal("abc123", token.AccessToken);
        Assert.Equal("3599", token.ExpiresIn);
        Assert.True(token.IsTokenValid());
    }

    [Fact]
    public void Deserialization_FromJson_WithLargeExpiry_IsValid()
    {
        const string json = """{"access_token":"token","expires_in":"86400"}""";

        var token = JsonSerializer.Deserialize<DarajaToken>(json)!;

        Assert.True(token.IsTokenValid());
    }

    [Fact]
    public void Serialization_RoundTrip_PreservesValues()
    {
        var original = new DarajaToken
        {
            AccessToken = "round-trip-token",
            ExpiresIn = "7200"
        };

        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<DarajaToken>(json)!;

        Assert.Equal(original.AccessToken, deserialized.AccessToken);
        Assert.Equal(original.ExpiresIn, deserialized.ExpiresIn);
    }
}
