using Mpesa.Daraja.Shared;

namespace Mpesa.Daraja.Tests.Shared;

public class DarajaResultTests
{
    [Fact]
    public void Success_IsSuccessIsTrue()
    {
        var result = DarajaResult<string>.Success("value");

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Success_ValueIsSet()
    {
        var result = DarajaResult<string>.Success("test-value");

        Assert.Equal("test-value", result.Value);
    }

    [Fact]
    public void Success_ErrorIsNull()
    {
        var result = DarajaResult<string>.Success("value");

        Assert.Null(result.Error);
    }

    [Fact]
    public void Failure_IsSuccessIsFalse()
    {
        var error = new DarajaError
        {
            RequestId = "req-1",
            ErrorCode = "500",
            ErrorMessage = "Server error"
        };

        var result = DarajaResult<string>.Failure(error);

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void Failure_ErrorIsSet()
    {
        var error = new DarajaError
        {
            RequestId = "req-1",
            ErrorCode = "400",
            ErrorMessage = "Bad request"
        };

        var result = DarajaResult<string>.Failure(error);

        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void Failure_ValueIsDefault()
    {
        var error = new DarajaError
        {
            RequestId = null,
            ErrorCode = "400",
            ErrorMessage = "Bad request"
        };

        var result = DarajaResult<string>.Failure(error);

        Assert.Null(result.Value);
    }

    [Fact]
    public void Failure_ValueIsDefaultForValueType()
    {
        var error = new DarajaError
        {
            RequestId = null,
            ErrorCode = "400",
            ErrorMessage = "Bad request"
        };

        var result = DarajaResult<int>.Failure(error);

        Assert.Equal(0, result.Value);
    }

    [Fact]
    public void Success_WithComplexType_ValueIsSet()
    {
        var response = new MpesaExpressResponse
        {
            MerchantRequestID = "123",
            CheckoutRequestID = "456",
            ResponseCode = "0",
            ResponseDescription = "Success",
            CustomerMessage = "Accepted"
        };

        var result = DarajaResult<MpesaExpressResponse>.Success(response);

        Assert.True(result.IsSuccess);
        Assert.Equal("123", result.Value!.MerchantRequestID);
        Assert.Equal("456", result.Value.CheckoutRequestID);
    }

    [Fact]
    public void Failure_ErrorPreservesAllFields()
    {
        var error = new DarajaError
        {
            RequestId = "req-abc",
            ErrorCode = "404.001",
            ErrorMessage = "Resource not found"
        };

        var result = DarajaResult<MpesaExpressResponse>.Failure(error);

        Assert.Equal("req-abc", result.Error!.RequestId);
        Assert.Equal("404.001", result.Error.ErrorCode);
        Assert.Equal("Resource not found", result.Error.ErrorMessage);
    }

    [Fact]
    public void Failure_WithNullRequestId_IsAllowed()
    {
        var error = new DarajaError
        {
            RequestId = null,
            ErrorCode = "500",
            ErrorMessage = "Internal error"
        };

        var result = DarajaResult<string>.Failure(error);

        Assert.Null(result.Error!.RequestId);
    }
}
