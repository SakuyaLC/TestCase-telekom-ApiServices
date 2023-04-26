using ItemsService.Model;
using System.Runtime.CompilerServices;

namespace ItemsService.Helper
{
    public interface IRabbitMQService
    {
        void SendMessage(object obj);
        void SendMessage(string message);
        Task<ItemForSearch> ReceiveSearchItem();
    }
}
