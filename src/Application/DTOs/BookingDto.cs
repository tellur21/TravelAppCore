using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class BookingDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Travel Package")]
        public Guid TravelPackageId { get; set; }

        [Display(Name = "Package Name")]
        public string TravelPackageName { get; set; } = string.Empty;

        public string Destination { get; set; } = string.Empty;

        [Display(Name = "Booking Date")]
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime BookingDate { get; set; }

        [Display(Name = "Number of Travelers")]
        public int NumberOfTravelers { get; set; }

        [Display(Name = "Total Amount")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = string.Empty;

        [Display(Name = "Special Requests")]
        public string SpecialRequests { get; set; } = string.Empty;

        public List<BookingTravelerDto> Travelers { get; set; } = new();
    }
}
