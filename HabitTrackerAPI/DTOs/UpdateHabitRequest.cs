using System.ComponentModel.DataAnnotations;
using HabitTrackerAPI.Models.Enums;
using HabitTrackerAPI.Validation;

namespace HabitTrackerAPI.DTOs
{
    [HabitCounterValidation]
    public class UpdateHabitRequest
    {
        [Required, MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public Priority Priority { get; set; }

        [Required]
        public Frequency Frequency { get; set; }

        [Required]
        public HabitTrackingMode TrackingMode { get; set; }

        public uint? PositiveCounter { get; set; }

        public uint? NegativeCounter { get; set; }
    }
}
