﻿namespace ForkAndSpoon.Application.DTOs.Auth
{
    public class UserLoginDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
