namespace WebApi.Dto
{
    public class LoginResponseDto
    {
        public bool Success { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public DateTime ValidTo { get; set; }
        public string Email { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
