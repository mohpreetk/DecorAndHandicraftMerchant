using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DecorAndHandicraftMerchant.Models
{
    public class Address
    {
        
        [Range(0, double.MaxValue)]
        public int AddressId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string City { get; set; }

        [Required]
        [StringLength(5, MinimumLength = 2)]
        public string Province { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Country { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 3)]
        public string PostalCode { get; set; }

        public List<Order> Orders { get; set; }
    }
}
