using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsersAndOrdersService.Model
{
    [Table("Users")]
    public class User
    {
        [Required]
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public ICollection<Order> Orders { get; set; }
        
    }
}
