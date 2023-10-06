using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Entities.RequestFeatures.User;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class UserRepository : RepositoryBase<User, WebApiDbContext>, IUserRepository
    {
        public UserRepository(WebApiDbContext repositoryContext) : base(repositoryContext)
        {
        }
        public void CreateUser(User user)
        {
            Create(user);
        }
        public void UpdateUser(User user)
        {
            Update(user);
        }
        public void DeleteUser(User user)
        {
            Delete(user);
        }

        public async Task<PagedList<User>> GetUsersAsync(UserParameters userParameters, bool trackChanges)
        {
            var users = await FindAll(trackChanges)
             .OrderBy(e => e.Name)
             .ToListAsync();

            return PagedList<User>
                .ToPagedList(users, userParameters.PageNumber, userParameters.PageSize);
        }
        public async Task<PagedList<User>> GetUsersWithRolesAsync(UserParameters userParameters, bool trackChanges)
        {
            var users = await FindByCondition(u => 
                u.Id >= userParameters.MinId && u.Id <= userParameters.MaxId && 
                u.Name.Length >= userParameters.MinNameLength && u.Name.Length <= userParameters.MaxNameLength &&
                u.Email.Length >= userParameters.MinEmailLength && u.Email.Length <= userParameters.MaxEmailLength &&
                u.Age >= userParameters.MinAge && u.Age <= userParameters.MaxAge,
                trackChanges)
             .Include(e => e.Roles)
             .OrderBy(e => e.Name)
             .ToListAsync();

            return PagedList<User>
                .ToPagedList(users, userParameters.PageNumber, userParameters.PageSize);
        }

        public async Task<User> GetUserAsync(int userId, bool trackChanges)
        {
            return await FindByCondition(user => user.Id == userId, trackChanges)
             .SingleOrDefaultAsync();
        }
    }
}