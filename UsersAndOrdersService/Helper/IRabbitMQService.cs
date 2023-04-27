using UsersAndOrdersService.Model;
using System.Runtime.CompilerServices;

namespace UsersAndOrdersService.Helper
{
    public interface IRabbitMQService
    {
        void SendMessage(object obj, string type);
        void SendMessage(string message, string type);
        Task<UserForSearch> ReceiveSearchUser();
    }
}
