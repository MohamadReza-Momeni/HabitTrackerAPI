using Task_Management_API.Models.Enums;

namespace Task_Management_API.DTOs
{
    public class TaskResponse
    {
        /// <summary>The unique identifier of the task.</summary>
        public int Id { get; set; }

        /// <summary>The short title of the task.</summary>
        public string Title { get; set; }

        /// <summary>A detailed description of the task.</summary>
        public string? Description { get; set; }

        /// <summary>The priority level of the task (Low, Medium, High).</summary>
        public Priority Priority { get; set; }

        /// <summary>The deadline for the task, if any.</summary>
        public DateTime? DueDate { get; set; }

        /// <summary>Indicates whether the task is completed.</summary>
        public bool IsCompleted { get; set; }

        /// <summary>When the task was created (UTC).</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>When the task was last updated (UTC).</summary>
        public DateTime UpdatedAt { get; set; }
    }
}
