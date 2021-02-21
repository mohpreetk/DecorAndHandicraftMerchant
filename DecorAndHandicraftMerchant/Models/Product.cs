using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DecorAndHandicraftMerchant.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string Name { get; set; }

        //public string Photo {get; set;}

        public string Description { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public SubCategory SubCategory { get; set; }

        public int SubCategoryId { get; set; }

        public List<Cart> Carts { get; set; }

        public List<OrderDetail> OrderDetails { get; set; } 

    }
}
