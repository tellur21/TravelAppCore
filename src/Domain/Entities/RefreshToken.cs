using System;

namespace Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public required string Token { get; set; }
        public required string JwtId { get; set; } // The JTI (JWT ID) of the access token this refresh token is for
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public required string UserId { get; set; } // Foreign key to the user
    }
}