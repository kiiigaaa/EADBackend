namespace WebApi.Dto
{
    public class UserDetailResponseDto
    {
        public Guid UserId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public bool Success { get; set; }
    }
}
