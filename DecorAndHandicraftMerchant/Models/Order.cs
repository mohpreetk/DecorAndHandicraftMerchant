using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DecorAndHandicraftMerchant.Models
{
    public class Order
    {
        [Display(Name = "Order")]
        public int OrderId { get; set; }

        public Profile Profile { get; set; }

        [Display(Name = "Profile")]
        public int ProfileId { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price can not be negative or zero")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal Total { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

    }
}
