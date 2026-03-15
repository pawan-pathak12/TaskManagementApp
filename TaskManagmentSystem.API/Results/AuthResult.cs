namespace TaskManagmentSystem.API.Results
{
    public class AuthResult
    {
        public string? Error { get; set; }
        public string? Message { get; set; }

        public bool IsSuccess { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }

        public static AuthResult Success(string message)
        {
            return new AuthResult
            {
                IsSuccess = true,
                Message = message,
            };
        }

        public static AuthResult Failure(string error)
        {
            return new AuthResult
            {
                IsSuccess = false,
                Message = error,
            };
        }
    }
}
