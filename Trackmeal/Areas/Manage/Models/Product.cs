using System.ComponentModel.DataAnnotations;

namespace Trackmeal.Areas.Manage.Models
{
    public class Product
    {
        public int Id { get; set; }
        [StringLength(100)] public string Name { get; set; } = null!;
        [StringLength(1000)] public string? Description { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
