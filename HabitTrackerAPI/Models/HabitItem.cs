using System.ComponentModel.DataAnnotations;
using HabitTrackerAPI.Models.Enums;
using HabitTrackerAPI.Validation;

namespace HabitTrackerAPI.Models
{
    [HabitCounterValidation]
    public class HabitItem: IActivityItem
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public Priority Priority { get; set; }  // "Low", "Medium", "High"

        [Required]
        public Frequency Frequency { get; set; }  // "Daily", "Weekly", "Monthly", "NoFrequency"

        [Required]
        public HabitTrackingMode TrackingMode { get; set; }

        public uint? PositiveCounter { get; set; }
        public uint? NegativeCounter { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public string UserId { get; set; } = default!;

        public ApplicationUser? User { get; set; }
    }
}
