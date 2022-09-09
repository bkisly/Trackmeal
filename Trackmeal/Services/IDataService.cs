namespace Trackmeal.Services
{
    public interface IDataService<T>
    {
        public Task<T[]> GetItemsAsync();
        public Task<T> GetItemByIdAsync(int id);
        public Task AddItemAsync(T item);
        public Task EditItemAsync(int id, T newItemData);
    }
}
