using Mpesa.Daraja.Services;

namespace Mpesa.Daraja.Tests;

public class DarajaGatewayTests
{
    [Fact]
    public void GetStkPushPassword_ReturnsBase64EncodedString()
    {
        // Arrange
        var gateway = new DarajaGateway("key", "secret", "passkey123", false);
        long shortCode = 174379;
        var timestamp = "20230101120000";

        // Act
        var password = gateway.GetStkPushPassword(shortCode, timestamp);

        // Assert
        Assert.NotNull(password);
        Assert.NotEmpty(password);

        // Verify it's valid base64
        var decoded = Convert.FromBase64String(password);
        var decodedString = System.Text.Encoding.UTF8.GetString(decoded);
        Assert.Equal($"{shortCode}passkey123{timestamp}", decodedString);
    }

    [Fact]
    public void GetStkPushPassword_DifferentTimestamps_ProduceDifferentPasswords()
    {
        // Arrange
        var gateway = new DarajaGateway("key", "secret", "passkey123", false);
        long shortCode = 174379;

        // Act
        var password1 = gateway.GetStkPushPassword(shortCode, "20230101120000");
        var password2 = gateway.GetStkPushPassword(shortCode, "20230101130000");

        // Assert
        Assert.NotEqual(password1, password2);
    }
}
