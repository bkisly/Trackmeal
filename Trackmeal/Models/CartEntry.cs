using System.ComponentModel.DataAnnotations;

namespace Trackmeal.Models
{
    public class CartEntry
    {
        public short Id { get; set; }
        [Range(0, 100)] public byte Amount { get; set; }
        public Product Product { get; set; } = null!;
    }
}
