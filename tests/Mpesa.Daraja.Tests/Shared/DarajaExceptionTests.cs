using Mpesa.Daraja.Shared;
using Mpesa.Daraja.Shared.Exceptions;

namespace Mpesa.Daraja.Tests.Shared;

public class DarajaExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_SetsMessage()
    {
        var exception = new DarajaException("something went wrong");

        Assert.Equal("something went wrong", exception.Message);
    }

    [Fact]
    public void Constructor_WithMessage_ErrorIsNull()
    {
        var exception = new DarajaException("error");

        Assert.Null(exception.Error);
    }

    [Fact]
    public void Constructor_WithMessageAndError_SetsMessage()
    {
        var error = new DarajaError
        {
            RequestId = "req-1",
            ErrorCode = "500",
            ErrorMessage = "Server error"
        };

        var exception = new DarajaException("API failed", error);

        Assert.Equal("API failed", exception.Message);
    }

    [Fact]
    public void Constructor_WithMessageAndError_SetsError()
    {
        var error = new DarajaError
        {
            RequestId = "req-1",
            ErrorCode = "401",
            ErrorMessage = "Unauthorized"
        };

        var exception = new DarajaException("Auth failed", error);

        Assert.NotNull(exception.Error);
        Assert.Equal("req-1", exception.Error.RequestId);
        Assert.Equal("401", exception.Error.ErrorCode);
        Assert.Equal("Unauthorized", exception.Error.ErrorMessage);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_SetsInnerException()
    {
        var inner = new InvalidOperationException("inner error");
        var exception = new DarajaException("outer error", inner);

        Assert.Equal("outer error", exception.Message);
        Assert.Same(inner, exception.InnerException);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_ErrorIsNull()
    {
        var inner = new Exception("inner");
        var exception = new DarajaException("outer", inner);

        Assert.Null(exception.Error);
    }

    [Fact]
    public void IsException_DerivesFromException()
    {
        var exception = new DarajaException("test");

        Assert.IsAssignableFrom<Exception>(exception);
    }

    [Fact]
    public void CanBeCaughtAsException()
    {
        DarajaError? caughtError = null;

        try
        {
            throw new DarajaException("test", new DarajaError
            {
                RequestId = "req-1",
                ErrorCode = "500",
                ErrorMessage = "error"
            });
        }
        catch (DarajaException ex)
        {
            caughtError = ex.Error;
        }

        Assert.NotNull(caughtError);
        Assert.Equal("500", caughtError.ErrorCode);
    }
}
