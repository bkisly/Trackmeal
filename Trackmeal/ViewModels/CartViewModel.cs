using Trackmeal.Models;

namespace Trackmeal.ViewModels
{
    public class CartViewModel
    {
        public IEnumerable<CartEntry> CartEntries { get; set; } = null!;
        public decimal TotalPrice => CartEntries.Sum(GetTotalEntryPrice);

        public decimal GetTotalEntryPrice(CartEntry entry) => entry.Product.Price * entry.Amount;
    }
}
