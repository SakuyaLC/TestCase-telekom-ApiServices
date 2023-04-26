using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using UsersAndOrdersService.Helper;
using UsersAndOrdersService.Model;
using AutoMapper;
using System.Threading.Tasks;

namespace UsersAndOrdersService.Helper
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly IMapper _mapper;

        public RabbitMQService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public void SendMessage(object obj)
        {
            var message = JsonSerializer.Serialize(obj);
            SendMessage(message);
        }

        public void SendMessage(string message)
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

                channel.QueueDeclare(queue: "SearchUser",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                               routingKey: "SearchUser",
                               basicProperties: null,
                               body: body);

                Console.WriteLine("Sent message: {0}", message);

            }
        }

        public async Task<UserForSearch> ReceiveSearchUser()
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
                UserForSearch userForSearch = new UserForSearch();

                channel.ExchangeDeclare(exchange: "user-search", type: "direct");
                var queueName = channel.QueueDeclare().QueueName;

                var routingKey = "UserForSearch";

                channel.QueueBind(queue: queueName,
                                  exchange: "user-search",
                                  routingKey: routingKey);

                Console.WriteLine("Waiting for messages...");

                var consumer = new EventingBasicConsumer(channel);

                var taskCompletionSource = new TaskCompletionSource<UserForSearch>();

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    userForSearch = JsonSerializer.Deserialize<UserForSearch>(message);
                    Console.WriteLine("Received message: {0}", message);
                    taskCompletionSource.SetResult(userForSearch);
                };
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                return await taskCompletionSource.Task;
            }

        }
    }
}

