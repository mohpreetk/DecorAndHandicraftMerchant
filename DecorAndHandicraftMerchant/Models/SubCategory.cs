using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DecorAndHandicraftMerchant.Models
{
    public class SubCategory
    {
        [Display(Name = "Sub Category")]
        public int SubCategoryId { get; set; }

        public string Photo { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Name should be between 4-30 Characters")]
        public string Name { get; set; }

        public Category Category { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public List<Product> Products { get; set; }
    }
}
