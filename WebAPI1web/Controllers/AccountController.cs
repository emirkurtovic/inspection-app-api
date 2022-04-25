using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using WebAPI1web.Data;
using WebAPI1web.DTOs;
using WebAPI1web.Models;
using WebAPI1web.Services;

namespace WebAPI1web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;

        public AccountController(AppDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(LoginRegisterDTO registerDTO)
        {
            try
            {
                if (await UserExists(registerDTO.username)) return BadRequest("Username is taken");
                using var hmac = new HMACSHA512();
                var user = new User
                {
                    Name = registerDTO.username.ToLower(),
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.password)),
                    PasswordSalt = hmac.Key,
                    Role = "Member",
                    About = ""
                };
                _context.Add(user);
                await _context.SaveChangesAsync();
                return new UserDTO
                {
                    Id = user.Id,
                    Username = user.Name,
                    Token = _tokenService.CreateToken(user)
                };
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginRegisterDTO loginDTO)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Name == loginDTO.username);
            if (user == null) return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.password));

            for(int i=0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            }
            return new UserDTO
            {
                Id=user.Id,
                Username = user.Name,
                Token = _tokenService.CreateToken(user)
            };
        }


        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.Name == username.ToLower());
        }
    }
}
