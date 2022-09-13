﻿using Trackmeal.Models;

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
        public Task AddProductAsync(int productId);
        public Task RemoveProductAsync(int productId);
        public Task DeleteEntryAsync(int entryId);
        public Task ClearCartAsync();
    }
}
