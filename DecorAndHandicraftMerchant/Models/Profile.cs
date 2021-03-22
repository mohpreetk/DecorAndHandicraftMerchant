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

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Phone Number")]
        public int PhoneNumber { get; set; }

        public string Email { get; set; }

        public List<Order> Orders { get; set; }
    }
}
