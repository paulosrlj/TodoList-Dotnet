using System.ComponentModel.DataAnnotations;

namespace TodoList.Models.DTO.Inbound
{
    public class LoginUser
    {
        [Required(ErrorMessage = "Email necessário!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha necessária!")]
        public string Password { get; set; }
    }
}
