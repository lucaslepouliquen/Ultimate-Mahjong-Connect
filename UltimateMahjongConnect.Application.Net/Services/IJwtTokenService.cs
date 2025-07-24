using UltimateMahjongConnect.Application.DTO;
using System.Security.Claims;

namespace UltimateMahjongConnect.Application.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(GamerDTO gamer);
        string GenerateTokenFromClaims(IEnumerable<Claim> claims);
        ClaimsPrincipal? ValidateToken(string token);
    }
} 