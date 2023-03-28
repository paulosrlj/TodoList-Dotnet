using Microsoft.EntityFrameworkCore;
using TodoList.Data;
using TodoList.Models.Domain;
using TodoList.Repositories.Interfaces;

namespace TodoList.Repositories.Concrete
{
    public class UserRepository : IUserRepository
    {

        private readonly TodoListDbContext _ctx;
        private readonly ILogger<IUserRepository> _logger;

        public UserRepository(TodoListDbContext ctx, ILogger<IUserRepository> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        public async Task<User> Create(User user)
        {
            await _ctx.AddAsync(user);
            await _ctx.SaveChangesAsync();
            return user;
        }

        public async Task<User> FindByEmail(string email)
        {
            var user = await _ctx.Users.FirstAsync(u => u.Email == email);
            return user;
        }

        public async Task<User> FindById(Guid id)
        {
            var user = await _ctx.Users.FirstAsync(u => u.Id == id);
            return user;
        }
    }
}
