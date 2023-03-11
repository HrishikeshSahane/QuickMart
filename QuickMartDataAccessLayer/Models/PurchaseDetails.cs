using System;
using System.Collections.Generic;

#nullable disable

namespace QuickMartDataAccessLayer.Models
{
    public partial class PurchaseDetails
    {
        public long PurchaseId { get; set; }
        public string EmailId { get; set; }
        public string ProductId { get; set; }
        public short QuantityPurchased { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public virtual Users Email { get; set; }
        public virtual Products ProductNavigation { get; set; }
    }
}
