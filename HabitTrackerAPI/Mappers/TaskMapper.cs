using Task_Management_API.DTOs;
using Task_Management_API.Models;


namespace Task_Management_API.Mappers
{
    public static class TaskMapper
    {
        public static TaskResponse ToTaskResponse(this TaskItem task)
        {
            return new TaskResponse
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Priority = task.Priority,
                DueDate = task.DueDate,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            };
        }

        public static TaskItem ToTaskItem(this CreateTaskRequest dto)
        {
            return new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                DueDate = dto.DueDate,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        public static void UpdateTaskItem(this TaskItem task, UpdateTaskRequest dto)
        {
            task.Title = dto.Title;
            task.Description = dto.Description;
            task.Priority = dto.Priority;
            task.DueDate = dto.DueDate;
            task.IsCompleted = dto.IsCompleted;
            task.UpdatedAt = DateTime.UtcNow;
        }
    }
}
