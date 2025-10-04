using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task_Management_API.Data;
using Task_Management_API.DTOs;
using Task_Management_API.Models.Enums;
using Task_Management_API.Mappers;

namespace Task_Management_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Tags("Tasks")]
    public class TasksController : ControllerBase
    {
        private readonly TaskDbContext _context;

        public TasksController(TaskDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all tasks with optional filtering, sorting, and pagination.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/tasks?isCompleted=false&amp;priority=High&amp;page=1&amp;pageSize=5
        /// Sample response:
        ///
        ///     {
        ///       "page": 1,
        ///       "pageSize": 5,
        ///       "totalCount": 12,
        ///       "tasks": [
        ///         {
        ///           "id": 1,
        ///           "title": "Finish .NET API",
        ///           "description": "Complete DTOs and examples",
        ///           "priority": "High",
        ///           "dueDate": "2025-12-01T00:00:00Z",
        ///           "isCompleted": false,
        ///           "createdAt": "2025-10-02T12:34:56Z",
        ///           "updatedAt": "2025-10-02T12:34:56Z"
        ///         }
        ///       ]
        ///     }
        /// </remarks>
        /// <param name="isCompleted">Filter by completion status (true/false).</param>
        /// <param name="priority">Filter by priority (Low, Medium, High).</param>
        /// <param name="dueBefore">Filter tasks due before this date.</param>
        /// <param name="page">Page number for pagination (default: 1).</param>
        /// <param name="pageSize">Number of tasks per page (default: 10).</param>
        /// <param name="sortBy">Sort field: CreatedAt, DueDate, Priority (default: CreatedAt).</param>
        /// <param name="order">Sort order: asc or desc (default: desc).</param>
        /// <returns>A paginated list of tasks.</returns>
        /// <response code="200">Returns the list of tasks.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TaskResponse>), 200)]
        public async Task<ActionResult<object>> GetTasks(
            bool? isCompleted,
            Priority? priority,
            DateTime? dueBefore,
            int page = 1,
            int pageSize = 10,
            string sortBy = "CreatedAt",
            string order = "desc")
        {
            var query = _context.Tasks.AsQueryable();

            // Filtering
            if (isCompleted.HasValue)
                query = query.Where(t => t.IsCompleted == isCompleted.Value);

            if (priority.HasValue)
                query = query.Where(t => t.Priority == priority.Value);

            if (dueBefore.HasValue)
                query = query.Where(t => t.DueDate <= dueBefore.Value);

            // Sorting
            query = sortBy.ToLower() switch
            {
                "duedate" => (order == "asc") ? query.OrderBy(t => t.DueDate) : query.OrderByDescending(t => t.DueDate),
                "priority" => (order == "asc") ? query.OrderBy(t => t.Priority) : query.OrderByDescending(t => t.Priority),
                _ => (order == "asc") ? query.OrderBy(t => t.CreatedAt) : query.OrderByDescending(t => t.CreatedAt)
            };

            // Pagination
            var totalCount = await query.CountAsync();
            var tasks = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                page,
                pageSize,
                totalCount,
                tasks = tasks.Select(t => t.ToTaskResponse())
            });
        }

        /// <summary>
        /// Get a single task by ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/tasks/1
        /// Sample response:
        ///
        ///     {
        ///       "id": 1,
        ///       "title": "Finish .NET API",
        ///       "description": "Complete DTOs and examples",
        ///       "priority": "High",
        ///       "dueDate": "2025-12-01T00:00:00Z",
        ///       "isCompleted": false,
        ///       "createdAt": "2025-10-02T12:34:56Z",
        ///       "updatedAt": "2025-10-02T12:34:56Z"
        ///     }
        /// </remarks>
        /// <param name="id">The ID of the task to retrieve.</param>
        /// <returns>The task details.</returns>
        /// <response code="200">Task found and returned.</response>
        /// <response code="404">Task not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TaskResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TaskResponse>> GetTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
                return NotFound();

            return Ok(task.ToTaskResponse());
        }

        /// <summary>
        /// Create a new task.
        /// </summary>
        /// <param name="request">The details of the task to create.</param>
        /// <returns>The created task.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/tasks
        ///     {
        ///       "title": "Learn DTOs",
        ///       "description": "Implement CreateTaskRequest and TaskResponse",
        ///       "priority": "Medium",
        ///       "dueDate": "2025-12-05T00:00:00Z"
        ///     }
        /// Sample response:
        ///
        ///     {
        ///       "id": 2,
        ///       "title": "Learn DTOs",
        ///       "description": "Implement CreateTaskRequest and TaskResponse",
        ///       "priority": "Medium",
        ///       "dueDate": "2025-12-05T00:00:00Z",
        ///       "isCompleted": false,
        ///       "createdAt": "2025-10-02T12:45:00Z",
        ///       "updatedAt": "2025-10-02T12:45:00Z"
        ///     }
        /// </remarks>
        /// <response code="201">Task created successfully.</response>
        /// <response code="400">Validation failed (e.g., due date in the past).</response>
        [HttpPost]
        [ProducesResponseType(typeof(TaskResponse), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<TaskResponse>> CreateTask(CreateTaskRequest request)
        {
            if (request.DueDate.HasValue && request.DueDate.Value < DateTime.UtcNow)
                return BadRequest(new { error = "Due date cannot be in the past." });

            var task = request.ToTaskItem();

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task.ToTaskResponse());
        }

        /// <summary>
        /// Update an existing task.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/tasks/1
        ///     {
        ///       "title": "Update DTO lesson",
        ///       "description": "Refactor and improve documentation",
        ///       "priority": "High",
        ///       "dueDate": "2025-12-10T00:00:00Z",
        ///       "isCompleted": true
        ///     }
        /// Sample response:
        ///
        ///     {
        ///       "id": 1,
        ///       "title": "Update DTO lesson",
        ///       "description": "Refactor and improve documentation",
        ///       "priority": "High",
        ///       "dueDate": "2025-12-10T00:00:00Z",
        ///       "isCompleted": true,
        ///       "createdAt": "2025-10-02T12:34:56Z",
        ///       "updatedAt": "2025-10-02T13:10:00Z"
        ///     }
        /// </remarks>
        /// <param name="id">The ID of the task to update.</param>
        /// <param name="request">The updated task details.</param>
        /// <returns>The updated task.</returns>
        /// <response code="200">Task updated successfully.</response>
        /// <response code="400">Invalid request (e.g., mismatched IDs).</response>
        /// <response code="404">Task not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TaskResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateTask(int id, UpdateTaskRequest request)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound();

            task.UpdateTaskItem(request);

            await _context.SaveChangesAsync();

            return Ok(task.ToTaskResponse());
        }

        /// <summary>
        /// Delete a task by ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/tasks/1
        /// Sample response:
        ///     (204 No Content)
        /// </remarks>
        /// <param name="id">The ID of the task to delete.</param>
        /// <response code="204">Task deleted successfully.</response>
        /// <response code="404">Task not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
