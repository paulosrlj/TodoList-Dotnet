using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoList.Data;
using TodoList.Models.Domain;
using TodoList.Models.DTO;
using TodoList.Models.DTO.Inbound;
using TodoList.Services.Interfaces;
using TodoList.Util;
using static TodoList.Models.Domain.JwtToken;

namespace TodoList.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly JwtTokenUtil _jwtTokenUtil;
        private readonly TodoListDbContext _ctx;
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InboundUser model)
        {
            try
            {
                var result = await _userService.Create(model);
                return Created("/create", result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel(ex.Message));
            }
        }
    }
}
