using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Entities.RequestFeatures.Role;
using Entities.RequestFeatures.User;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;

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
        public async Task<PagedList<User>> GetUsersWithRolesAsync(
            UserParameters userParameters, 
            RoleParameters roleParameters, 
            bool trackChanges)
        {
            var users = await FindAll(trackChanges)
             .FilterUsers(userParameters)
             .Include(e => e.Roles.OrderBy(r => r.Name))
             .FilterUserRoles(roleParameters)
             .SortUsersThenByRoles(userParameters.OrderBy)
             .ToListAsync();

            return PagedList<User>
                .ToPagedList(users, userParameters.PageNumber, userParameters.PageSize);
        }

        public async Task<User> GetUserAsync(int userId, bool trackChanges)
        {
            return await FindByCondition(user => user.Id == userId, trackChanges)
                .Include(e => e.Roles.OrderBy(r => r.Name))
             .SingleOrDefaultAsync();
        }
    }
}