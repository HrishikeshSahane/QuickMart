using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickMartCoreMVCApp.Models
{
    public class ShowOrders
    {
        public long OrderID { get; set; }
        public string EmailId { get; set; }
        public string FinalItems { get; set; }
        public string TotalCost { get; set; }
    }
}
