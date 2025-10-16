using System;
using System.Collections.Generic;
using System.Linq;
using HabitTrackerAPI.DTOs;
using HabitTrackerAPI.Models;

namespace HabitTrackerAPI.Mappers
{
    public static class DailyMapper
    {
        public static DailyResponse ToDailyResponse(this DailyItem daily)
        {
            return new DailyResponse
            {
                Id = daily.Id,
                Title = daily.Title,
                Description = daily.Description,
                Priority = daily.Priority,
                RepeatDuration = daily.RepeatDuration,
                StartDate = daily.StartDate,
                CreatedAt = daily.CreatedAt,
                UpdatedAt = daily.UpdatedAt,
                Checklists = (daily.Checklists ?? new List<DailyChecklist>())
                    .Select(checklist => new DailyChecklistResponseItem
                    {
                        Id = checklist.Id,
                        Description = checklist.Description,
                        IsCompleted = checklist.IsCompleted
                    })
                    .ToList()
            };
        }

        public static DailyItem ToDailyItem(this CreateDailyRequest dto, string userId)
        {
            return new DailyItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                RepeatDuration = dto.RepeatDuration,
                StartDate = dto.StartDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                UserId = userId,
                Checklists = (dto.Checklists ?? new List<DailyChecklistItem>())
                    .Select(item => new DailyChecklist
                    {
                        Description = item.Description,
                        IsCompleted = item.IsCompleted
                    })
                    .ToList()
            };
        }

        public static void UpdateDailyItem(this DailyItem daily, UpdateDailyRequest dto)
        {
            daily.Title = dto.Title;
            daily.Description = dto.Description;
            daily.Priority = dto.Priority;
            daily.RepeatDuration = dto.RepeatDuration;
            daily.StartDate = dto.StartDate;
            daily.UpdatedAt = DateTime.UtcNow;

            daily.Checklists ??= new List<DailyChecklist>();

            var existingById = daily.Checklists
                .Where(c => c.Id > 0)
                .ToDictionary(c => c.Id, c => c);

            var incomingIds = new HashSet<int>();

            foreach (var checklistDto in dto.Checklists ?? new List<DailyChecklistUpdateItem>())
            {
                if (checklistDto.Id.HasValue && existingById.TryGetValue(checklistDto.Id.Value, out var existing))
                {
                    existing.Description = checklistDto.Description;
                    existing.IsCompleted = checklistDto.IsCompleted;
                    incomingIds.Add(existing.Id);
                }
                else
                {
                    daily.Checklists.Add(new DailyChecklist
                    {
                        Description = checklistDto.Description,
                        IsCompleted = checklistDto.IsCompleted,
                        DailyItem = daily
                    });
                }
            }

            var toRemove = daily.Checklists
                .Where(c => c.Id > 0 && !incomingIds.Contains(c.Id))
                .ToList();

            foreach (var checklist in toRemove)
            {
                daily.Checklists.Remove(checklist);
            }
        }
    }
}
