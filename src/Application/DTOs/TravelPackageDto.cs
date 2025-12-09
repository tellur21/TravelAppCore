using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class TravelPackageDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Package Name")]
        public string Name { get; set; } = string.Empty;

        public string Destination { get; set; } = string.Empty;

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Price { get; set; }

        public string Description { get; set; } = string.Empty;

        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Max Capacity")]
        public int MaxCapacity { get; set; }

        [Display(Name = "Available Slots")]
        public int AvailableSlots { get; set; }

        public string Status { get; set; } = string.Empty;

        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; } = string.Empty;

        [Display(Name = "Duration")]
        public int Duration => (EndDate - StartDate).Days + 1;
    }
}
