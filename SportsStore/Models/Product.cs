using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsStore.Models
{
    public class Product
    {
        public long ProductID { get; set; }
        [Required(ErrorMessage = "Please, enter a product name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please, enter a product description")]
        public string Description { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please, enter a positive value")]
        [Column(TypeName ="decimal(8, 2)")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Please, specify a product category")]
        public string Category { get; set; }
    }
}
