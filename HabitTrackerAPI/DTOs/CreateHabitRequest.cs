using System.ComponentModel.DataAnnotations;
using HabitTrackerAPI.Models.Enums;
using HabitTrackerAPI.Validation;

namespace HabitTrackerAPI.DTOs
{
    [HabitCounterValidation]
    public class CreateHabitRequest
    {
        /// <summary>
        /// Short title of the habit (required, max 200 chars).
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
        /// Frequency for performing the habit (e.g. Daily, Weekly).
        /// </summary>
        [Required]
        public Frequency Frequency { get; set; }

        /// <summary>
        /// Determines whether the habit tracks positive results, negative results, or both.
        /// </summary>
        [Required]
        public HabitTrackingMode TrackingMode { get; set; }

        /// <summary>
        /// Initial value for the positive counter when the habit tracks positive outcomes.
        /// </summary>
        public uint? PositiveCounter { get; set; }

        /// <summary>
        /// Initial value for the negative counter when the habit tracks negative outcomes.
        /// </summary>
        public uint? NegativeCounter { get; set; }
    }
}
