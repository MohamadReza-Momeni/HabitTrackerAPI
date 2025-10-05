using HabitTrackerAPI.Models.Enums;

namespace HabitTrackerAPI.Models
{
    public class IActivityItem
    {
        int Id { get; set; }
        string Title { get; set; }
        string? Description { get; set; }
        Priority Priority { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}
