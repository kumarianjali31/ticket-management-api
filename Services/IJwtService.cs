using TicketManagement.Models;

namespace TicketManagement.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}