using System.ComponentModel.DataAnnotations;

namespace ItemsService.Model
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
