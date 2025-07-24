using System.Security.Claims;
using UltimateMahjongConnect.Application.DTO;
using UltimateMahjongConnect.Application.Interface;
using UltimateMahjongConnect.Domain.Models;

namespace UltimateMahjongConnect.Application.Services
{
    public interface IAnonymousSessionService
    {
        Task<AnonymousSessionDTO> CreateAnonymousSessionAsync();
        Task<AnonymousSessionDTO?> GetAnonymousSessionAsync(string sessionId);
        Task<bool> ValidateAnonymousSessionAsync(string sessionId);
        Task DeleteAnonymousSessionAsync(string sessionId);
    }

    public class AnonymousSessionService : IAnonymousSessionService
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly Dictionary<string, AnonymousSessionDTO> _anonymousSessions = new();
        private readonly object _lockObject = new();

        public AnonymousSessionService(IJwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        public async Task<AnonymousSessionDTO> CreateAnonymousSessionAsync()
        {
            var sessionId = Guid.NewGuid().ToString();
            var anonymousId = $"anon_{Guid.NewGuid():N}";
            
            var anonymousSession = new AnonymousSessionDTO
            {
                SessionId = sessionId,
                AnonymousId = anonymousId,
                CreatedAt = DateTime.UtcNow,
                LastActivity = DateTime.UtcNow,
                IsActive = true
            };

            // Créer un token JWT pour l'utilisateur anonyme
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, anonymousId),
                new Claim(ClaimTypes.Name, "Anonymous"),
                new Claim("session_id", sessionId),
                new Claim("is_anonymous", "true")
            };

            var token = _jwtTokenService.GenerateTokenFromClaims(claims);
            anonymousSession.Token = token;

            lock (_lockObject)
            {
                _anonymousSessions[sessionId] = anonymousSession;
            }

            return anonymousSession;
        }

        public async Task<AnonymousSessionDTO?> GetAnonymousSessionAsync(string sessionId)
        {
            lock (_lockObject)
            {
                if (_anonymousSessions.TryGetValue(sessionId, out var session))
                {
                    session.LastActivity = DateTime.UtcNow;
                    return session;
                }
            }
            return null;
        }

        public async Task<bool> ValidateAnonymousSessionAsync(string sessionId)
        {
            lock (_lockObject)
            {
                if (_anonymousSessions.TryGetValue(sessionId, out var session))
                {
                    // Vérifier si la session n'est pas expirée (24h)
                    var expirationTime = session.CreatedAt.AddHours(24);
                    if (DateTime.UtcNow > expirationTime)
                    {
                        _anonymousSessions.Remove(sessionId);
                        return false;
                    }

                    session.LastActivity = DateTime.UtcNow;
                    return session.IsActive;
                }
            }
            return false;
        }

        public async Task DeleteAnonymousSessionAsync(string sessionId)
        {
            lock (_lockObject)
            {
                _anonymousSessions.Remove(sessionId);
            }
        }

        // Méthode pour nettoyer les sessions expirées (à appeler périodiquement)
        public void CleanupExpiredSessions()
        {
            var expiredSessions = new List<string>();
            var cutoffTime = DateTime.UtcNow.AddHours(-24);

            lock (_lockObject)
            {
                foreach (var kvp in _anonymousSessions)
                {
                    if (kvp.Value.CreatedAt < cutoffTime)
                    {
                        expiredSessions.Add(kvp.Key);
                    }
                }

                foreach (var sessionId in expiredSessions)
                {
                    _anonymousSessions.Remove(sessionId);
                }
            }
        }
    }
} 