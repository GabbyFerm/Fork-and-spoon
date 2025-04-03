using AutoMapper;
using ForkAndSpoon.Application.DTOs.User;
using ForkAndSpoon.Domain.Models;

namespace ForkAndSpoon.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}
