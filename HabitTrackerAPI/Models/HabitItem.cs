using System.ComponentModel.DataAnnotations;
using HabitTrackerAPI.Models.Enums;

namespace HabitTrackerAPI.Models
{
    public class HabitItem: IActivityItem
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public Priority Priority { get; set; }  // "Low", "Medium", "High"
        public Frequency Frequency { get; set; }  // "Daily", "Weekly", "Monthly", "NoFrequency"

        public uint PositiveCounter { get; set; } = 0;
        public uint NegativeCounter { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
