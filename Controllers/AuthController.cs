using Microsoft.AspNetCore.Mvc;
using TicketManagement.DTOs;
using TicketManagement.Services;

namespace TicketManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;

        public AuthController(IAuthService authService, IJwtService jwtService)
        {
            _authService = authService;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);

            if (!result)
            {
                return BadRequest("Email already exists.");
            }

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _authService.LoginAsync(request);

            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            var token = _jwtService.GenerateToken(user);

            return Ok(new
            {
                token
            });
        }
    }
}