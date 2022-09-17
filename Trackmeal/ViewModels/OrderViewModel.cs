using Trackmeal.Models;

namespace Trackmeal.ViewModels
{
    public class OrderViewModel
    {
        public IEnumerable<Product> Products { get; set; } = null!;
        public IEnumerable<CartEntry> CartEntries { get; set; } = null!;

        public int ProductAmountInCart(Product product)
        {
            var entry = CartEntries.SingleOrDefault(entry => entry.Product.Id == product.Id);
            return entry?.Amount ?? 0;
        }
    }
}
