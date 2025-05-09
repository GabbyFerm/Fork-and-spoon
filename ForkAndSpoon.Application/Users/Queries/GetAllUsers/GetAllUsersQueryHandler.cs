using AutoMapper;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Application.Users.DTOs;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, OperationResult<List<UserDto>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<List<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var result = await _userRepository.GetAllAsync();

            if (!result.IsSuccess)
                return OperationResult<List<UserDto>>.Failure(result.ErrorMessage ?? "No users found.");

            var userDtos = _mapper.Map<List<UserDto>>(result.Data);

            return OperationResult<List<UserDto>>.Success(userDtos);
        }
    }
}
