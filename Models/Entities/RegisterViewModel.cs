using System.ComponentModel.DataAnnotations;

namespace TestWebStagiaire2024.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Nom d'utilisateur requis")]
        [StringLength(20, ErrorMessage = "Maximum 20 caractères")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mot de passe requis")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Maximum 100 caractères")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirmez le mot de passe")]
        [Compare("Password", ErrorMessage = "Le mot de passe et le mot de passe de confirmation ne correspondent pas")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
