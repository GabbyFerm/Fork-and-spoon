﻿namespace ForkAndSpoon.Application.Users.DTOs
{
    public class UpdatePasswordDto
    {
        public required string CurrentPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}
