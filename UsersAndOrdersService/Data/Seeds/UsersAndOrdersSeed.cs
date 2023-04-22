using UsersAndOrdersService.Data.Context;
using UsersAndOrdersService.Data.Repositories;
using UsersAndOrdersService.Model;

namespace UsersAndOrdersService.Data.Seeds
{
    public class UsersAndOrdersSeed
    {
        private readonly DataContext dataContext;
        public UsersAndOrdersSeed(DataContext context)
        {
            this.dataContext = context;
        }

        public void SeedDataContext()
        {
            if (!dataContext.Orders.Any())
            {
                var users = new List<User>()
                {
                    new User {
                        Name = "Александр",
                        Email = "alex@mail.ru",
                        Password = UserRepository.Encrypt("12345"),
                        Role = UserRole.User,
                        Orders = new List<Order>()
                        {
                            new Order {
                                UserId = 1,
                                DateTime = DateTime.Now,
                                OrderedItems = new List<OrderedItem> { 
                                    new OrderedItem(){ OrderId = 1, ItemId = 1, Quantity = 4},
                                    new OrderedItem(){ OrderId = 1, ItemId = 2, Quantity = 6},
                                    new OrderedItem(){ OrderId = 1, ItemId = 3, Quantity = 2},
                                }},
                            new Order {
                                UserId = 1,
                                DateTime = DateTime.Now,
                                OrderedItems = new List<OrderedItem> {
                                    new OrderedItem(){ OrderId = 2, ItemId = 1, Quantity = 7},
                                    new OrderedItem(){ OrderId = 2, ItemId = 2, Quantity = 3},
                                    new OrderedItem(){ OrderId = 2, ItemId = 3, Quantity = 2},
                                }},

                        }},
                    new User {
                        Name = "Вадим",
                        Email = "vadim@mail.ru",
                        Password = UserRepository.Encrypt("34523"),
                        Role = UserRole.User,
                        Orders = new List<Order>()
                        {
                            new Order {
                                UserId = 2,
                                DateTime = DateTime.Now,
                                OrderedItems = new List<OrderedItem> {
                                    new OrderedItem(){ OrderId = 3, ItemId = 4, Quantity = 64},
                                    new OrderedItem(){ OrderId = 3, ItemId = 5, Quantity = 2},
                                    new OrderedItem(){ OrderId = 3, ItemId = 6, Quantity = 34},
                                }},
                            new Order {
                                UserId = 2,
                                DateTime = DateTime.Now,
                                OrderedItems = new List<OrderedItem> {
                                    new OrderedItem(){ OrderId = 4, ItemId = 4, Quantity = 2},
                                    new OrderedItem(){ OrderId = 4, ItemId = 5, Quantity = 1},
                                    new OrderedItem(){ OrderId = 4, ItemId = 6, Quantity = 6},
                                }},

                        }},
                    new User {
                        Name = "Дмитрий",
                        Email = "dima@mail.ru",
                        Password = UserRepository.Encrypt("45763"),
                        Role = UserRole.User,
                        Orders = new List<Order>()
                        {
                            new Order {
                                UserId = 3,
                                DateTime = DateTime.Now,
                                OrderedItems = new List<OrderedItem> {
                                    new OrderedItem(){ OrderId = 5, ItemId = 7, Quantity = 64},
                                    new OrderedItem(){ OrderId = 5, ItemId = 8, Quantity = 2},
                                    new OrderedItem(){ OrderId = 5, ItemId = 9, Quantity = 34},
                                }},
                            new Order {
                                UserId = 3,
                                DateTime = DateTime.Now,
                                OrderedItems = new List<OrderedItem> {
                                    new OrderedItem(){ OrderId = 6, ItemId = 7, Quantity = 2},
                                    new OrderedItem(){ OrderId = 6, ItemId = 8, Quantity = 1},
                                    new OrderedItem(){ OrderId = 6, ItemId = 9, Quantity = 6},
                                }},

                        }},
                };

                dataContext.Users.AddRange(users);
                dataContext.SaveChanges();
            }
        }
    }
}
