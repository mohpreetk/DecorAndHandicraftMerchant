using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DecorAndHandicraftMerchant.Models
{
    public class Cart
    {
        [Display(Name = "Cart")]
        public int CartId { get; set; }

        public Product Product { get; set; }

        [Display(Name = "Product")]
        public int ProductId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price can not be negative or zero")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Unit Price")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal UnitPrice { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity can not be negative")]
        public int Quantity { get; set; } = 0;

        [Range(0, double.MaxValue, ErrorMessage = "Delivery cost can not be negative")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Delivery Cost")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal DeliveryCost { get; set; }

    }
}
