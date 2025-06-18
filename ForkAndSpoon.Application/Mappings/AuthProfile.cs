using AutoMapper;
using ForkAndSpoon.Domain.Models;
using ForkAndSpoon.Application.Authorize.DTOs;
using ForkAndSpoon.Application.Authorize.Commands.Register;

namespace ForkAndSpoon.Application.Authorize.Mappings
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            // Mapping from User (domain) to UserDtoResponse (API response)
            CreateMap<User, UserDtoResponse>()
                .ForMember(destinationProperty => destinationProperty.Id,
                    config => config.MapFrom(sourceObject => sourceObject.UserID))
                .ForMember(destinationProperty => destinationProperty.Name,
                    config => config.MapFrom(sourceObject => sourceObject.UserName))
                .ForMember(destinationProperty => destinationProperty.Roles,
                    config => config.MapFrom(sourceObject => sourceObject.Role));

            // Mapping from RegisterCommand (request) to User (domain)
            CreateMap<RegisterCommand, User>()
                .ForMember(destinationProperty => destinationProperty.Password,
                    config => config.Ignore()) // Will be hashed manually
                .ForMember(destinationProperty => destinationProperty.Role,
                    config => config.MapFrom(_ => "User")); // Default role
        }
    }
}