using ItemsService.Data.Context;
using ItemsService.Model;

namespace ItemsService.Data.Seeds
{
    public class ItemSeed
    {
        private readonly DataContext dataContext;
        public ItemSeed(DataContext context)
        {
            this.dataContext = context;
        }

        public void SeedDataContext()
        {
            if (!dataContext.Items.Any())
            {
                var items = new List<Item>()
                {
                    new Item() {Title = "Товар1", Description = "Описание товара 1", Price = 278.99},
                    new Item() {Title = "Товар2", Description = "Описание товара 2", Price = 128.99},
                    new Item() {Title = "Товар3", Description = "Описание товара 3", Price = 48.99},
                    new Item() {Title = "Товар4", Description = "Описание товара 4", Price = 346.99},
                    new Item() {Title = "Товар5", Description = "Описание товара 5", Price = 567.99},
                    new Item() {Title = "Товар6", Description = "Описание товара 6", Price = 12.99},
                    new Item() {Title = "Товар7", Description = "Описание товара 7", Price = 754.99},
                    new Item() {Title = "Товар8", Description = "Описание товара 8", Price = 2239.99},
                    new Item() {Title = "Товар9", Description = "Описание товара 9", Price = 2764.99},
                    new Item() {Title = "Товар10", Description = "Описание товара 10", Price = 2782.99},
                };

                dataContext.Items.AddRange(items);
                dataContext.SaveChanges();
            }
        }
    }
}
