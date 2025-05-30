using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SellPhoneMvcUI.Models
{
    [Table("OrderStatuses")]
    public class OrderStatus
    {
        [Key]
        public int StatusId { get; set; }

        [Required]
        [StringLength(50)]
        public string StatusName { get; set; }

        public string? Description { get; set; }

        // Navigation property
        public ICollection<Order> Orders { get; set; }
    }
}
