using Microsoft.EntityFrameworkCore;
using Trackmeal.Data;
using Trackmeal.Models;

namespace Trackmeal.Services
{
    public class CartDataService : ICartDataService
    {
        private readonly ApplicationDbContext _context;
        private readonly IModifiableDataService<Product> _productsService;

        public CartDataService(ApplicationDbContext context, IModifiableDataService<Product> productsService)
        {
            _context = context;
            _productsService = productsService;
        }

        public async Task<CartEntry[]> GetItemsAsync()
        {
            return await _context.Cart
                .Include(entry => entry.Product)
                .Where(entry => !entry.OrderId.HasValue)
                .ToArrayAsync();
        }

        public async Task<CartEntry> GetItemByIdAsync(int id)
        {
            return await _context.Cart
                .Include(entry => entry.Product)
                .SingleAsync(entry => entry.Id == id);
        }

        public async Task AddProductAsync(int productId)
        {
            var cart = _context.Cart;
            var entryToModify = await cart.SingleOrDefaultAsync(entry => entry.Product.Id == productId && !entry.OrderId.HasValue);

            if (entryToModify is not null) entryToModify.Amount++;
            else
            {
                CartEntry newEntry = new()
                {
                    Amount = 1,
                    Product = await _productsService.GetItemByIdAsync(productId)
                };

                await cart.AddAsync(newEntry);
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveProductAsync(int productId)
        {
            var entryToModify = await _context.Cart.SingleAsync(entry => entry.Product.Id == productId && !entry.OrderId.HasValue);
            if (entryToModify.Amount == 1) _context.Cart.Remove(entryToModify);
            else entryToModify.Amount--;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteEntryAsync(int entryId)
        {
            _context.Cart.Remove(await _context.Cart.SingleAsync(entry => entry.Id == entryId));
            await _context.SaveChangesAsync();
        }

        public async Task ClearCartAsync()
        {
            var entriesToRemove = await _context.Cart.Where(entry => !entry.OrderId.HasValue).ToArrayAsync();
            foreach (var entry in entriesToRemove)
                _context.Cart.Remove(entry);

            await _context.SaveChangesAsync();
        }
    }
}
