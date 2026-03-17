using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Mpesa.Daraja.Shared;

/// <summary>
///     Fluent builder for registering Daraja SDK features.
/// </summary>
public sealed class DarajaBuilder
{
    internal DarajaBuilder(IServiceCollection services)
    {
        Services = services;
    }

    private IServiceCollection Services { get; }

    /// <summary>
    ///     Enables the M-Pesa Express feature.
    /// </summary>
    /// <returns>The builder for further chaining.</returns>
    public DarajaBuilder WithMpesaExpress()
    {
        Services.TryAddSingleton<MpesaExpressFeatureMarker>();
        Services.TryAddTransient<IMpesaExpress, MpesaExpress>();

        return this;
    }

    /// <summary>
    ///     Enables the reversal feature.
    /// </summary>
    /// <returns>The builder for further chaining.</returns>
    public DarajaBuilder WithReversal()
    {
        Services.TryAddSingleton<ReversalFeatureMarker>();
        Services.TryAddTransient<Reversal>();

        return this;
    }
}
