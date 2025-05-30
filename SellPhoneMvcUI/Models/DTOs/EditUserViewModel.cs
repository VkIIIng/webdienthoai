using System.ComponentModel.DataAnnotations;

namespace SellPhoneMvcUI.Models.DTOs
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(255)]
        public string Address { get; set; }
        public List<string> AllRoles { get; set; } = new List<string>(); // Danh sách tất cả vai trò
        public string SelectedRole { get; set; } // Vai trò được chọn
    }
}
