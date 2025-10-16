using System.ComponentModel.DataAnnotations;

namespace HabitTrackerAPI.DTOs.Auth
{
    public class RefreshRequest
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
