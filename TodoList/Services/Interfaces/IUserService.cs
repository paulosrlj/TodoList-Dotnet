using TodoList.Models.Domain;
using TodoList.Models.DTO.Inbound;

namespace TodoList.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> Create(InboundUser inboundUser);
    }
}
