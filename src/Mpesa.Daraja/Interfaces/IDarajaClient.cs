using Mpesa.Daraja.Models;

namespace Mpesa.Daraja.Interfaces
{
    internal interface IDarajaClient
    {
        /// <summary>
        ///     Access token to access other APIs
        /// </summary>
        string? AccessToken { get; }

        /// <summary>
        ///    Token expiry time in seconds
        /// </summary>
        long ExpiresIn { get; }

        /// <summary>
        ///  Client to handle http requests to the daraja API
        /// </summary>
        HttpClient Client { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="expiresIn"></param>
        void TokenRefreshed(string accessToken, long expiresIn);

        /// <summary>
        ///     Initiates online payment on behalf of a customer.
        /// </summary>
        /// <param name="mpesaStkData"></param>
        /// <returns></returns>
        Task<PushResponse?> SendStkPushAsync(StkData mpesaStkData);

        /// <summary>
        ///     Check the status of an Online M-Pesa Payment.
        /// </summary>
        /// <param name="pushResponse">Response from initiating the STK Push</param>
        /// <param name="stkData">The <see cref="StkData"/> object used to initiate the stk push</param>
        /// <returns></returns>
        Task<(bool isCompleted, PushQueryResponse? pushQueryResponse)> QueryStkPushStatus(PushResponse pushResponse,
            StkData stkData);
    }
}
