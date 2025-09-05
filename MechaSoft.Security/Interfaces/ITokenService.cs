using MechaSoft.Domain.Model;
using System.Security.Claims;

namespace MechaSoft.Security.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
