using ItemsService.Model;
using Microsoft.EntityFrameworkCore;

namespace ItemsService.Data.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Item> Items { get; set; }
    }
}
