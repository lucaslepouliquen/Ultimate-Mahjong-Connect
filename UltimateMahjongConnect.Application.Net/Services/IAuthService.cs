using UltimateMahjongConnect.Application.DTO;

namespace UltimateMahjongConnect.Application.Services
{
    public interface IAuthService
    {
        Task<AuthResultDTO> LoginAsync(LoginRequestDTO loginRequest);
        Task<AuthResultDTO> RegisterAsync(RegisterRequestDTO registerRequest);
        Task<AuthResultDTO> OAuthLoginAsync(OAuthLoginRequestDTO oauthRequest);
        Task<GamerResultDTO> GetCurrentGamerAsync(string token);
    }
} 