using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DecorAndHandicraftMerchant.Models
{
    public class Profile
    {
        [Display(Name = "Profile")]
        public int ProfileId { get; set; }

        public string Username { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Phone Number")]
        public int PhoneNumber { get; set; }

        public List<Order> Orders { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Address Line should be between 5-100 Characters")]
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "City should be between 3-30 Characters")]
        public string City { get; set; }

        [Required]
        [StringLength(5, MinimumLength = 2, ErrorMessage = "Province should be between 2-5 Characters")]
        public string Province { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Country should be between 2-30 Characters")]
        public string Country { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Postal Code should be between 3-10 Characters")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

    }
}
