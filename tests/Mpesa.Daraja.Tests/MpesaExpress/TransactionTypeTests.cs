using System.Text.Json;

namespace Mpesa.Daraja.Tests.MpesaExpress;

public class TransactionTypeTests
{
    [Fact]
    public void CustomerPayBillOnline_HasExpectedValue()
    {
        Assert.Equal(0, (int)TransactionType.CustomerPayBillOnline);
    }

    [Fact]
    public void CustomerBuyGoodsOnline_HasExpectedValue()
    {
        Assert.Equal(1, (int)TransactionType.CustomerBuyGoodsOnline);
    }

    [Fact]
    public void EnumHasExactlyTwoValues()
    {
        var values = Enum.GetValues<TransactionType>();
        Assert.Equal(2, values.Length);
    }

    [Fact]
    public void Serialization_CustomerPayBillOnline_SerializesAsString()
    {
        var json = JsonSerializer.Serialize(TransactionType.CustomerPayBillOnline);

        Assert.Equal("\"CustomerPayBillOnline\"", json);
    }

    [Fact]
    public void Serialization_CustomerBuyGoodsOnline_SerializesAsString()
    {
        var json = JsonSerializer.Serialize(TransactionType.CustomerBuyGoodsOnline);

        Assert.Equal("\"CustomerBuyGoodsOnline\"", json);
    }

    [Fact]
    public void Deserialization_FromString_CustomerPayBillOnline()
    {
        var result = JsonSerializer.Deserialize<TransactionType>("\"CustomerPayBillOnline\"");

        Assert.Equal(TransactionType.CustomerPayBillOnline, result);
    }

    [Fact]
    public void Deserialization_FromString_CustomerBuyGoodsOnline()
    {
        var result = JsonSerializer.Deserialize<TransactionType>("\"CustomerBuyGoodsOnline\"");

        Assert.Equal(TransactionType.CustomerBuyGoodsOnline, result);
    }

    [Fact]
    public void Serialization_RoundTrip_PayBill()
    {
        var original = TransactionType.CustomerPayBillOnline;
        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<TransactionType>(json);

        Assert.Equal(original, deserialized);
    }

    [Fact]
    public void Serialization_RoundTrip_BuyGoods()
    {
        var original = TransactionType.CustomerBuyGoodsOnline;
        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<TransactionType>(json);

        Assert.Equal(original, deserialized);
    }
}
