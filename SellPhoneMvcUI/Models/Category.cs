using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SellPhoneMvcUI.Models
{
    [Table("Categorys")]
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }

        public string? Description { get; set; }

        // Navigation property
        public ICollection<Product> Products { get; set; }
    }
}
