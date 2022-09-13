using Trackmeal.Models;

namespace Trackmeal.Services
{
    public class TestCartDataService : ICartDataService
    {
        private readonly IModifiableDataService<Product> _productsService;
        private readonly List<CartEntry> _cart = new();

        public TestCartDataService(IModifiableDataService<Product> productsService)
        {
            _productsService = productsService;
        }

        public Task<CartEntry[]> GetItemsAsync()
        {
            return Task.FromResult(_cart.ToArray());
        }

        public Task<CartEntry> GetItemByIdAsync(int id)
        {
            var entry = _cart.Single(entry => entry.Id == id);
            return Task.FromResult(entry);
        }

        public async Task AddProductAsync(int productId)
        {
            var entryToModify = _cart.SingleOrDefault(entry => entry.Product.Id == productId);

            if (entryToModify is not null) entryToModify.Amount++;
            else
            {
                CartEntry newEntry = new()
                {
                    Id = (short)(_cart.Any() ? _cart.Select(entry => entry.Id).Max() + 1 : 1),
                    Amount = 1,
                    Product = await _productsService.GetItemByIdAsync(productId)
                };

                _cart.Add(newEntry);
            }
        }

        public Task RemoveProductAsync(int productId)
        {
            var entryToModify = _cart.Single(entry => entry.Product.Id == productId);
            if (entryToModify.Amount == 1) _cart.Remove(entryToModify);
            else entryToModify.Amount--;

            return Task.CompletedTask;
        }

        public Task DeleteEntryAsync(int entryId)
        {
            _cart.Remove(_cart.Single(entry => entry.Id == entryId));
            return Task.CompletedTask;
        }

        public Task ClearCartAsync()
        {
            _cart.Clear();
            return Task.CompletedTask;
        }
    }
}
