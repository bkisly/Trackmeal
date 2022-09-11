using System.ComponentModel.DataAnnotations;
using Trackmeal.Models;

namespace Trackmeal.ViewModels
{
    public class ProductFormViewModel
    {
        public int Id { get; set; }
        [StringLength(100)] public string Name { get; set; } = string.Empty;
        [StringLength(1000)] public string? Description { get; set; }
        [Required, Range(0, 10000)] public decimal? Price { get; set; }

        public ProductFormViewModel()
        {
        }

        public ProductFormViewModel(Product model)
        {
            Id = model.Id;
            Name = model.Name;
            Description = model.Description;
            Price = model.Price;
        }
    }
}
