namespace ForkAndSpoon.Application.DTOs.Auth
{
    public class UserRegisterDto
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
