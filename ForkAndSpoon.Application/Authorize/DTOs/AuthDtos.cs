namespace ForkAndSpoon.Application.Authorize.DTOs
{
    public class UserRegisterDto
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
    public class UserLoginDto
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
    public class ResetPasswordDto
    {
        public required string Email { get; set; }
        public required string NewPassword { get; set; }
    }
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public UserDtoResponse User { get; set; } = new();
    }

    public class UserDtoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Roles { get; set; } = string.Empty;
    }
}
