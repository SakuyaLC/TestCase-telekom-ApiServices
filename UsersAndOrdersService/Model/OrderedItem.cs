using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsersAndOrdersService.Model
{
    [Table("OrderedItems")]
    public class OrderedItem
    {
        [Required]
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public Order Order { get; set; }
        public int Quantity { get; set; }
    }
}
