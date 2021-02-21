using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DecorAndHandicraftMerchant.Models
{
    public class SubCategory
    {
        public int SubCategoryId { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string Name { get; set; }

        public Category Category { get; set; }

        public int CategoryId { get; set; }

        public List<Product> Products { get; set; }
    }
}
