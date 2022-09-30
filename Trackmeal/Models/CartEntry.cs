using System.ComponentModel.DataAnnotations;

namespace Trackmeal.Models
{
    public class CartEntry
    {
        public short Id { get; set; }
        public string? UserId { get; set; }
        [Range(0, 100)] public byte Amount { get; set; }

        public Product Product { get; set; } = null!;
        public int? OrderId { get; set; }

        public decimal TotalPrice => Product.Price * Amount;
    }
}
