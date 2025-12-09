using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class BookingTravelerDto
    {
        public Guid Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Passport Number")]
        public string PassportNumber { get; set; } = string.Empty;

        [Display(Name = "Passport Expiry")]
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime PassportExpiry { get; set; }
    }
}
