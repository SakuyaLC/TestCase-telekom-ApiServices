using SearchService.Data.DTO;
using SearchService.Model;

namespace SearchService.Helper
{
    public interface IRabbitMQService
    {
        void SendMessage(object obj, string type);
        void SendMessage(string message, string type);
        Task<ICollection<Item>> RecieveItem();
        Task<ICollection<UserDTO>> RecieveUser();
    }
}
