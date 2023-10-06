using Entities.Models;
using Entities.RequestFeatures.User;
using Entities.RequestFeatures;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
