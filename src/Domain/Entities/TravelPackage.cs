using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities;

public class TravelPackage
{
    public Guid Id { get; set; }
    [Column(TypeName = "nvarchar(100)")]
    public string Name { get; set; } = string.Empty;
    [Column(TypeName = "nvarchar(500)")]
    public string Destination { get; set; } = string.Empty;
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }
    [Column(TypeName = "nvarchar(2000)")]
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int MaxCapacity { get; set; }
    public int AvailableSlots { get; set; }
    public PackageStatus Status { get; set; }
    [Column(TypeName = "nvarchar(500)")]
    public string ImageUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}

