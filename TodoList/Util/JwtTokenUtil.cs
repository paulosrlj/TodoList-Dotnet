using Microsoft.IdentityModel.Tokens;
using static TodoList.Models.Domain.JwtToken;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoList.Models.Domain;

namespace TodoList.Util
{
    public class JwtTokenUtil
    {

        public IConfiguration _configuration;

        public JwtTokenUtil(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public UserToken BuildToken(User userInfo, Claim[] claims)
        {
            var jwtKey = _configuration["JWT:key"];
            var expireTime = int.Parse(_configuration["JWT:ExpireTime"]);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // tempo de expiração do token: 1 hora
            var expiration = DateTime.UtcNow.AddHours(expireTime);
            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds
            );

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}
