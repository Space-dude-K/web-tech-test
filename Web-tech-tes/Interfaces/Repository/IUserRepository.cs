using Entities.Models;
using Entities.RequestFeatures;
using Entities.RequestFeatures.User;

namespace Repository
{
    public interface IUserRepository
    {
        Task<PagedList<User>> GetUsersAsync(UserParameters userParameters, bool trackChanges);
        Task<User> GetUserAsync(int userId, bool trackChanges);
        void CreateUser(User user);
        void DeleteUser(User user);
        void UpdateUser(User user);
    }
}