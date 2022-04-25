using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI1web.Models;

namespace WebAPI1web.Services
{
    public class TokenService
    {
        //posto ne mora napustiti  server
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }
        public string CreateToken(User user)
        {
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId,user.Name),
                new Claim(ClaimTypes.Role,user.Role)
            };
            //moze i ClaimTypes klasa umjesto JwtRegisteredClaimNames
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            //moze i direktno preko konstruktora new JwtSecurityToken, tj bez handlera + deskriptora
            return tokenHandler.WriteToken(token);
        }
    }
}
