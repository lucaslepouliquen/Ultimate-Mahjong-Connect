namespace UltimateMahjongConnect.Application.DTO
{
    public class LoginRequestDTO
    {
        public required string UsernameOrEmail { get; set; }
        public required string Password { get; set; }
    }

    public class RegisterRequestDTO
    {
        public required string Pseudonyme { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public int Age { get; set; } = 18;
    }

    public class AuthResponseDTO
    {
        public required string Token { get; set; }
        public required string Pseudonyme { get; set; }
        public required string Email { get; set; }
        public int Score { get; set; }
    }

    public class AuthResultDTO
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public AuthResponseDTO? Data { get; set; }

        public static AuthResultDTO Success(AuthResponseDTO data)
        {
            return new AuthResultDTO
            {
                IsSuccess = true,
                Data = data
            };
        }

        public static AuthResultDTO Failure(string errorMessage)
        {
            return new AuthResultDTO
            {
                IsSuccess = false,
                ErrorMessage = errorMessage
            };
        }
    }

    public class GamerResultDTO
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public GamerDTO? Data { get; set; }

        public static GamerResultDTO Success(GamerDTO data)
        {
            return new GamerResultDTO
            {
                IsSuccess = true,
                Data = data
            };
        }

        public static GamerResultDTO Failure(string errorMessage)
        {
            return new GamerResultDTO
            {
                IsSuccess = false,
                ErrorMessage = errorMessage
            };
        }
    }

    public class OAuthLoginRequestDTO
    {
        public required string Email { get; set; }
        public required string Name { get; set; }
        public required string Provider { get; set; }
        public required string ProviderId { get; set; }
    }
} 