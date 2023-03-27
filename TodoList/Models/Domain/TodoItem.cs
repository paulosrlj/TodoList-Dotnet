using System.ComponentModel.DataAnnotations;

namespace TodoList.Models.Domain
{
    public class TodoItem
    {

        public TodoItem()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Guid UserId { get; set; }
        public User user { get; set; }
    }
}