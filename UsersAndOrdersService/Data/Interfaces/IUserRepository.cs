using UsersAndOrdersService.Model;

namespace UsersAndOrdersService.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> UserExists(int Id);
        Task<ICollection<User>> GetUsers();
        Task<User> GetSpecificUser(int Id);
        Task<bool> CreateUser(User user);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(User user);
        Task<bool> Save();
    }
}
