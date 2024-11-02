using System.ComponentModel.DataAnnotations;

namespace TestWebStagiaire2024.Models.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public List<Item> GroceryList { get; set; } = new List<Item>();

        public bool IsDeleted { get; set; } = false;
    }
}
