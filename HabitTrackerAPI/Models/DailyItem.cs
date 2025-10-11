using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HabitTrackerAPI.Models.Enums;

namespace HabitTrackerAPI.Models
{
    public class DailyItem: IActivityItem
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public Priority Priority { get; set; }  // "Low", "Medium", "High"

        [Required]
        public RepeatDuration RepeatDuration { get; set; }  // "Daily", "Weekly", "Monthly", "Yearly"

        [Required]
        public DateTime StartDate { get; set; } 

        public ICollection<DailyChecklist> Checklists { get; set; } = new List<DailyChecklist>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
