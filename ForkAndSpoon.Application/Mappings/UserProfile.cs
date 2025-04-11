using AutoMapper;
using ForkAndSpoon.Application.Users.DTOs;
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
