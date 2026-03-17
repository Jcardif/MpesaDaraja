namespace Mpesa.Daraja.Shared.Exceptions;

/// <summary>
///     Exception thrown when the Daraja API returns an error response.
/// </summary>
public class DarajaException : Exception
{
    /// <summary>
    ///     An instance of the <see cref="DarajaError"/>
    /// </summary>
    public DarajaError? Error { get; }

    /// <summary>
    ///     Initialize a new instance of the <see cref="DarajaException"/>
    /// </summary>
    /// <param name="message"></param>
    public DarajaException(string message) : base(message)
    {
    }

    /// <summary>
    ///     Initialize a new instance of the <see cref="DarajaException"/>
    /// </summary>
    /// <param name="message"></param>
    /// <param name="error"></param>
    public DarajaException(string message, DarajaError error) : base(message)
    {
        Error = error;
    }

    /// <summary>
    ///     Initialize a new instance of the <see cref="DarajaException"/>
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public DarajaException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
