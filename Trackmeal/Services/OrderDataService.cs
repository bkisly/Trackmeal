using Microsoft.EntityFrameworkCore;
using Trackmeal.Data;
using Trackmeal.Models;

namespace Trackmeal.Services
{
    public class OrderDataService : IModifiableDataService<Order>
    {
        private readonly ApplicationDbContext _context;

        public OrderDataService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order[]> GetItemsAsync()
        {
            return await _context.Orders
                .Include(order => order.OrderStatus)
                .Include(order => order.Entries)
                .ThenInclude(entry => entry.Product)
                .ToArrayAsync();
        }

        public async Task<Order> GetItemByIdAsync(int id)
        {
            return await _context.Orders
                .Include(order => order.OrderStatus)
                .Include(order => order.Entries)
                .ThenInclude(entry => entry.Product)
                .SingleAsync(order => order.Id == id);
        }

        public async Task AddItemAsync(Order item)
        {
            item.DateOrdered = DateTime.Now;
            item.Token = Guid.NewGuid();

            await _context.Orders.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task EditItemAsync(int id, Order newItemData)
        {
            var orderToEdit = await _context.Orders.SingleAsync(order => order.Id == id);
            orderToEdit.DateOrdered = newItemData.DateOrdered;
            orderToEdit.Token = newItemData.Token;
            orderToEdit.Entries = newItemData.Entries;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteItemAsync(int id)
        {
            _context.Orders.Remove(await _context.Orders.SingleAsync(order => order.Id == id));
            await _context.SaveChangesAsync();
        }
    }
}
