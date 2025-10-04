using System.ComponentModel.DataAnnotations;
using Task_Management_API.Models.Enums;

namespace Task_Management_API.DTOs
{
    public class UpdateTaskRequest
    {
        [Required, MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public Priority Priority { get; set; }

        public DateTime? DueDate { get; set; }

        public bool IsCompleted { get; set; }
    }
}
