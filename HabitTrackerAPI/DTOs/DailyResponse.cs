using System;
using System.Collections.Generic;
using HabitTrackerAPI.Models.Enums;

namespace HabitTrackerAPI.DTOs
{
    public class DailyResponse
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public Priority Priority { get; set; }

        public RepeatDuration RepeatDuration { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public IReadOnlyList<DailyChecklistResponseItem> Checklists { get; init; } = Array.Empty<DailyChecklistResponseItem>();
    }

    public class DailyChecklistResponseItem
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }
    }
}
