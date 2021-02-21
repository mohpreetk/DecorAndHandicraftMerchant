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
        public int CartId { get; set; }

        public Product Product { get; set; }

        public int ProductId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18,3)")]
        public decimal UnitPrice { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; } = 0;

        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18,3)")]
        public decimal DeliveryCost { get; set; }

    }
}
