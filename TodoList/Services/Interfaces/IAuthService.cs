using TodoList.Models.Domain;
using TodoList.Models.DTO.Inbound;
using static TodoList.Models.Domain.JwtToken;

namespace TodoList.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserToken> Login(LoginUser userInfo);
    }
}
