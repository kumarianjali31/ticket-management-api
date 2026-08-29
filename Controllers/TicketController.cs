using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketManagement.Data;
using TicketManagement.DTOs;
using TicketManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace TicketManagement.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TicketController> _logger;

        public TicketController(AppDbContext context, ILogger<TicketController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> CreateTicket(TicketRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var ticket = new Ticket
            {
                Title = request.Title,
                Description = request.Description,
                Priority = request.Priority,
                Status = "Open",
                CreatedByUserId = int.Parse(userId),
                CreatedAt = DateTime.UtcNow
            };

            _context.Tickets.Add(ticket);

            await _context.SaveChangesAsync();
            _logger.LogInformation(
            "Ticket created successfully. Ticket Id: {TicketId}",
                ticket.Id);

            return Ok(ticket);
        }

        [HttpGet]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetTickets()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var tickets = await _context.Tickets
                .Where(t => t.CreatedByUserId == int.Parse(userId))
                .ToListAsync();

            return Ok(tickets);
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateTicketStatusRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(t =>
                    t.Id == id &&
                    t.AssignedToUserId == int.Parse(userId));

            if (ticket == null)
            {
                return NotFound("Ticket not found or ticket is not assigned to you.");
            }

            ticket.Status = request.Status;

            await _context.SaveChangesAsync();

            return Ok(ticket);
        }

        [HttpPut("admin/{id}/assign")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignTicket(int id, int agentId)
        {
            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null)
            {
                return NotFound("Ticket not found.");
            }

            var agent = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == agentId && u.Role == "Agent");

            if (agent == null)
            {
                return BadRequest("Agent not found.");
            }

            ticket.AssignedToUserId = agent.Id;

            await _context.SaveChangesAsync();

            return Ok(ticket);
        }
    }
}