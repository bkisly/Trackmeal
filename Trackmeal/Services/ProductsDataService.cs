using Microsoft.EntityFrameworkCore;
using Trackmeal.Data;
using Trackmeal.Models;

namespace Trackmeal.Services
{
    public class ProductsDataService : IDataService<Product>
    {
        private readonly ApplicationDbContext _context;

        public ProductsDataService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product[]> GetItemsAsync()
        {
            return await _context.Products.ToArrayAsync();
        }

        public async Task<Product> GetItemByIdAsync(int id)
        {
            return await _context.Products.SingleAsync(p => p.Id == id);
        }

        public async Task AddItemAsync(Product item)
        {
            await _context.Products.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task EditItemAsync(int id, Product newItemData)
        {
            var productToUpdate = await _context.Products.SingleAsync(item => item.Id == id);
            productToUpdate.Name = newItemData.Name;
            productToUpdate.Description = newItemData.Description;
            productToUpdate.Price = newItemData.Price;
            await _context.SaveChangesAsync();
        }
    }
}
