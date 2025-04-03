namespace ForkAndSpoon.Application.DTOs.User
{
    public class UserDto
    {
        public int UserID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
    }
}
