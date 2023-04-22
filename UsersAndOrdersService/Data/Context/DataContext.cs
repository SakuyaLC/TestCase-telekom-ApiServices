using Microsoft.EntityFrameworkCore;
using UsersAndOrdersService.Model;

namespace UsersAndOrdersService.Data.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderedItem> OrderedItems { get; set; }

        public void OnModelCreating(ModelBuilder modelBuilder)
        {
            // установка связи один ко многим между User и Order
            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId);

            // установка связи один ко многим между Order и OrderedItems
            modelBuilder.Entity<Order>()
                .HasMany(u => u.OrderedItems)
                .WithOne(o => o.Order)
                .HasForeignKey(o => o.OrderId);
        }
    }
}
