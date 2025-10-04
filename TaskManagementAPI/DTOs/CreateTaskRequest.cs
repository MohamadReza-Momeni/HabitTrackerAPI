using System.ComponentModel.DataAnnotations;
using Task_Management_API.Models.Enums;


namespace Task_Management_API.DTOs
{
    public class CreateTaskRequest
    {
        /// <summary>
        /// Short title of the task (required, max 200 chars).
        /// </summary>
        [Required, MaxLength(200)]
        public string Title { get; set; }

        /// <summary>
        /// Detailed description (optional, max 1000 chars).
        /// </summary>
        [MaxLength(1000)]
        public string? Description { get; set; }

        /// <summary>
        /// Task priority: Low, Medium, High.
        /// </summary>
        [Required]
        public Priority Priority { get; set; }

        /// <summary>
        /// Optional deadline (must be in the future).
        /// </summary>
        public DateTime? DueDate { get; set; }
    }
}
