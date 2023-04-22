using UsersAndOrdersService.Model;

namespace UsersAndOrdersService.Data.Interfaces
{
    public interface IOrderRepository
    {
        Task<bool> OrderExists(int Id);
        Task<ICollection<Order>> GetOrders();
        Task<Order> GetSpecificOrder(int Id);
        Task<bool> CreateOrder(Order order);
        Task<bool> UpdateOrder(Order order);
        Task<bool> DeleteOrder(Order order);
        Task<bool> Save();
    }
}
