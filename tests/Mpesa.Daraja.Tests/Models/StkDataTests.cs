using Mpesa.Daraja.Models;

namespace Mpesa.Daraja.Tests.Models;

public class StkDataTests
{
    [Fact]
    public void Constructor_SetsTimestamp()
    {
        // Act
        var stkData = new StkData();

        // Assert
        Assert.NotNull(stkData.Timestamp);
        Assert.Equal(14, stkData.Timestamp.Length); // yyyyMMddHHmmss = 14 chars
    }

    [Fact]
    public void Properties_CanBeSetAndRetrieved()
    {
        // Arrange & Act
        var stkData = new StkData
        {
            BusinessShortCode = 174379,
            TransactionType = "CustomerPayBillOnline",
            Amount = 100,
            PartyA = 254712345678,
            PartyB = 174379,
            PhoneNumber = 254712345678,
            CallBackUrl = new Uri("https://example.com/callback"),
            AccountReference = "TestRef",
            TransactionDesc = "Test Payment"
        };

        // Assert
        Assert.Equal(174379, stkData.BusinessShortCode);
        Assert.Equal("CustomerPayBillOnline", stkData.TransactionType);
        Assert.Equal(100, stkData.Amount);
        Assert.Equal(254712345678, stkData.PartyA);
        Assert.Equal(174379, stkData.PartyB);
        Assert.Equal(254712345678, stkData.PhoneNumber);
        Assert.Equal("https://example.com/callback", stkData.CallBackUrl?.ToString());
        Assert.Equal("TestRef", stkData.AccountReference);
        Assert.Equal("Test Payment", stkData.TransactionDesc);
    }
}
