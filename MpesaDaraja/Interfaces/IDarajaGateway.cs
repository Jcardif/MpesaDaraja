using MpesaDaraja.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpesaDaraja.Interfaces
{
    public interface IDarajaGateway
    {
        Task<DarajaClient?> GetDarajaClientAsync();
        Task<DarajaClient?> RefreshTokenAsync();
        bool IsTokenValid(string token);
        string GetStkPushPassword(long shortCode, string timestamp);

    }
}
