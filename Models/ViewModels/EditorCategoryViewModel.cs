using System.ComponentModel.DataAnnotations;

namespace Blog.Models.ViewModels
{
    public class EditorCategoryViewModel
    {
        [Required(ErrorMessage = "Nome Obrigatorio")]
        [StringLength(40,MinimumLength = 3,ErrorMessage = "O nome deve ter entre 4 e 20 caracteres")]
        public string Name { get; set; }

        [Required]
        public string Slug { get; set; }
    }
}