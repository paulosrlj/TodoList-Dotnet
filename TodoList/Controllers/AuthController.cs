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
using TodoList.Services;
using TodoList.Services.Interfaces;
using TodoList.Models.DTO.Inbound;

namespace TodoList.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger,
            IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("API is fine");
        }

        ///// <summary>
        ///// Cria um usuário
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[HttpPost("Criar")]
        //public async Task<IActionResult> CreateUser([FromBody] User model)
        //{
        //    try
        //    {
        //        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

        //        bool userRoleExists = await _roleManager.RoleExistsAsync("Usuário");
        //        if (!userRoleExists)
        //        {
        //            await _roleManager.CreateAsync(new IdentityRole("Usuário"));
        //        }

        //        var result = await _userManager.CreateAsync(user, model.Password);
        //        await _userManager.AddToRoleAsync(user, "Usuário");

        //        var errors = result.Errors.Select(e => e.Description);

        //        if (result.Succeeded)
        //        {
        //            var token = BuildToken(model);
        //            return Ok(new ResponseModel(token));
        //        }
        //        else
        //        {
        //            return BadRequest(new ResponseModel(errors));
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError("{0}", e);
        //        return BadRequest(e);
        //    }
        //}


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
        public async Task<IActionResult> Login([FromBody] LoginUser userInfo)
        {
            try
            {
                var result = await _authService.Login(userInfo);
                return Ok(new ResponseModel(result.Token));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel(ex.Message));
            }
        }

    }
}
