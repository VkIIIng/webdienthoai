using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SellPhoneMvcUI.Models
{
    [Table("Users")]
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(255)]
        public string Address { get; set; }


        // Navigation properties
        public ICollection<Order> Orders { get; set; }

        public Cart Cart { get; set; }
    }
}
