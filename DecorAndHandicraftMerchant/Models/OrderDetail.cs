using System;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DecorAndHandicraftMerchant.Models
{
    public class OrderDetail
    {
        [Display(Name = "Order Detail")]
        public int OrderDetailId { get; set; }

        public Product Product { get; set; }

        [Display(Name = "Product")]
        public int ProductId { get; set; }

        public Order Order { get; set; }

        [Display(Name = "Order")]
        public int OrderId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price can not be negative or zero")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal UnitPrice { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quanity can not be negative")]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price can not be negative or zero")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public Decimal Total { get; set; }
    }
}
