using Entities.Models;

namespace Repository
{
    public interface IRoleRepository
    {
        Task<Role> GetRoleAsync(int roleId, bool trackChanges);
        Task<ICollection<Role>> GetRolesAsync(bool trackChanges);
    }
}