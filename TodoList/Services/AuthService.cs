using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using static TodoList.Models.Domain.JwtToken;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoList.Models.Domain;
using TodoList.Models.DTO.Inbound;
using TodoList.Enums.Roles;
using TodoList.Exceptions;
using TodoList.Services.Interfaces;

namespace TodoList.Services
{
    public class AuthService : IAuthService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration, RoleManager<IdentityRole> roleManager,
            ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<UserToken> Login(LoginUser userInfo)
        {
            var user = await _userManager.FindByEmailAsync(userInfo.Email);

            if (user == null)
            {
                throw new DoesntExistsException("O usuário não existe.");
            }

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName,
                userInfo.Password,
                isPersistent: false,
                lockoutOnFailure: false
                );

            if (result.Succeeded)
            {
                var token = await BuildToken(userInfo, user);
                return token;
            }
            else
            {
                throw new Exception("Email ou senha inválidos!");
            }
        }

        private async Task<UserToken> BuildToken(LoginUser userInfo, ApplicationUser userFound)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim("TipoUsuario", Roles.Usuario.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(userFound);

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // tempo de expiração do token: 1 hora
            var expiration = DateTime.UtcNow.AddHours(double.Parse(_configuration["JWT:ExpireTime"]));

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
