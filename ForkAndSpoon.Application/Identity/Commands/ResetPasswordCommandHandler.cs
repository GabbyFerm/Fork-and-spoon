using ForkAndSpoon.Application.Identity.DTOs;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Identity.Commands
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, OperationResult<bool>>
    {
        private readonly IAuthRepository _authRepository;

        public ResetPasswordCommandHandler(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        public async Task<OperationResult<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var dto = new ResetPasswordDto 
            { 
                Email = request.Email,
                NewPassword = request.NewPassword,            
            };

            return await _authRepository.ResetPasswordAsync(dto);
        }
    }
}
