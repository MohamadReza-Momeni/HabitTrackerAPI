using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HabitTrackerAPI.Models.Enums;

namespace HabitTrackerAPI.DTOs
{
    public class UpdateDailyRequest
    {
        [Required, MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public Priority Priority { get; set; }

        [Required]
        public RepeatDuration RepeatDuration { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public List<DailyChecklistUpdateItem> Checklists { get; set; } = new();
    }

    public class DailyChecklistUpdateItem
    {
        public int? Id { get; set; }

        [Required, MaxLength(300)]
        public string Description { get; set; }

        public bool IsCompleted { get; set; }
    }
}
