using AutoMapper;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Application.Users.DTOs;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Users.Queries.GetLoggedInUser
{
    public class GetLoggedInUserQueryHandler : IRequestHandler<GetLoggedInUserQuery, OperationResult<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetLoggedInUserQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<UserDto>> Handle(GetLoggedInUserQuery request, CancellationToken cancellationToken)
        {
            // Fetch the user by ID
            var result = await _userRepository.GetByIdAsync(request.UserId);

            // If user not found or failed to load, return failure
            if (!result.IsSuccess || result.Data == null)
                return OperationResult<UserDto>.Failure(result.ErrorMessage ?? "User not found.");

            // Map user to DTO
            var userDto = _mapper.Map<UserDto>(result.Data);

            // Return success with the mapped user
            return OperationResult<UserDto>.Success(userDto);
        }
    }
}
