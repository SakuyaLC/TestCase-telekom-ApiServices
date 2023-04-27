using System.ComponentModel.DataAnnotations;
using UsersAndOrdersService.Model;

namespace UsersAndOrdersService.Data.DTO
{
    public class OrderedItemDTO
    {
        [Required]
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
}
