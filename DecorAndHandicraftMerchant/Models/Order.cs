using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DecorAndHandicraftMerchant.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public Profile Profile { get; set; }

        public int ProfileId { get; set; }

        public Address Address { get; set; }

        public int AddressId { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Total { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

    }
}
