using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class BookingTraveler
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    [Column(TypeName = "nvarchar(100)")]
    public string FirstName { get; set; } = string.Empty;
    [Column(TypeName = "nvarchar(100)")]
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    [Column(TypeName = "nvarchar(50)")]
    public string PassportNumber { get; set; } = string.Empty;
    public DateTime PassportExpiry { get; set; }
    
    // Navigation properties
    public Booking Booking { get; set; } = null!;
}