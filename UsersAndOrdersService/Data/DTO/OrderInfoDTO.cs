using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using UsersAndOrdersService.Model;

namespace UsersAndOrdersService.Data.DTO
{
    public class OrderInfoDTO
    {
        [Required]
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime DateTime { get; set; }
        [JsonPropertyName("OrderedItems")]
        public ICollection<OrderedItemDTO> OrderedItems { get; set; }
    }
}
