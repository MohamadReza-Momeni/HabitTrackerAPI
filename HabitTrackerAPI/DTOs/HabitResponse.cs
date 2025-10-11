using HabitTrackerAPI.Models.Enums;

namespace HabitTrackerAPI.DTOs
{
    public class HabitResponse
    {
        /// <summary>The unique identifier of the habit.</summary>
        public int Id { get; set; }

        /// <summary>The short title of the habit.</summary>
        public string Title { get; set; }

        /// <summary>A detailed description of the habit.</summary>
        public string? Description { get; set; }

        /// <summary>The priority level of the habit (Low, Medium, High).</summary>
        public Priority Priority { get; set; }

        /// <summary>The frequency at which the habit should occur.</summary>
        public Frequency Frequency { get; set; }

        /// <summary>Indicates whether the habit tracks positive results, negative results, or both.</summary>
        public HabitTrackingMode TrackingMode { get; set; }

        /// <summary>How many times the habit was successfully completed.</summary>
        public uint? PositiveCounter { get; set; }

        /// <summary>How many times the habit was missed.</summary>
        public uint? NegativeCounter { get; set; }

        /// <summary>When the habit was created (UTC).</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>When the habit was last updated (UTC).</summary>
        public DateTime UpdatedAt { get; set; }
    }
}
