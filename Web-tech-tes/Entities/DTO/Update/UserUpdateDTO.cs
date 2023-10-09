using Entities.DTO.Manipulation;
using System.ComponentModel.DataAnnotations;

namespace Entities.DTO.Update
{
    public class UserUpdateDTO : UserManipulationDTO
    {
        [Required(ErrorMessage = "User name is a required field.")]
        public string Name { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Age is required and it can't be negative")]
        public int Age { get; set; }
        [Required(ErrorMessage = "User email is a required field.")]
        public string Email { get; set; }
    }
}