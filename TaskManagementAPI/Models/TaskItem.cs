using System.ComponentModel.DataAnnotations;
using Task_Management_API.Models.Enums;

namespace Task_Management_API.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public Priority Priority { get; set; }  // "Low", "Medium", "High"

        public DateTime? DueDate { get; set; }

        public bool IsCompleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
