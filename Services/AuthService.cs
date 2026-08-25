using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketManagement.Data;
using TicketManagement.DTOs;
using TicketManagement.Models;

namespace TicketManagement.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(AppDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existingUser != null)
            {
                return false;
            }

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Role = "Employee",
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<User?> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return null;
            }

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return user;
        }
    }
}