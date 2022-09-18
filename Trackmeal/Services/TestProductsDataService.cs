using Trackmeal.Models;

namespace Trackmeal.Services
{
    public class TestProductsDataService : IModifiableDataService<Product>
    {
        private readonly List<Product> _products = new()
        {
            new Product { Id = 1, Name = "Burger", Description = "Tasty burger", Price = 9.99M },
            new Product { Id = 2, Name = "Pizza", Description = "Tasty pizza", Price = 12.99M },
            new Product { Id = 3, Name = "Coffee", Description = "Tasty coffee", Price = 3.99M },
        };

        public Task<Product[]> GetItemsAsync()
        {
            return Task.FromResult(_products.ToArray());
        }

        public Task<Product> GetItemByIdAsync(int id)
        {
            return Task.FromResult(_products.Single(p => p.Id == id));
        }

        public Task AddItemAsync(Product item)
        {
            item.Id = _products.Any() ? _products.Select(p => p.Id).Max() + 1 : 1;
            _products.Add(item);
            return Task.CompletedTask;
        }

        public Task EditItemAsync(int id, Product newItemData)
        {
            var itemToUpdate = _products.Single(item => item.Id == id);
            itemToUpdate.Name = newItemData.Name;
            itemToUpdate.Description = newItemData.Description;
            itemToUpdate.Price = newItemData.Price;
            return Task.CompletedTask;
        }

        public Task DeleteItemAsync(int id)
        {
            var productToDelete = _products.Single(p => p.Id == id);
            _products.Remove(productToDelete);
            return Task.CompletedTask;
        }
    }
}
