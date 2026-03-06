namespace Mpesa.Daraja.Shared;

/// <summary>
///     Represents the result of a Daraja API call, containing either a successful response or an error.
/// </summary>
/// <typeparam name="T">The type of the successful response.</typeparam>
public class DarajaResult<T>
{
    /// <summary>
    ///     The value of the successful response from the Daraja API, or null if the request failed.
    /// </summary>
    public T? Value { get; }

    /// <summary>
    ///     The error details if the API request failed, or null if the request was successful.
    /// </summary>
    public DarajaError? Error { get; }

    /// <summary>
    ///     Indicates whether the API request was successful
    /// </summary>
    public bool IsSuccess => Error is null;

    private DarajaResult(T? value, DarajaError? error)
    {
        Value = value;
        Error = error;
    }

    /// <summary>
    ///     Returns a successful result containing the specified value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static DarajaResult<T> Success(T value) => new(value, null);

    /// <summary>
    ///     Returns a failed result containing the specified error details.
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static DarajaResult<T> Failure(DarajaError error) => new(default, error);
}
