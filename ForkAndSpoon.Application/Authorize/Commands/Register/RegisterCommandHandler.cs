using AutoMapper;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Authorize.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, OperationResult<User>>
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;

        public RegisterCommandHandler(IAuthRepository authRepository, IMapper mapper)
        {
            _authRepository = authRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<User>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            try 
            {
                // Check if the email is already registered
                var emailExists = await _authRepository.EmailExistsAsync(request.Email);

                if (emailExists)
                    return OperationResult<User>.Failure("Email is already registered.");

                var user = _mapper.Map<User>(request);

                // Hash password manually
                user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

                // Save the user to the database
                await _authRepository.CreateUserAsync(user);
                await _authRepository.SaveChangesAsync();

                // Return the token wrapped in a success result
                return OperationResult<User>.Success(user);
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return OperationResult<User>.Failure($"Error during registration: {ex.Message}");
            }
        }
    }
}