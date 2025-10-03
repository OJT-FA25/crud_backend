using crud_backend.Data;
using crud_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace crud_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UsersController(AppDbContext context) => _context = context;

        // GET: api/users
        [HttpGet]
        public IActionResult GetUsers() => Ok(_context.Users.ToList());

        // POST: api/users
        [HttpPost]
        public IActionResult AddUser([FromBody] User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok(user);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            var existing = _context.Users.Find(id);
            if (existing == null) return NotFound();

            existing.Name = user.Name;
            _context.SaveChanges();
            return Ok(existing);
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            _context.SaveChanges();
            return NoContent();
        }
    }
}