using Microsoft.AspNetCore.Identity;
using Trackmeal.Models;

namespace Trackmeal.Services
{
    public interface IDataService<T>
    {
        public Task<T[]> GetItemsAsync();
        public Task<T> GetItemByIdAsync(int id);
    }

    public interface IModifiableDataService<T> : IDataService<T>
    {
        public Task AddItemAsync(T item);
        public Task EditItemAsync(int id, T newItemData);
        public Task DeleteItemAsync(int id);
    }

    public interface ICartDataService : IDataService<CartEntry>
    {
        public Task<CartEntry[]> GetAllEntries();
        public Task AddProductAsync(int productId);
        public Task RemoveProductAsync(int productId);
        public Task DeleteEntryAsync(int entryId);
        public Task ClearCartAsync();
    }

    public interface IIdentityCartDataService : ICartDataService
    {
        public Task<CartEntry[]> GetItemsAsync(IdentityUser user);
        public Task<CartEntry> GetItemByIdAsync(int id, IdentityUser user);
        public Task AddProductAsync(int productId, IdentityUser user);
        public Task RemoveProductAsync(int productId, IdentityUser user);
        public Task ClearCartAsync(IdentityUser user);
    }

    public interface IOrderDataService : IModifiableDataService<Order>
    {
        public Task<Order> GetItemByIdAsync(int id, IdentityUser user);
        public Task<Order> GetOrderByTokenAsync(Guid token);
        public Task AddItemAsync(Order item, IdentityUser user);
        public Task NextStateAsync(int orderId);
        public Task PreviousStateAsync(int orderId);
        public Task SetStateAsync(int orderId, byte stateId);
    }
}
