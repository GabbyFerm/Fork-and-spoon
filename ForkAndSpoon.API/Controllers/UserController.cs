﻿using ForkAndSpoon.API.Helpers;
using ForkAndSpoon.Application.Users.Commands.DeleteUser;
using ForkAndSpoon.Application.Users.Commands.UpdateEmail;
using ForkAndSpoon.Application.Users.Commands.UpdatePassword;
using ForkAndSpoon.Application.Users.Commands.UpdateUserName;
using ForkAndSpoon.Application.Users.DTOs;
using ForkAndSpoon.Application.Users.Queries.GetAllUsers;
using ForkAndSpoon.Application.Users.Queries.GetLoggedInUser;
using ForkAndSpoon.Application.Users.Queries.GetUserById;
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
            var users = await _mediator.Send(new GetAllUsersQuery());

            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _mediator.Send(new GetUserByIdQuery(id));

            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }

        [Authorize]
        [HttpGet("get-me")] // Logged in user
        public async Task<IActionResult> GetLoggedInUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("Unable to identify the user from token.");

            int userId = int.Parse(userIdClaim.Value);
            var user = await _mediator.Send(new GetLoggedInUserQuery(userId));

            return Ok(user);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var callerId = ClaimsHelper.GetUserIdFromClaims(User);
            string callerRole = ClaimsHelper.GetUserRoleFromClaims(User);

            var result = await _mediator.Send(new DeleteUserCommand(id, callerId, callerRole));

            if (!result)
                return Forbid(); // User not allowed to delete someone else

            return NoContent();
        }

        [Authorize]
        [HttpPatch("update-email")]
        public async Task<IActionResult> UpdateEmail([FromBody] UpdateEmailDto updateDto)
        {
            var userId = ClaimsHelper.GetUserIdFromClaims(User);

            var command = new UpdateEmailCommand(userId, updateDto.Email);
            var result = await _mediator.Send(command);

            return result ? NoContent() : BadRequest("Email already in use or update failed.");
        }

        [Authorize]
        [HttpPatch("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto updateDto)
        {
            var userId = ClaimsHelper.GetUserIdFromClaims(User);

            var command = new UpdatePasswordCommand(userId, updateDto.CurrentPassword, updateDto.NewPassword);
            var result = await _mediator.Send(command);

            return result ? NoContent() : BadRequest("Current password is incorrect or update failed.");
        }

        [Authorize]
        [HttpPatch("update-username")]
        public async Task<IActionResult> UpdateUserName([FromBody] UpdateUserNameDto updateDto)
        {
            var userId = ClaimsHelper.GetUserIdFromClaims(User);

            var command = new UpdateUserNameCommand(userId, updateDto.UserName);
            var result = await _mediator.Send(command);

            return result ? NoContent() : BadRequest("Username already in use or update failed.");
        }
    }
}