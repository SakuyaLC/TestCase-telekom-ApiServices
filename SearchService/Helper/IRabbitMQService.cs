namespace SearchService.Helper
{
    public interface IRabbitMQService
    {
        void SendMessage(object obj);
        void SendMessage(string message);
    }
}
