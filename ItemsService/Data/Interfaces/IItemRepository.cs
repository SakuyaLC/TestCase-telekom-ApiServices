using ItemsService.Model;
using System.Collections;

namespace ItemsService.Data.Interfaces
{
    public interface IItemRepository
    {
        Task<bool> ItemExists(int Id);
        Task<ICollection<Item>> GetItems();
        Task<Item> GetSpecificItem(int Id);
        Task<bool> CreateItem(Item item);
        Task<bool> UpdateItem(Item item);
        Task<bool> DeleteItem(Item item);
        Task<bool> Save();
    }
}
