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
                PositiveCounter = habit.PositiveCounter,
                NegativeCounter = habit.NegativeCounter,
                CreatedAt = habit.CreatedAt,
                UpdatedAt = habit.UpdatedAt
            };
        }

        public static HabitItem ToHabitItem(this CreateHabitRequest dto)
        {
            return new HabitItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                Frequency = dto.Frequency,
                PositiveCounter = 0,
                NegativeCounter = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
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
            habit.UpdatedAt = DateTime.UtcNow;
        }
    }
}
