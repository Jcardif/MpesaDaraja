namespace Mpesa.Daraja.Auth
{
    /// <summary>
    ///     Handles Access to the Daraja API
    /// </summary>
    public interface IDarajaGateway
    {
        /// <summary>
        ///     Get the Daraja client by obtaining an access token from the Daraja API.
        /// </summary>
        Task InitializeDarajaAsync();

    }
}
