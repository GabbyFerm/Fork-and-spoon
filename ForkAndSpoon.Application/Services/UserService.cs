using AutoMapper;
using ForkAndSpoon.Application.DTOs.User;
using ForkAndSpoon.Application.Interfaces;
using ForkAndSpoon.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ForkAndSpoon.Application.Services
{
    public class UserService : IUserService
    {
        private readonly ForkAndSpoonDbContext _context;
        private readonly IMapper _mapper;

        public UserService(ForkAndSpoonDbContext context, IMapper mapper)
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
    }

}
