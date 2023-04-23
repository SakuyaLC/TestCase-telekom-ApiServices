using System.ComponentModel.DataAnnotations;

namespace UsersAndOrdersService.Data.DTO
{
    public class UserCreationDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
