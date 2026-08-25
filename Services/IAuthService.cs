using TicketManagement.DTOs;
using TicketManagement.Models;

namespace TicketManagement.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterRequest request);
        Task<User?> LoginAsync(LoginRequest request);
    }
}