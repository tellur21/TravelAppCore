using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class UserProfile
{
    public string UserId { get; set; } = string.Empty;
    [Column(TypeName = "nvarchar(100)")]
    public string FirstName { get; set; } = string.Empty;
    [Column(TypeName = "nvarchar(100)")]
    public string LastName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    [Column(TypeName = "nvarchar(20)")]
    public string PhoneNumber { get; set; } = string.Empty;
    [Column(TypeName = "nvarchar(200)")]
    public string Address { get; set; } = string.Empty;
    [Column(TypeName = "nvarchar(100)")]
    public string City { get; set; } = string.Empty;
    [Column(TypeName = "nvarchar(100)")]
    public string Country { get; set; } = string.Empty;
    [Column(TypeName = "nvarchar(50)")]
    public string PassportNumber { get; set; } = string.Empty;
    public DateTime? PassportExpiry { get; set; }
    [Column(TypeName = "nvarchar(10)")]
    public string PreferredLanguage { get; set; } = "en";
    [Column(TypeName = "nvarchar(500)")]
    public string AvatarUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}