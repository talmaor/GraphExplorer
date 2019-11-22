using System.Threading.Tasks;

namespace GraphExplorer.Utils
{
    interface IRepository<T>
    {
        Task<T> GetItemAsync(string collectionId);
        Task CreateOrUpdateItemAsync(T item, string collectionId);
        Task DeleteItemAsync(string id, string collectionId);
    }
}
