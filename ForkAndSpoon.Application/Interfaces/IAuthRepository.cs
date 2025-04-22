using ForkAndSpoon.Application.Identity.Auth;
using ForkAndSpoon.Application.Identity.DTOs;
using ForkAndSpoon.Domain.Models;

namespace ForkAndSpoon.Application.Interfaces
{
    public interface IAuthRepository
    {
        Task<OperationResult<string>> LoginAsync(UserLoginDto userLoginDto);
        Task<OperationResult<string>> RegisterAsync(UserRegisterDto userRegisterDto);
        Task<OperationResult<bool>> ResetPasswordAsync(ResetPasswordDto resetDto);
    }
}
