using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities;

public class Booking
{
    public Guid Id { get; set; }
    public Guid TravelPackageId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime BookingDate { get; set; }
    public int NumberOfTravelers { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalAmount { get; set; }
    public BookingStatus Status { get; set; }
    [Column(TypeName = "nvarchar(1000)")]
    public string SpecialRequests { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    
    // Navigation properties
    public TravelPackage TravelPackage { get; set; } = null!;
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public ICollection<BookingTraveler> Travelers { get; set; } = new List<BookingTraveler>();
}
