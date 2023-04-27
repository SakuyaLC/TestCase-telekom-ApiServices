using System.ComponentModel.DataAnnotations;
using UsersAndOrdersService.Model;

namespace UsersAndOrdersService.Data.DTO
{
    public class OrderDTO
    {
        [Required]
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime DateTime { get; set; }
    }
}
