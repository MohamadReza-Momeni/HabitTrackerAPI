using System.ComponentModel.DataAnnotations;

namespace HabitTrackerAPI.Models
{
    public class DailyChecklist
    {
        public int Id { get; set; }

        [Required, MaxLength(300)]
        public string Description { get; set; }

        public bool IsCompleted { get; set; } = false;

        public int DailyItemId { get; set; }

        public DailyItem DailyItem { get; set; }
    }
}
