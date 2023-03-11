using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickMartDataAccessLayer.Models
{
    public class MostPurchases
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int TotalQuantity { get; set; }
    }
}
