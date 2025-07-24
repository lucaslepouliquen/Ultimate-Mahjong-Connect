namespace UltimateMahjongConnect.Application.DTO
{
    public class AnonymousSessionDTO
    {
        public string SessionId { get; set; } = string.Empty;
        public string AnonymousId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime LastActivity { get; set; }
        public bool IsActive { get; set; }
    }

    public class AnonymousSessionRequestDTO
    {
        public string? SessionId { get; set; }
    }

    public class AnonymousSessionResponseDTO
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public AnonymousSessionDTO? Data { get; set; }

        public static AnonymousSessionResponseDTO Success(AnonymousSessionDTO data)
        {
            return new AnonymousSessionResponseDTO
            {
                IsSuccess = true,
                Data = data
            };
        }

        public static AnonymousSessionResponseDTO Failure(string errorMessage)
        {
            return new AnonymousSessionResponseDTO
            {
                IsSuccess = false,
                ErrorMessage = errorMessage
            };
        }
    }
} 