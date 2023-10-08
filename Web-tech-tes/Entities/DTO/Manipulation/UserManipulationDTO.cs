using Entities.Models;

namespace Entities.DTO.Manipulation
{
    public abstract class UserManipulationDTO
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}