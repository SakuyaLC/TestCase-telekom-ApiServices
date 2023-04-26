using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SearchService.Data.DTO;
using SearchService.Model;
using System.Text;
using System.Text.Json;
using SearchService.Data.DTO;
using static SearchService.Helper.IRabbitMQService;

namespace SearchService.Helper
{
    public class RabbitMQService : IRabbitMQService
    {
        public void SendMessage(object obj, string type)
        {
            var message = JsonSerializer.Serialize(obj);
            SendMessage(message, type);
        }

        public void SendMessage(string message, string type)
        {

            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672, // порт RabbitMQ по умолчанию
                UserName = "guest", // имя пользователя по умолчанию
                Password = "guest" // пароль пользователя по умолчанию
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                if (type.Equals("Item"))
                {

                    channel.ExchangeDeclare(exchange: "item-search", type: "direct");

                    var body = Encoding.UTF8.GetBytes(message);

                    var routingKey = "ItemForSearch";

                    channel.BasicPublish(exchange: "item-search",
                                         routingKey: routingKey,
                                         basicProperties: null,
                                         body: body);

                    Console.WriteLine("Sent message: {0}", message);
                }

                if (type.Equals("User"))
                {

                    channel.ExchangeDeclare(exchange: "user-search", type: "direct");

                    var body = Encoding.UTF8.GetBytes(message);

                    var routingKey = "UserForSearch";

                    channel.BasicPublish(exchange: "user-search",
                                         routingKey: routingKey,
                                         basicProperties: null,
                                         body: body);

                    Console.WriteLine("Sent message: {0}", message);
                }
            }
        }

        public async Task<ICollection<Item>> RecieveItem()
        {
            ICollection<Item> itemsResult = new List<Item>();

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "SearchItem",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);

                var taskCompletionSource = new TaskCompletionSource<ICollection<Item>>();

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    itemsResult = JsonSerializer.Deserialize<ICollection<Item>>(message);
                    Console.WriteLine("Received message: {0}", message);
                    taskCompletionSource.SetResult(itemsResult);
                };
                channel.BasicConsume(queue: "SearchItem",
                                     autoAck: true,
                                     consumer: consumer);

                return await taskCompletionSource.Task;
            }
        }

        public async Task<ICollection<UserDTO>> RecieveUser()
        {
            ICollection<UserDTO> userReusult = new List<UserDTO>();

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "SearchUser",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);

                var taskCompletionSource = new TaskCompletionSource<ICollection<UserDTO>>();

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    userReusult = JsonSerializer.Deserialize<ICollection<UserDTO>>(message);
                    Console.WriteLine("Received message: {0}", message);
                    taskCompletionSource.SetResult(userReusult);
                };
                channel.BasicConsume(queue: "SearchUser",
                                     autoAck: true,
                                     consumer: consumer);

                return await taskCompletionSource.Task;
            }
        }
    }
}
