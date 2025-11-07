using System.ComponentModel.DataAnnotations;

namespace Blog.Models.ViewModels
{

    public class RegisterViewModel
    {

        [Required(ErrorMessage = "Nome Obrigatorio")]

        public string Name { get; set; }

        [Required(ErrorMessage = "No E-mail Obrigatorio")]
        [EmailAddress(ErrorMessage = "Email Invalido")]

        public string Email { get; set; }



    }
}