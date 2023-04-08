using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace QuickMartDataAccessLayer.Models
{
    public partial class Users
    {
        public Users()
        {
            PurchaseDetails = new HashSet<PurchaseDetails>();
        }

        [Display(Name = "EmailId")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
        ErrorMessage = "Invalid email format")]
        [Required]
        public string EmailId { get; set; }
        [Required]
        public string UserPassword { get; set; }
        public byte? RoleId { get; set; }
        [Required]
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        [RegularExpression(@"^[#.0-9a-zA-Z\s,-:]+$",ErrorMessage ="Enter valid address")]
        [Required]
        public string Address { get; set; }
        public virtual Roles Role { get; set; }
        public virtual ICollection<PurchaseDetails> PurchaseDetails { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; } = new List<Feedback>();
        public virtual ICollection<Order> Orders { get; set; }
    }
}
