using System.ComponentModel.DataAnnotations;

namespace TestWebStagiaire2024.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Nom d'utilisateur requis")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mot de passe requis")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}