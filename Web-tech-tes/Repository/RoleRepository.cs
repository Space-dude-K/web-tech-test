using Entities.Models;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class RoleRepository : RepositoryBase<Role, WebApiDbContext>, IRoleRepository
    {
        public RoleRepository(WebApiDbContext repositoryContext) : base(repositoryContext)
        {
        }
        public async Task<ICollection<Role>> GetRolesAsync(bool trackChanges)
        {
            return await FindAll(trackChanges)
             .OrderBy(e => e.Name)
             .ToListAsync();
        }

        public async Task<Role> GetRoleAsync(int roleId, bool trackChanges)
        {
            return await FindByCondition(role => role.Id == roleId, trackChanges)
             .SingleOrDefaultAsync();
        }
    }
}
