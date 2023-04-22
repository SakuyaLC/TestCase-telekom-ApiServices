using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UsersAndOrdersService.Model;

namespace UsersAndOrdersService.Model
{
    [Table("Orders")]
    public class Order
    {
        [Required]
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime DateTime { get; set; }
        public ICollection<OrderedItem> OrderedItems { get; set; }
    }
}
