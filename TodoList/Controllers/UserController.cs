﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoList.Data;
using TodoList.Models.Domain;
using TodoList.Models.DTO;
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

        public UserController(UserManager<ApplicationUser> userManager,
            IConfiguration configuration, JwtTokenUtil jwtTokenUtil, TodoListDbContext ctx)
        {
            _userManager = userManager;
            _configuration = configuration;
            _jwtTokenUtil = jwtTokenUtil;
            _ctx = ctx;
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User model)
        {
            var user = new ApplicationUser { UserName = model.Username, Email = model.Email };

            var result = await _userManager.CreateAsync(user, model.Password);

            var errors = result.Errors.Select(e => e.Description);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, model.Username),
                new Claim(JwtRegisteredClaimNames.Email, model.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (result.Succeeded)
            {
                _ctx.Users.Add(model);
                await _ctx.SaveChangesAsync();

                var token = _jwtTokenUtil.BuildToken(model, claims);
                return Ok(new ResponseModel(token));
            }
            else
            {
                return BadRequest(new ResponseModel(errors));
            }

        }

    }
}
