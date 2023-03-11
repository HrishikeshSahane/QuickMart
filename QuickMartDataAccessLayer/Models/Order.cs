using System;
using System.Collections.Generic;

#nullable disable

namespace QuickMartDataAccessLayer.Models
{
    public partial class Order
    {
        public long OrderId { get; set; }
        public string EmailId { get; set; }
        public string FinalItems { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsPaymentReceived { get; set; }
        public int TotalCost { get; set; }

        public virtual Users Email { get; set; }
    }
}
