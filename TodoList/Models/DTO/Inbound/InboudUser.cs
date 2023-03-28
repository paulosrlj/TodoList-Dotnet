using System.ComponentModel.DataAnnotations;

namespace TodoList.Models.DTO.Inbound
{
    public class InboundUser
    {
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Nome de usuário necessário!")]
        public string Username { get; set; }


        [Required(ErrorMessage = "Email necessário!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha necessária!")]
        public string Password { get; set; }

    }
}