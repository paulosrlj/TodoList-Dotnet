using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoList.Models.Domain;

namespace TodoList.Data
{
    public class TodoListDbContext : IdentityDbContext<ApplicationUser>
    {
        public TodoListDbContext(DbContextOptions<TodoListDbContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TodoItem> Todos { get; set; }
    }
}
