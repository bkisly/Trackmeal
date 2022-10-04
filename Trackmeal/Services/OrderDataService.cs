using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Trackmeal.Data;
using Trackmeal.Models;

namespace Trackmeal.Services
{
    public class OrderDataService : IOrderDataService
    {
        private readonly ApplicationDbContext _context;
        private readonly IIdentityCartDataService _cartDataService;

        public OrderDataService(ApplicationDbContext context, IIdentityCartDataService cartDataService)
        {
            _context = context;
            _cartDataService = cartDataService;
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

        public async Task<Order> GetItemByIdAsync(int id, IdentityUser user)
        {
            return await _context.Orders
                .Include(order => order.OrderStatus)
                .Include(order => order.Entries)
                .ThenInclude(entry => entry.Product)
                .SingleAsync(order => order.Id == id && order.UserId == user.Id);
        }

        public async Task<Order> GetOrderByTokenAsync(Guid token)
        {
            return await _context.Orders
                .Include(order => order.OrderStatus)
                .Include(order => order.Entries)
                .ThenInclude(entry => entry.Product)
                .SingleAsync(order => order.Token == token);
        }

        public async Task AddItemAsync(Order item)
        {
            item.DateOrdered = DateTime.Now;
            item.Token = Guid.NewGuid();

            await _context.Orders.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task AddItemAsync(Order item, IdentityUser user)
        {
            item.UserId = user.Id;
            await AddItemAsync(item);
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
            foreach (var entry in (await _cartDataService.GetAllEntries()).Where(entry => entry.OrderId == id))
                await _cartDataService.DeleteEntryAsync(entry.Id);

            _context.Orders.Remove(await _context.Orders.SingleAsync(order => order.Id == id));
            await _context.SaveChangesAsync();
        }

        public async Task NextStateAsync(int orderId)
        {
            var order = await _context.Orders.SingleAsync(order => order.Id == orderId);
            if (order.OrderStatusId < (byte)OrderStatusEnum.Completed)
            {
                order.OrderStatusId++;
                await _context.SaveChangesAsync();
            }
        }

        public async Task PreviousStateAsync(int orderId)
        {
            var order = await _context.Orders.SingleAsync(order => order.Id == orderId);
            if (order.OrderStatusId > (byte)OrderStatusEnum.Submitted)
            {
                order.OrderStatusId--;
                await _context.SaveChangesAsync();
            }
        }

        public async Task SetStateAsync(int orderId, byte stateId)
        {
            var order = await _context.Orders.SingleAsync(order => order.Id == orderId);

            if (stateId is < (byte)OrderStatusEnum.Completed and > (byte)OrderStatusEnum.Submitted)
            {
                order.OrderStatusId = stateId;
                await _context.SaveChangesAsync();
            }
            else throw new InvalidOperationException("Tried to assign an invalid order status ID.");
        }
    }
}
