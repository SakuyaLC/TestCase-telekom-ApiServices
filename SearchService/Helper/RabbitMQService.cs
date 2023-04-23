using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using static SearchService.Helper.IRabbitMQService;

namespace SearchService.Helper
{
    public class RabbitMQService : IRabbitMQService
    {
        public void SendMessage(object obj)
        {
            var message = JsonSerializer.Serialize(obj);
            SendMessage(message);
        }

        public void SendMessage(string message)
        {
            // Не забудьте вынести значения "localhost" и "MyQueue"
            // в файл конфигурации
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
                channel.QueueDeclare(queue: "SearchItem",
                               durable: false,
                               exclusive: false,
                               autoDelete: false,
                               arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                               routingKey: "SearchItem",
                               basicProperties: null,
                               body: body);
            }
        }
    }
}
