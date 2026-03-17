using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mpesa.Daraja.Shared;

namespace Mpesa.Daraja.Auth;

internal sealed class DarajaOptionsValidator(
    IServiceProvider serviceProvider)
    : IValidateOptions<DarajaOptions>
{
    public ValidateOptionsResult Validate(string? name, DarajaOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(options.ConsumerKey))
        {
            errors.Add($"{nameof(DarajaOptions.ConsumerKey)} must be provided.");
        }

        if (string.IsNullOrWhiteSpace(options.ConsumerSecret))
        {
            errors.Add($"{nameof(DarajaOptions.ConsumerSecret)} must be provided.");
        }

        if (serviceProvider.GetService<MpesaExpressFeatureMarker>() is not null &&
            string.IsNullOrWhiteSpace(options.PassKey))
        {
            errors.Add($"{nameof(DarajaOptions.PassKey)} must be provided when M-Pesa Express is enabled.");
        }

        if (serviceProvider.GetService<ReversalFeatureMarker>() is not null &&
            string.IsNullOrWhiteSpace(options.InitiatorPassword))
        {
            errors.Add($"{nameof(DarajaOptions.InitiatorPassword)} must be provided when reversal is enabled.");
        }

        if (errors.Count > 0)
        {
            return ValidateOptionsResult.Fail(errors);
        }

        return ValidateOptionsResult.Success;
    }
}
