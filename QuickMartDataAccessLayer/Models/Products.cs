using System;
using System.Collections.Generic;

#nullable disable

namespace QuickMartDataAccessLayer.Models
{
    public partial class Products
    {
        public Products()
        {
            PurchaseDetails = new HashSet<PurchaseDetails>();
        }

        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public byte? CategoryId { get; set; }
        public decimal Price { get; set; }
        public int QuantityAvailable { get; set; }

        public virtual Categories Categories { get; set; }
        public virtual ICollection<PurchaseDetails> PurchaseDetails { get; set; }
    }
}
