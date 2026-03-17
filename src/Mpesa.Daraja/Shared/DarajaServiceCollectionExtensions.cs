using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Mpesa.Daraja.Auth;

namespace Mpesa.Daraja.Shared;

/// <summary>
///     Dependency injection registration extensions for the Daraja SDK.
/// </summary>
public static class DarajaServiceCollectionExtensions
{
    internal const string HttpClientName = "MpesaDaraja";

    /// <summary>
    ///     Registers the shared Daraja gateway and HTTP client infrastructure.
    /// </summary>
    /// <param name="services">The service collection to update.</param>
    /// <param name="options">Configures gateway options.</param>
    /// <returns>A fluent builder for enabling Daraja SDK features.</returns>
    public static DarajaBuilder AddMpesaDaraja(
        this IServiceCollection services,
        Action<DarajaOptions> options)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(options);

        services.Configure(options);
        services.AddHttpClient(HttpClientName, static (serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<DarajaOptions>>().Value;
            client.BaseAddress = DarajaGateway.CreateBaseAddress(options.IsLive);
        });
        services.TryAddSingleton<DarajaGateway>();
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IValidateOptions<DarajaOptions>, DarajaOptionsValidator>());
        services.AddOptions<DarajaOptions>().ValidateOnStart();

        return new DarajaBuilder(services);
    }
}
