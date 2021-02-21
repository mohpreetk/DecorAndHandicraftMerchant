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
        public int OrderDetailId { get; set; }

        public Product Product { get; set; }

        public int ProductId { get; set; }

        public Order Order { get; set; }

        public int OrderId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18,3)")]
        public decimal UnitPrice { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; } = 0;

        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18,3)")]
        public Decimal Total { get; set; }
    }
}
