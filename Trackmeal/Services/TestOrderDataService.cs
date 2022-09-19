using Trackmeal.Models;

namespace Trackmeal.Services
{
    public class TestOrderDataService : IModifiableDataService<Order>
    {
        private readonly List<Order> _orders = new();

        public Task<Order[]> GetItemsAsync()
        {
            return Task.FromResult(_orders.ToArray());
        }

        public Task<Order> GetItemByIdAsync(int id)
        {
            return Task.FromResult(_orders.Single(o => o.Id == id));
        }

        public Task AddItemAsync(Order item)
        {
            _orders.Add(item);
            return Task.CompletedTask;
        }

        public Task EditItemAsync(int id, Order newItemData)
        {
            var orderToModify = _orders.Single(o => o.Id == id);
            orderToModify.DateOrdered = newItemData.DateOrdered;
            orderToModify.Token = newItemData.Token;
            orderToModify.Entries = newItemData.Entries;

            return Task.CompletedTask;
        }

        public Task DeleteItemAsync(int id)
        {
            var orderToRemove = _orders.Single(o => o.Id == id);
            _orders.Remove(orderToRemove);
            return Task.CompletedTask;
        }
    }
}
