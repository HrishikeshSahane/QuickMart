using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuickMartCoreMVCApp.Models
{
    public class Products
    {
        public string ProductId { get; set; }

        [Required(ErrorMessage = "Product Name is required.")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "CategoryId field is required.")]
        public byte? CategoryId { get; set; }

        [Required(ErrorMessage = "Price field is required.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "QuantityAvailable field is required.")]
        public int QuantityAvailable { get; set; }

    }
}
