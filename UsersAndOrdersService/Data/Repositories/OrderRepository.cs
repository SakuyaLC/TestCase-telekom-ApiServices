using Microsoft.EntityFrameworkCore;
using UsersAndOrdersService.Data.Context;
using UsersAndOrdersService.Data.Interfaces;
using UsersAndOrdersService.Model;

namespace UsersAndOrdersService.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _context;

        public OrderRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> OrderExists(int Id)
        {
            return await _context.Orders.AnyAsync(i => i.Id == Id);
        }

        public async Task<ICollection<Order>> GetOrders()
        {
            return await _context.Orders.OrderBy(i => i.Id).ToListAsync();
        }

        public async Task<Order> GetSpecificOrder(int Id)
        {
            return await _context.Orders.Where(i => i.Id == Id).SingleOrDefaultAsync();
        }

        public async Task<bool> CreateOrder(Order order)
        {
            await _context.AddAsync(order);
            return await Save();
        }

        public async Task<bool> UpdateOrder(Order order)
        {
            _context.Update(order);
            return await Save();
        }

        public async Task<bool> DeleteOrder(Order order)
        {
            _context.Remove(order);
            return await Save();
        }

        public async Task<bool> Save()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
