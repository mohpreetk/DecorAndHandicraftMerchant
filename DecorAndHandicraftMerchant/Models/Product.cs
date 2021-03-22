using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DecorAndHandicraftMerchant.Models
{
    public class Product
    {
        [Display(Name = "Product")]
        public int ProductId { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Address Line should be between 4-30 Characters")]
        public string Name { get; set; }

        public string Photo { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price can not be negative or zero")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal Price { get; set; }

        public SubCategory SubCategory { get; set; }

        [Display(Name = "Sub Category")]
        public int SubCategoryId { get; set; }

        public List<Cart> Carts { get; set; }

        public List<OrderDetail> OrderDetails { get; set; } 

    }
}
