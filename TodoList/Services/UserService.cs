using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using TodoList.Controllers;
using TodoList.Enums.Roles;
using TodoList.Exceptions;
using TodoList.Models.Domain;
using TodoList.Models.DTO.Inbound;
using TodoList.Repositories.Interfaces;
using TodoList.Services.Interfaces;

namespace TodoList.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<IUserService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(ILogger<IUserService> logger, IUserRepository userRepository, 
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<User> Create(InboundUser inboundUser)
        {
            var alreadyExists = await _userManager.FindByEmailAsync(inboundUser.Email);

            if (alreadyExists != null)
            {
                throw new AlreadyExistsException("O usuário já existe.");
            }

            var result = await CreateAuthUser(inboundUser);
            if (result.Any())
            {
                throw new UserManagerException(result);
            }

            User user = new();
            Reflection.CopyProperties(inboundUser, user);

            return await _userRepository.Create(user);
        }

        private async Task<IEnumerable<string>> CreateAuthUser(InboundUser model)
        {
            var user = new ApplicationUser { UserName = model.Username, Email = model.Email };

            bool userRoleExists = await _roleManager.RoleExistsAsync(Roles.Usuario.ToString());

            if (!userRoleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.Usuario.ToString()));
            }

            var result = await _userManager.CreateAsync(user, model.Password);

            await _userManager.AddToRoleAsync(user, Roles.Usuario.ToString());

            var errors = result.Errors.Select(e => e.Description);

            if (errors.Any())
            {
                return errors;
            }

            return Array.Empty<string>();
        }
    }
}
