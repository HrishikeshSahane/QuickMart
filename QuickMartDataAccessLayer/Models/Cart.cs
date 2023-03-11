using System;
using System.Collections.Generic;

#nullable disable

namespace QuickMartDataAccessLayer.Models
{
    public partial class Cart
    {
        public long CartId { get; set; }
        public string EmailId { get; set; }
        public string Items { get; set; }
        public DateTime DateOfPurchase { get; set; }

        public virtual Users Email { get; set; }
    }
}
