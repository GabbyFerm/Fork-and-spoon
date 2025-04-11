using ForkAndSpoon.Application.Identity.Auth;
using ForkAndSpoon.Application.Identity.DTOs;

namespace ForkAndSpoon.Application.Interfaces
{
    public interface IAuthRepository
    {
        Task<string> LoginAsync(UserLoginDto userLoginDto);
        Task<string> RegisterAsync(UserRegisterDto userRegisterDto);
        Task<bool> ResetPasswordAsync(ResetPasswordDto resetDto);
    }
}
