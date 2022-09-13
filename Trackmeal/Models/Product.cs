using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trackmeal.Models
{
    public class Product
    {
        public int Id { get; set; }
        [StringLength(100)] public string Name { get; set; } = null!;
        [StringLength(1000)] public string? Description { get; set; } = null!;
        [Range(0, 10000), Column(TypeName = "decimal(18,2)")] public decimal Price { get; set; }
    }
}
