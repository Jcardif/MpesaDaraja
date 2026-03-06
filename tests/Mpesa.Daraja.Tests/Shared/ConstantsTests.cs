using Mpesa.Daraja.Shared;

namespace Mpesa.Daraja.Tests.Shared;

public class ConstantsTests
{
    [Fact]
    public void SandboxBaseUrl_IsCorrectSafaricomUrl()
    {
        Assert.Equal("https://sandbox.safaricom.co.ke/", Constants.SANDBOX_BASE_URL);
    }

    [Fact]
    public void ProductionBaseUrl_IsCorrectSafaricomUrl()
    {
        Assert.Equal("https://api.safaricom.co.ke/", Constants.PRODUCTION_BASE_URL);
    }

    [Fact]
    public void DefaultGrantType_IsClientCredentials()
    {
        Assert.Equal("client_credentials", Constants.DEFAULT_GRANT_TYPE);
    }

    [Fact]
    public void SandboxBaseUrl_EndsWithSlash()
    {
        Assert.EndsWith("/", Constants.SANDBOX_BASE_URL);
    }

    [Fact]
    public void ProductionBaseUrl_EndsWithSlash()
    {
        Assert.EndsWith("/", Constants.PRODUCTION_BASE_URL);
    }

    [Fact]
    public void SandboxBaseUrl_IsValidUri()
    {
        Assert.True(Uri.TryCreate(Constants.SANDBOX_BASE_URL, UriKind.Absolute, out var uri));
        Assert.Equal(Uri.UriSchemeHttps, uri!.Scheme);
    }

    [Fact]
    public void ProductionBaseUrl_IsValidUri()
    {
        Assert.True(Uri.TryCreate(Constants.PRODUCTION_BASE_URL, UriKind.Absolute, out var uri));
        Assert.Equal(Uri.UriSchemeHttps, uri!.Scheme);
    }
}
