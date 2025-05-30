using System.ComponentModel.DataAnnotations;

namespace SellPhoneMvcUI.Models.DTOs
{
    public class CategoryViewModel
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }

        public string? Description { get; set; }
    }
}
