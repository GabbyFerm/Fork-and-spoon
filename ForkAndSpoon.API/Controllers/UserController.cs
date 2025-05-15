using ForkAndSpoon.API.Helpers;
using ForkAndSpoon.Application.Users.Commands.DeleteUser;
using ForkAndSpoon.Application.Users.Commands.UpdateEmail;
using ForkAndSpoon.Application.Users.Commands.UpdatePassword;
using ForkAndSpoon.Application.Users.Commands.UpdateUserName;
using ForkAndSpoon.Application.Users.DTOs;
using ForkAndSpoon.Application.Users.Queries.GetAllUsers;
using ForkAndSpoon.Application.Users.Queries.GetLoggedInUser;
using ForkAndSpoon.Application.Users.Queries.GetUserById;
using ForkAndSpoon.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ForkAndSpoon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _mediator.Send(new GetAllUsersQuery());

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery(id));

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("get-me")] 
        public async Task<IActionResult> GetLoggedInUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                var errorResult = OperationResult<UserDto>.Failure("Unable to identify the result from token.");
                return Unauthorized(errorResult);
            }

            int userId = int.Parse(userIdClaim.Value);
            var result = await _mediator.Send(new GetLoggedInUserQuery(userId));

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var callerId = ClaimsHelper.GetUserIdFromClaims(User);
            var callerRole = ClaimsHelper.GetUserRoleFromClaims(User);

            var result = await _mediator.Send(new DeleteUserCommand(id, callerId, callerRole));

            if (!result.IsSuccess)
                return BadRequest(result); 

            return Ok(result);
        }

        [Authorize]
        [HttpPatch("update-email")]
        public async Task<IActionResult> UpdateEmail([FromBody] UpdateEmailDto updateDto)
        {
            var userId = ClaimsHelper.GetUserIdFromClaims(User);
            var result = await _mediator.Send(new UpdateEmailCommand(userId, updateDto.Email));

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize]
        [HttpPatch("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto updateDto)
        {
            var userId = ClaimsHelper.GetUserIdFromClaims(User);
            var result = await _mediator.Send(new UpdatePasswordCommand(userId, updateDto.CurrentPassword, updateDto.NewPassword));

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize]
        [HttpPatch("update-username")]
        public async Task<IActionResult> UpdateUserName([FromBody] UpdateUserNameDto updateDto)
        {
            var userId = ClaimsHelper.GetUserIdFromClaims(User);
            var result = await _mediator.Send(new UpdateUserNameCommand(userId, updateDto.UserName));

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}