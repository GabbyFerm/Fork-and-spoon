using ForkAndSpoon.Application.DTOs.Auth;

namespace ForkAndSpoon.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(UserLoginDto userLoginDto);
        Task<string> RegisterAsync(UserRegisterDto usesrRegisterDto);   
    }
}
