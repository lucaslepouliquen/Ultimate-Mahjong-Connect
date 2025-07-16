using AutoMapper;
using System.Security.Claims;
using UltimateMahjongConnect.Application.DTO;
using UltimateMahjongConnect.Application.Interface;
using UltimateMahjongConnect.Domain.Models;

namespace UltimateMahjongConnect.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IGamerRepository _gamerRepository;
        private readonly IMapper _mapper;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(IGamerRepository gamerRepository, IMapper mapper, IJwtTokenService jwtTokenService)
        {
            _gamerRepository = gamerRepository;
            _mapper = mapper;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<AuthResultDTO> LoginAsync(LoginRequestDTO loginRequest)
        {
            var gamer = await _gamerRepository.GetGamerByEmailAsync(loginRequest.UsernameOrEmail)
                       ?? await _gamerRepository.GetGamerByPseudonymeAsync(loginRequest.UsernameOrEmail);

            if (gamer == null)
                return AuthResultDTO.Failure("Gamer not found");

            if (!VerifyPassword(loginRequest.Password, gamer))
                return AuthResultDTO.Failure("Invalid credentials");

            var gamerDto = _mapper.Map<GamerDTO>(gamer);
            var token = _jwtTokenService.GenerateToken(gamerDto);

            var authResponse = new AuthResponseDTO
            {
                Token = token,
                Pseudonyme = gamer.Pseudonyme,
                Email = gamer.Email,
                Score = gamer.Score
            };

            return AuthResultDTO.Success(authResponse);
        }

        public async Task<AuthResultDTO> RegisterAsync(RegisterRequestDTO registerRequest)
        {
            var existingGamer = await _gamerRepository.GetGamerByEmailAsync(registerRequest.Email)
                               ?? await _gamerRepository.GetGamerByPseudonymeAsync(registerRequest.Pseudonyme);

            if (existingGamer != null)
                return AuthResultDTO.Failure("A gamer with this email or pseudonyme already exists");

            var newGamer = new Gamer(registerRequest.Pseudonyme, registerRequest.Age, registerRequest.Email);
            
            var gamerId = await _gamerRepository.CreateGamerAsync(newGamer);
            var createdGamer = await _gamerRepository.GetGamerByIdAsync(gamerId);

            if (createdGamer == null)
                return AuthResultDTO.Failure("Failed to create gamer");

            var gamerDto = _mapper.Map<GamerDTO>(createdGamer);
            var token = _jwtTokenService.GenerateToken(gamerDto);

            var authResponse = new AuthResponseDTO
            {
                Token = token,
                Pseudonyme = createdGamer.Pseudonyme,
                Email = createdGamer.Email,
                Score = createdGamer.Score
            };

            return AuthResultDTO.Success(authResponse);
        }

        public async Task<AuthResultDTO> OAuthLoginAsync(OAuthLoginRequestDTO oauthRequest)
        {
            try
            {
                var existingGamer = await _gamerRepository.GetGamerByEmailAsync(oauthRequest.Email);
                
                if (existingGamer != null)
                {
                    var gamerDto = _mapper.Map<GamerDTO>(existingGamer);
                    var token = _jwtTokenService.GenerateToken(gamerDto);
                    var response = new AuthResponseDTO
                    {
                        Token = token,
                        Pseudonyme = existingGamer.Pseudonyme,
                        Email = existingGamer.Email,
                        Score = existingGamer.Score
                    };
                    return AuthResultDTO.Success(response);
                }
                else
                {
                    var newGamer = new Gamer
                    {
                        Pseudonyme = await GenerateUniquePseudonyme(oauthRequest.Name),
                        Email = oauthRequest.Email,
                        PasswordHash = string.Empty,
                        Age = 18,
                        Score = 0
                    };

                    var gamerId = await _gamerRepository.CreateGamerAsync(newGamer);
                    newGamer.Id = gamerId;

                    var gamerDto = _mapper.Map<GamerDTO>(newGamer);
                    var token = _jwtTokenService.GenerateToken(gamerDto);
                    var response = new AuthResponseDTO
                    {
                        Token = token,
                        Pseudonyme = newGamer.Pseudonyme,
                        Email = newGamer.Email,
                        Score = newGamer.Score
                    };
                    return AuthResultDTO.Success(response);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"OAuth login failed: {ex.Message}", ex);
            }
        }

        private async Task<string> GenerateUniquePseudonyme(string baseName)
        {
            var pseudonyme = baseName.Replace(" ", "").ToLower();
            var existingGamer = await _gamerRepository.GetGamerByPseudonymeAsync(pseudonyme);
            
            if (existingGamer == null)
                return pseudonyme;

            int counter = 1;
            string uniquePseudonyme;
            do
            {
                uniquePseudonyme = $"{pseudonyme}{counter}";
                existingGamer = await _gamerRepository.GetGamerByPseudonymeAsync(uniquePseudonyme);
                counter++;
            } while (existingGamer != null);

            return uniquePseudonyme;
        }

        public async Task<GamerResultDTO> GetCurrentGamerAsync(string token)
        {
            try
            {
                var claims = _jwtTokenService.ValidateToken(token);
                
                var pseudonyme = claims.FindFirst(ClaimTypes.Name)?.Value;
                if (string.IsNullOrEmpty(pseudonyme))
                    return GamerResultDTO.Failure("Token does not contain pseudonyme");

                var gamer = await _gamerRepository.GetGamerByPseudonymeAsync(pseudonyme);
                if (gamer == null)
                    return GamerResultDTO.Failure("Gamer not found");

                var gamerDto = _mapper.Map<GamerDTO>(gamer);
                return GamerResultDTO.Success(gamerDto);
            }
            catch (UnauthorizedAccessException ex)
            {
                return GamerResultDTO.Failure($"Invalid token: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return GamerResultDTO.Failure($"Token validation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unexpected error during token validation", ex);
            }
        }



        private bool VerifyPassword(string password, Gamer gamer)
        {
            return true;
        }
    }
} 