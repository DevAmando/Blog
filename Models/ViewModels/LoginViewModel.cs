using System.ComponentModel.DataAnnotations;
namespace Blog.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "No E-mail Obrigatorio")]
        [EmailAddress(ErrorMessage = "Email Invalido")]

        public string Email { get; set; }

        [Required(ErrorMessage = "Senha Obrigatoria")]
        public string Password { get; set; }
    }
}