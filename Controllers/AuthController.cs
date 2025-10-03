using System.Linq;
using crud_backend.Data;
using crud_backend.Models;
using crud_backend.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;

namespace crud_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AuthController(AppDbContext context) => _context = context;

        // POST api/auth/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Email and password are required.");

            var exists = _context.Users.Any(u => u.Email == dto.Email);
            if (exists) return Conflict("Email already registered.");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            // trả về thông tin cơ bản (không trả password)
            return CreatedAtAction(nameof(GetMe), new { id = user.Id }, new { user.Id, user.Name, user.Email });
        }

        // POST api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Email and password are required.");

            var user = _context.Users.SingleOrDefault(u => u.Email == dto.Email);
            if (user == null) return Unauthorized("Invalid credentials.");

            bool valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!valid) return Unauthorized("Invalid credentials.");

            // Nếu cần: trả token JWT ở đây. Hiện trả user info đơn giản.
            return Ok(new { user.Id, user.Name, user.Email });
        }

        // GET api/auth/me (test)
        [HttpGet("me")]
        public IActionResult GetMe()
        {
            // placeholder: trong hệ thống có auth bạn sẽ lấy user từ token
            return Ok("Add authentication to get current user.");
        }
    }
}