using ForkAndSpoon.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForkAndSpoon.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(UserLoginDto userLoginDto);
        Task<string> RegisterAsync(UserRegisterDto usesrRegisterDto);   
    }
}
