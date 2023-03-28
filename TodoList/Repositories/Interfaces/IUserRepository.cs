using TodoList.Models.Domain;

namespace TodoList.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> Create(User user);
        Task<User> FindById(Guid id);
        Task<User> FindByEmail(string email);
    }
}
