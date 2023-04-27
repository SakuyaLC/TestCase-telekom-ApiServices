using Microsoft.EntityFrameworkCore;
using UsersAndOrdersService.Data.Context;
using UsersAndOrdersService.Data.Interfaces;
using UsersAndOrdersService.Model;

namespace UsersAndOrdersService.Data.Repositories
{
    public class OrderedItemsRepository : IOrderedItemsRepository
    {
        private readonly DataContext _context;

        public OrderedItemsRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> OrderedItemExists(int Id)
        {
            return await _context.OrderedItems.AnyAsync(o => o.Id == Id);
        }

        public async Task<ICollection<OrderedItem>> GetOrderedItems()
        {
            return await _context.OrderedItems.OrderBy(o => o.Id).ToListAsync();
        }

        public async Task<OrderedItem> GetSpecificOrderedItem(int Id)
        {
            return await _context.OrderedItems.Where(o => o.Id == Id).SingleOrDefaultAsync();
        }

        public async Task<bool> CreateOrderedItem(OrderedItem orderedItem)
        {
            await _context.AddAsync(orderedItem);
            return await Save();
        }

        public async Task<bool> UpdateOrderedItem(OrderedItem orderedItem)
        {
            _context.Update(orderedItem);
            return await Save();
        }

        public async Task<bool> DeleteOrderedItem(OrderedItem orderedItem)
        {
            _context.Remove(orderedItem);
            return await Save();
        }

        public async Task<ICollection<OrderedItem>> GetOrderedItemsForOrder(int Id)
        {
            return await _context.OrderedItems.Where(o => o.OrderId == Id).OrderBy(o => o.Id).ToListAsync();
        }

        public async Task<bool> Save()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
