using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuickMartCoreMVCApp.Models
{
    public class PurchaseDetails
    {
        
        public long PurchaseId { get; set; }
      
        public string EmailId { get; set; }

        public string ProductId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public short QuantityPurchased { get; set; }
        
        public DateTime DateOfPurchase { get; set; }
    }
}
