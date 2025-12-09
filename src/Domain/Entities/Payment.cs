using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities;

public class Payment
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; }
    public PaymentMethod Method { get; set; }
    [Column(TypeName = "nvarchar(100)")]
    public string TransactionId { get; set; } = string.Empty;
    public DateTime ProcessedAt { get; set; }
    [Column(TypeName = "nvarchar(500)")]
    public string FailureReason { get; set; } = string.Empty;
    
    // Navigation properties
    public Booking Booking { get; set; } = null!;
}