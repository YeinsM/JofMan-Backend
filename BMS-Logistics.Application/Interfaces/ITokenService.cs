using System.Security.Claims;

namespace BMS_Logistics.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string userId, IEnumerable<Claim>? additionalClaims = null);
    }
}