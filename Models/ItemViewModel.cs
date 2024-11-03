using System.ComponentModel.DataAnnotations;

namespace TestWebStagiaire2024.Models
{
    public class ItemViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Nom requis")]
        [StringLength(50, ErrorMessage = "Maximum 50 caractères")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Quantité requise")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantité minimale à 1")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Prix requis")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Prix minimum à 0.01")]
        public decimal Price { get; set; }
    }
}