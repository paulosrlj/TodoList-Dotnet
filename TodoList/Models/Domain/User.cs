using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoList.Models.Domain
{
    [Index(nameof(Username), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class User 
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Nome de usuário necessário!")]
        public string Username { get; set; }

        
        [Required(ErrorMessage = "Email necessário!")]
        [EmailAddress]
        
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha necessária!")]
        public string Password { get; set; }

        public List<TodoItem>? TodoItems { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
