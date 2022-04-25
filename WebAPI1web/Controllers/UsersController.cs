using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI1web.Data;
using WebAPI1web.DTOs;
using WebAPI1web.Models;
using System.Security.Claims;

namespace WebAPI1web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublicUserDTO>>> GetUsers()
        {
            var users=await _context.Users.ToListAsync();
            return users.ConvertAll(x=>new PublicUserDTO {Id=x.Id,Username=x.Name});
        }

        [Authorize]
        [HttpGet("{username}")]
        public async Task<ActionResult<PublicUserDTO>> GetUser(string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x=> x.Name==username);
            if (user == null) return NotFound();
            return new PublicUserDTO
            {
                Id = user.Id,
                Username = user.Name,
                About = user.About
            };
        }

        [Authorize]
        [HttpPut("{username}")]
        public async Task<IActionResult> PutUser(string username, PublicUserDTO _user)
        {
            try
            {
                if (username != _user.Username) return BadRequest();
                var user = await _context.Users.SingleOrDefaultAsync(x => x.Name == username);
                if (user == null) return NotFound();
                var _username = GetCurrentUsername();
                if (user.Name != _username) return Unauthorized("You are not authorized to modify this user!");
                user.About = _user.About;
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }
        private string GetCurrentUsername()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;

                return userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            }

            return null;
        }
    }
}
