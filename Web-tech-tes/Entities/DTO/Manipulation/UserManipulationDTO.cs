using Entities.Models;

namespace Entities.DTO.Manipulation
{
    public abstract class UserManipulationDTO
    {
        public virtual ICollection<RoleDTO> Roles { get; set; }
    }
}