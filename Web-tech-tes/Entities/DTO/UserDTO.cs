using Entities.Models;

namespace Entities.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}