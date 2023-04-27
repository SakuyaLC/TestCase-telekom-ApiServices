using UsersAndOrdersService.Model;

namespace UsersAndOrdersService.Data.Interfaces
{
    public interface IOrderedItemsRepository
    {
        Task<bool> OrderedItemExists(int Id);
        Task<ICollection<OrderedItem>> GetOrderedItems();
        Task<OrderedItem> GetSpecificOrderedItem(int Id);
        Task<bool> CreateOrderedItem(OrderedItem orderedItem);
        Task<bool> UpdateOrderedItem(OrderedItem orderedItem);
        Task<bool> DeleteOrderedItem(OrderedItem orderedItem);
        Task<ICollection<OrderedItem>> GetOrderedItemsForOrder(int Id);
        Task<bool> Save();
    }
}
