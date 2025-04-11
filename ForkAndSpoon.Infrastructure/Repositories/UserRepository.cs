using AutoMapper;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Application.Users.DTOs;
using ForkAndSpoon.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ForkAndSpoon.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ForkAndSpoonDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(ForkAndSpoonDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateEmailAsync(int userId, string newEmail)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            // Check if the new email already exists in the database
            var emailExists = await _context.Users.AnyAsync(user => user.Email == newEmail);
            if (emailExists) throw new InvalidOperationException("Email is already registered.");

            // Update the user's email
            user.Email = newEmail;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdatePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            // Verify the current password
            var isCurrentPasswordValid = BCrypt.Net.BCrypt.Verify(currentPassword, user.Password);
            if (!isCurrentPasswordValid) return false;

            // Hash the new password and update
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _context.SaveChangesAsync();

            return true;
        }
    }

}
