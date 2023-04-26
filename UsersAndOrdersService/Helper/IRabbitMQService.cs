using UsersAndOrdersService.Model;
using System.Runtime.CompilerServices;

namespace UsersAndOrdersService.Helper
{
    public interface IRabbitMQService
    {
        void SendMessage(object obj);
        void SendMessage(string message);
        Task<UserForSearch> ReceiveSearchUser();
    }
}
