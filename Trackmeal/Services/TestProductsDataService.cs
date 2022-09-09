using Trackmeal.Models;

namespace Trackmeal.Services
{
    public class TestProductsDataService : IDataService<Product>
    {
        private readonly Product[] products = new Product[3]
        {
            new() { Id = 1, Name = "Burger", Description = "Tasty burger", Price = 9.99M },
            new() { Id = 2, Name = "Pizza", Description = "Tasty pizza", Price = 12.99M },
            new() { Id = 3, Name = "Coffee", Description = "Tasty coffee", Price = 3.99M },
        };

        public Task<Product> GetItemByIdAsync(int id)
        {
            return Task.FromResult(products.Single(p => p.Id == id));
        }

        public Task<Product[]> GetItemsAsync()
        {
            return Task.FromResult(products);
        }
    }
}
