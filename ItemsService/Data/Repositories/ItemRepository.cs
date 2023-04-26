using ItemsService.Data.Context;
using ItemsService.Data.Interfaces;
using ItemsService.Model;
using Microsoft.EntityFrameworkCore;

namespace ItemsService.Data.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly DataContext _context;

        public ItemRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> ItemExists(int Id)
        {
            return await _context.Items.AnyAsync(i => i.Id == Id);
        }

        public async Task<ICollection<Item>> GetItems()
        {
            return await _context.Items.OrderBy(i => i.Id).ToListAsync();
        }

        public async Task<Item> GetSpecificItem(int Id)
        {
            return await _context.Items.Where(i => i.Id == Id).SingleOrDefaultAsync();
        }

        public async Task<bool> CreateItem(Item item)
        {
            await _context.AddAsync(item);
            return await Save();
        }

        public async Task<bool> UpdateItem(Item item)
        {
            _context.Update(item);
            return await Save();
        }

        public async Task<bool> DeleteItem(Item item)
        {
            _context.Remove(item);
            return await Save();
        }

        public async Task<bool> Save()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<ICollection<Item>> SearchItems(ItemForSearch itemForSearch)
        {
            var query = _context.Items.AsQueryable();

            if (!string.IsNullOrEmpty(itemForSearch.Title))
            {
                query = query.Where(i => i.Title.Contains(itemForSearch.Title));
            }

            if (!string.IsNullOrEmpty(itemForSearch.Description))
            {
                query = query.Where(i => i.Description.Contains(itemForSearch.Description));
            }

            if (itemForSearch.Price != 0)
            {
                query = query.Where(i => i.Price == itemForSearch.Price);
            }

            var items = await query.ToListAsync();
            return items;
        }

    }
}
