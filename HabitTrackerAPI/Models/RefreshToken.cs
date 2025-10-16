using System.ComponentModel.DataAnnotations;

namespace HabitTrackerAPI.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }

        [Required]
        public string TokenHash { get; set; } = string.Empty;

        [Required]
        public string TokenId { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;

        public ApplicationUser? User { get; set; }

        [Required]
        public DateTime ExpiresAtUtc { get; set; }

        [Required]
        public DateTime CreatedAtUtc { get; set; }

        public DateTime? RevokedAtUtc { get; set; }

        public bool IsRevoked => RevokedAtUtc.HasValue || DateTime.UtcNow >= ExpiresAtUtc;
    }
}
