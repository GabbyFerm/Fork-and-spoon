﻿namespace ForkAndSpoon.Application.Identity.DTOs
{
    public class ResetPasswordDto
    {
        public required string Email { get; set; }
        public required string NewPassword { get; set; }
    }
}
