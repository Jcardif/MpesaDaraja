using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mpesa.Daraja.Auth;
using Mpesa.Daraja.Shared;

namespace Mpesa.Daraja.Tests.DependencyInjection;

public class DarajaServiceCollectionExtensionsTests
{
    [Fact]
    public void AddMpesaDaraja_ReturnsBuilder()
    {
        var services = new ServiceCollection();

        var builder = services.AddMpesaDaraja(options =>
        {
            options.ConsumerKey = "key";
            options.ConsumerSecret = "secret";
        });

        Assert.IsType<DarajaBuilder>(builder);
    }

    [Fact]
    public void WithMpesaExpress_RegistersService()
    {
        var services = new ServiceCollection();
        services
            .AddMpesaDaraja(options =>
            {
                options.ConsumerKey = "key";
                options.ConsumerSecret = "secret";
                options.PassKey = "passkey";
            })
            .WithMpesaExpress();

        using var serviceProvider = services.BuildServiceProvider();

        var service = serviceProvider.GetRequiredService<IMpesaExpress>();

        Assert.IsType<Daraja.MpesaExpress>(service);
    }

    [Fact]
    public void WithReversal_RegistersService()
    {
        var services = new ServiceCollection();
        services
            .AddMpesaDaraja(options =>
            {
                options.ConsumerKey = "key";
                options.ConsumerSecret = "secret";
                options.InitiatorPassword = "initiator-password";
            })
            .WithReversal();

        using var serviceProvider = services.BuildServiceProvider();

        var service = serviceProvider.GetRequiredService<Daraja.Reversal>();

        Assert.NotNull(service);
    }

    [Fact]
    public void AddMpesaDaraja_RegistersConfiguredOptions()
    {
        var services = new ServiceCollection();
        services.AddMpesaDaraja(options =>
        {
            options.ConsumerKey = "key";
            options.ConsumerSecret = "secret";
            options.IsLive = true;
        });

        using var serviceProvider = services.BuildServiceProvider();
        var options = serviceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<DarajaOptions>>().Value;

        Assert.Equal("key", options.ConsumerKey);
        Assert.Equal("secret", options.ConsumerSecret);
        Assert.True(options.IsLive);
    }

    [Fact]
    public void WithMpesaExpress_AndWithReversal_EnableFluentRegistration()
    {
        var services = new ServiceCollection();
        services
            .AddMpesaDaraja(options =>
            {
                options.ConsumerKey = "key";
                options.ConsumerSecret = "secret";
                options.PassKey = "passkey";
                options.InitiatorPassword = "initiator-password";
            })
            .WithMpesaExpress()
            .WithReversal();

        using var serviceProvider = services.BuildServiceProvider();

        Assert.IsType<Daraja.MpesaExpress>(serviceProvider.GetRequiredService<IMpesaExpress>());
        Assert.NotNull(serviceProvider.GetRequiredService<Daraja.Reversal>());
    }

    [Fact]
    public void AddMpesaDaraja_WhenConsumerKeyMissing_ThrowsOptionsValidationException()
    {
        var services = new ServiceCollection();
        services.AddMpesaDaraja(options => options.ConsumerSecret = "secret");

        using var serviceProvider = services.BuildServiceProvider();

        var exception = Assert.Throws<OptionsValidationException>(
            () => serviceProvider.GetRequiredService<IOptions<DarajaOptions>>().Value);

        Assert.Contains(exception.Failures, failure => failure.Contains(nameof(DarajaOptions.ConsumerKey), StringComparison.Ordinal));
    }

    [Fact]
    public void AddMpesaDaraja_WhenConsumerSecretMissing_ThrowsOptionsValidationException()
    {
        var services = new ServiceCollection();
        services.AddMpesaDaraja(options => options.ConsumerKey = "key");

        using var serviceProvider = services.BuildServiceProvider();

        var exception = Assert.Throws<OptionsValidationException>(
            () => serviceProvider.GetRequiredService<IOptions<DarajaOptions>>().Value);

        Assert.Contains(exception.Failures, failure => failure.Contains(nameof(DarajaOptions.ConsumerSecret), StringComparison.Ordinal));
    }

    [Fact]
    public void WithMpesaExpress_WhenPassKeyMissing_ThrowsOptionsValidationException()
    {
        var services = new ServiceCollection();
        services
            .AddMpesaDaraja(options =>
            {
                options.ConsumerKey = "key";
                options.ConsumerSecret = "secret";
            })
            .WithMpesaExpress();

        using var serviceProvider = services.BuildServiceProvider();

        var exception = Assert.Throws<OptionsValidationException>(
            () => serviceProvider.GetRequiredService<IOptions<DarajaOptions>>().Value);

        Assert.Contains(exception.Failures, failure => failure.Contains(nameof(DarajaOptions.PassKey), StringComparison.Ordinal));
    }

    [Fact]
    public void WithReversal_WhenInitiatorPasswordMissing_ThrowsOptionsValidationException()
    {
        var services = new ServiceCollection();
        services
            .AddMpesaDaraja(options =>
            {
                options.ConsumerKey = "key";
                options.ConsumerSecret = "secret";
            })
            .WithReversal();

        using var serviceProvider = services.BuildServiceProvider();

        var exception = Assert.Throws<OptionsValidationException>(
            () => serviceProvider.GetRequiredService<IOptions<DarajaOptions>>().Value);

        Assert.Contains(exception.Failures, failure => failure.Contains(nameof(DarajaOptions.InitiatorPassword), StringComparison.Ordinal));
    }

    [Fact]
    public void AddMpesaDaraja_DoesNotRequirePassKeyUntilMpesaExpressEnabled()
    {
        var services = new ServiceCollection();
        services.AddMpesaDaraja(options =>
        {
            options.ConsumerKey = "key";
            options.ConsumerSecret = "secret";
        });

        using var serviceProvider = services.BuildServiceProvider();

        var options = serviceProvider.GetRequiredService<IOptions<DarajaOptions>>().Value;

        Assert.Equal("key", options.ConsumerKey);
    }

    [Fact]
    public void AddMpesaDaraja_DoesNotRequireInitiatorPasswordUntilReversalEnabled()
    {
        var services = new ServiceCollection();
        services.AddMpesaDaraja(options =>
        {
            options.ConsumerKey = "key";
            options.ConsumerSecret = "secret";
        });

        using var serviceProvider = services.BuildServiceProvider();

        var options = serviceProvider.GetRequiredService<IOptions<DarajaOptions>>().Value;

        Assert.Equal("secret", options.ConsumerSecret);
    }
}
