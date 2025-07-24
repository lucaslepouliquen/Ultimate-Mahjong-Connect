using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using UltimateMahjongConnect.Application.DTO;
using UltimateMahjongConnect.Application.Services;

namespace Ultimate_Mahjong_Connect.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IAnonymousSessionService _anonymousSessionService;

        public AuthController(IAuthService authService, IAnonymousSessionService anonymousSessionService)
        {
            _authService = authService;
            _anonymousSessionService = anonymousSessionService;
        }

        /// <summary>
        /// Create an anonymous session for users who want to play without registration
        /// </summary>
        /// <returns>Anonymous session with JWT token</returns>
        [HttpPost("anonymous")]
        public async Task<IActionResult> CreateAnonymousSession()
        {
            try
            {
                var anonymousSession = await _anonymousSessionService.CreateAnonymousSessionAsync();
                return Ok(new { 
                    message = "Anonymous session created successfully",
                    data = anonymousSession 
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Failed to create anonymous session: {ex.Message}" });
            }
        }

        /// <summary>
        /// Validate an anonymous session
        /// </summary>
        /// <param name="sessionId">Session ID to validate</param>
        /// <returns>Validation result</returns>
        [HttpGet("anonymous/validate/{sessionId}")]
        public async Task<IActionResult> ValidateAnonymousSession(string sessionId)
        {
            try
            {
                var isValid = await _anonymousSessionService.ValidateAnonymousSessionAsync(sessionId);
                return Ok(new { 
                    isValid = isValid,
                    message = isValid ? "Session is valid" : "Session is invalid or expired"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Failed to validate session: {ex.Message}" });
            }
        }

        /// <summary>
        /// Get anonymous session details
        /// </summary>
        /// <param name="sessionId">Session ID</param>
        /// <returns>Session details</returns>
        [HttpGet("anonymous/{sessionId}")]
        public async Task<IActionResult> GetAnonymousSession(string sessionId)
        {
            try
            {
                var session = await _anonymousSessionService.GetAnonymousSessionAsync(sessionId);
                if (session == null)
                    return NotFound(new { message = "Session not found" });

                return Ok(new { 
                    message = "Session retrieved successfully",
                    data = session 
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Failed to get session: {ex.Message}" });
            }
        }

        /// <summary>
        /// Delete an anonymous session
        /// </summary>
        /// <param name="sessionId">Session ID to delete</param>
        /// <returns>Deletion result</returns>
        [HttpDelete("anonymous/{sessionId}")]
        public async Task<IActionResult> DeleteAnonymousSession(string sessionId)
        {
            try
            {
                await _anonymousSessionService.DeleteAnonymousSessionAsync(sessionId);
                return Ok(new { message = "Session deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Failed to delete session: {ex.Message}" });
            }
        }

        /// <summary>
        /// Register a new gamer
        /// </summary>
        /// <param name="request">Registration request</param>
        /// <returns>JWT token if successful</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(request);
            
            if (!result.IsSuccess)
                return BadRequest(new { message = result.ErrorMessage });

            return Ok(result.Data);
        }

        /// <summary>
        /// Login with pseudonyme/email and password
        /// </summary>
        /// <param name="request">Login request</param>
        /// <returns>JWT token if successful</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(request);
            
            if (!result.IsSuccess)
                return Unauthorized(new { message = result.ErrorMessage });

            return Ok(result.Data);
        }

        /// <summary>
        /// Initiate Google OAuth authentication
        /// </summary>
        /// <returns>Challenge result</returns>
        [HttpGet("google/login")]
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action(nameof(GoogleCallback), "Auth");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, "Google");
        }

        /// <summary>
        /// Google OAuth callback
        /// </summary>
        /// <returns>JWT token if successful</returns>
        [HttpGet("google/callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync("Google");
            if (!result.Succeeded)
                return BadRequest(new { message = "Google authentication failed" });

            return await ProcessOAuthCallback(result.Principal, "Google");
        }

        /// <summary>
        /// Initiate Facebook OAuth authentication
        /// </summary>
        /// <returns>Challenge result</returns>
        [HttpGet("facebook/login")]
        public IActionResult FacebookLogin()
        {
            var redirectUrl = Url.Action(nameof(FacebookCallback), "Auth");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, "Facebook");
        }

        /// <summary>
        /// Facebook OAuth callback
        /// </summary>
        /// <returns>JWT token if successful</returns>
        [HttpGet("facebook/callback")]
        public async Task<IActionResult> FacebookCallback()
        {
            var result = await HttpContext.AuthenticateAsync("Facebook");
            if (!result.Succeeded)
                return BadRequest(new { message = "Facebook authentication failed" });

            return await ProcessOAuthCallback(result.Principal, "Facebook");
        }

        /// <summary>
        /// Process OAuth callback and create/login user
        /// </summary>
        /// <param name="principal">Claims principal from OAuth provider</param>
        /// <param name="provider">OAuth provider name</param>
        /// <returns>JWT token</returns>
        private async Task<IActionResult> ProcessOAuthCallback(ClaimsPrincipal principal, string provider)
        {
            var email = principal.FindFirst(ClaimTypes.Email)?.Value;
            var name = principal.FindFirst(ClaimTypes.Name)?.Value;
            var nameIdentifier = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(email))
                return BadRequest(new { message = $"Email not provided by {provider}" });

            // Create OAuth login request
            var oauthRequest = new OAuthLoginRequestDTO
            {
                Email = email,
                Name = name ?? email.Split('@')[0],
                Provider = provider,
                ProviderId = nameIdentifier
            };

            var authResult = await _authService.OAuthLoginAsync(oauthRequest);
            
            if (!authResult.IsSuccess)
                return BadRequest(new { message = authResult.ErrorMessage });

            // Redirect to frontend with token
            var frontendUrl = $"http://localhost:4200/auth/callback?token={authResult.Data.Token}";
            return Redirect(frontendUrl);
        }

        /// <summary>
        /// Get current gamer info (requires authentication)
        /// </summary>
        /// <returns>Current gamer information</returns>
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentGamer()
        {
            var authHeader = Request.Headers.Authorization.FirstOrDefault();
            if (authHeader == null || !authHeader.StartsWith("Bearer "))
                return Unauthorized(new { message = "Authorization header missing or invalid" });

            var token = authHeader.Substring(7);
            var result = await _authService.GetCurrentGamerAsync(token);
            
            if (!result.IsSuccess)
                return Unauthorized(new { message = result.ErrorMessage });

            return Ok(result.Data);
        }
    }
} 