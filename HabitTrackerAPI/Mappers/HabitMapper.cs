using HabitTrackerAPI.DTOs;
using HabitTrackerAPI.Models;

namespace HabitTrackerAPI.Mappers
{
    public static class HabitMapper
    {
        public static HabitResponse ToHabitResponse(this HabitItem habit)
        {
            return new HabitResponse
            {
                Id = habit.Id,
                Title = habit.Title,
                Description = habit.Description,
                Priority = habit.Priority,
                Frequency = habit.Frequency,
                TrackingMode = habit.TrackingMode,
                PositiveCounter = habit.PositiveCounter,
                NegativeCounter = habit.NegativeCounter,
                CreatedAt = habit.CreatedAt,
                UpdatedAt = habit.UpdatedAt
            };
        }

        public static HabitItem ToHabitItem(this CreateHabitRequest dto, string userId)
        {
            return new HabitItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                Frequency = dto.Frequency,
                TrackingMode = dto.TrackingMode,
                PositiveCounter = dto.PositiveCounter,
                NegativeCounter = dto.NegativeCounter,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                UserId = userId
            };
        }

        public static void UpdateHabitItem(this HabitItem habit, UpdateHabitRequest dto)
        {
            habit.Title = dto.Title;
            habit.Description = dto.Description;
            habit.Priority = dto.Priority;
            habit.Frequency = dto.Frequency;
            habit.PositiveCounter = dto.PositiveCounter;
            habit.NegativeCounter = dto.NegativeCounter;
            habit.TrackingMode = dto.TrackingMode;
            habit.UpdatedAt = DateTime.UtcNow;
        }
    }
}
