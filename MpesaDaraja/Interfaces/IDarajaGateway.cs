using MpesaDaraja.Services;

namespace MpesaDaraja.Interfaces
{
    /// <summary>
    ///     Handles Access to the Daraja API
    /// </summary>
    public interface IDarajaGateway
    {
        /// <summary>
        ///     Get the <see cref="DarajaClient"/>
        /// </summary>
        /// <param name="isInProduction">Is the API usage in production (value true) or sandbox (value false)</param>
        /// <returns><see cref="DarajaClient"/></returns>
        Task<DarajaClient?> GetDarajaClientAsync(bool isInProduction);

        /// <summary>
        ///     Refresh the access token after expiry
        /// </summary>
        /// <returns><see cref="DarajaClient"/></returns>
        Task<DarajaClient?> RefreshTokenAsync();

        /// <summary>
        ///     Check whether the access token has expired
        /// </summary>
        /// <param name="token"></param>
        /// <returns><see cref="bool"/></returns>
        bool IsTokenValid(string token);

        /// <summary>
        ///     Get the password used for encrypting the request sent
        /// </summary>
        /// <param name="shortCode">This is organizations shortcode (Paybill or Buygoods - A 5 to 7 digit account number) used to identify an organization and receive the transaction.</param>
        /// <param name="timestamp">This is the Timestamp of the transaction, normally in the format of YEAR+MONTH+DATE+HOUR+MINUTE+SECOND (yyyyMMddHHmmss)</param>
        /// <returns><see cref="string"/> which is the requested password</returns>
        string GetStkPushPassword(long shortCode, string timestamp);

    }
}
