using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecurePad.Interfaces
{
    public interface IDataStore<T>
    {

        Task AddItemAsync(T item);
        Task UpdateItemAsync(T item);
        Task DeleteItemAsync(string id, bool persist = true);
        Task DeleteEverything();
        Task<T> GetItemAsync(string id);
        Task<IEnumerable<T>> GetItemsAsync();
        Task SaveAsync();

    }
}
