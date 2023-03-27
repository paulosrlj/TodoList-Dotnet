using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static TodoList.Models.Domain.JwtToken;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoList.Models.Domain;
using TodoList.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using Microsoft.AspNetCore.Rewrite;

namespace TodoList.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AuthController> _logger;

        public AuthController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            RoleManager<IdentityRole> roleManager, ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("API is fine");
        }

        /// <summary>
        /// Cria um usuário
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Criar")]
        public async Task<IActionResult> CreateUser([FromBody] User model)
        {
            try
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

                bool userRoleExists = await _roleManager.RoleExistsAsync("Usuário");
                if (!userRoleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole("Usuário"));
                }

                var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRoleAsync(user, "Usuário");

                var errors = result.Errors.Select(e => e.Description);

                if (result.Succeeded)
                {
                    var token = BuildToken(model);
                    return Ok(new ResponseModel(token));
                }
                else
                {
                    return BadRequest(new ResponseModel(errors));
                }
            } catch (Exception e)
            {
                _logger.LogError("{0}", e);
                return BadRequest(e);
            }
        }


        /// <summary>
        /// Realiza a autentação no sistema retornando um token JWT
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        /// <remarks>
        ///     POST /login
        ///     {
        ///        "Email": "usuario@gmail.com",
        ///        "Password": "123Token@"
        ///     }
        /// </remarks>  
        /// <response code="201">Retorna o token JWT</response>
        /// <response code="400">Se o login for inválido</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] User userInfo)    
        {
            var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password,
                 isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var token = await BuildToken(userInfo);
                return Ok(new ResponseModel(token));
            }
            else
            {
                return BadRequest(new ResponseModel(new[] { "Login ou senha inválidos!" }));
            }
        }

        private async Task<UserToken> BuildToken(User userInfo)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim("meuValor", "oque voce quiser"),
                new Claim("TipoUsuario", "Usuário"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var user = await _userManager.FindByEmailAsync(userInfo.Email);

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // tempo de expiração do token: 1 hora
            var expiration = DateTime.UtcNow.AddHours(1);
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
