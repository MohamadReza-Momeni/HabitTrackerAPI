using System.Security.Claims;
using HabitTrackerAPI.Data;
using HabitTrackerAPI.DTOs;
using HabitTrackerAPI.Mappers;
using HabitTrackerAPI.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HabitTrackerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Tags("Habits")]
    [Authorize]
    public class HabitsController : ControllerBase
    {
        private readonly TaskDbContext _context;

        public HabitsController(TaskDbContext context)
        {
            _context = context;
        }

        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("User identifier claim missing.");

        /// <summary>
        /// Retrieve all habits with optional filtering, sorting, and pagination.
        /// </summary>
        /// <param name="priority">Filter by priority (Low, Medium, High).</param>
        /// <param name="frequency">Filter by frequency (Daily, Weekly, Monthly, NoFrequency).</param>
        /// <param name="title">Filter by title containing this value.</param>
        /// <param name="page">Page number for pagination (default: 1).</param>
        /// <param name="pageSize">Number of habits per page (default: 10).</param>
        /// <param name="sortBy">Sort field: CreatedAt, Priority, Frequency, PositiveCounter, NegativeCounter (default: CreatedAt).</param>
        /// <param name="order">Sort order: asc or desc (default: desc).</param>
        /// <returns>A paginated list of habits.</returns>
        /// <response code="200">Returns the list of habits.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<HabitResponse>), 200)]
        public async Task<ActionResult<object>> GetHabits(
            Priority? priority,
            Frequency? frequency,
            string? title,
            int page = 1,
            int pageSize = 10,
            string sortBy = "CreatedAt",
            string order = "desc")
        {
            var userId = GetUserId();

            var query = _context.Habits
                .AsNoTracking()
                .Where(h => h.UserId == userId);

            if (priority.HasValue)
                query = query.Where(h => h.Priority == priority.Value);

            if (frequency.HasValue)
                query = query.Where(h => h.Frequency == frequency.Value);

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(h => EF.Functions.ILike(h.Title, $"%{title}%"));

            query = sortBy.ToLower() switch
            {
                "priority" => (order == "asc") ? query.OrderBy(h => h.Priority) : query.OrderByDescending(h => h.Priority),
                "frequency" => (order == "asc") ? query.OrderBy(h => h.Frequency) : query.OrderByDescending(h => h.Frequency),
                "positivecounter" => (order == "asc") ? query.OrderBy(h => h.PositiveCounter) : query.OrderByDescending(h => h.PositiveCounter),
                "negativecounter" => (order == "asc") ? query.OrderBy(h => h.NegativeCounter) : query.OrderByDescending(h => h.NegativeCounter),
                _ => (order == "asc") ? query.OrderBy(h => h.CreatedAt) : query.OrderByDescending(h => h.CreatedAt)
            };

            var totalCount = await query.CountAsync();
            var habits = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                page,
                pageSize,
                totalCount,
                habits = habits.Select(h => h.ToHabitResponse())
            });
        }

        /// <summary>
        /// Retrieve a single habit by ID.
        /// </summary>
        /// <param name="id">The ID of the habit to retrieve.</param>
        /// <returns>The habit details.</returns>
        /// <response code="200">Habit found and returned.</response>
        /// <response code="404">Habit not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(HabitResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<HabitResponse>> GetHabit(int id)
        {
            var userId = GetUserId();

            var habit = await _context.Habits
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == id && h.UserId == userId);

            if (habit == null)
                return NotFound();

            return Ok(habit.ToHabitResponse());
        }

        /// <summary>
        /// Create a new habit.
        /// </summary>
        /// <param name="request">The details of the habit to create.</param>
        /// <returns>The created habit.</returns>
        /// <response code="201">Habit created successfully.</response>
        /// <response code="400">Validation failed for the supplied request.</response>
        [HttpPost]
        [ProducesResponseType(typeof(HabitResponse), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<HabitResponse>> CreateHabit(CreateHabitRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var userId = GetUserId();
            var habit = request.ToHabitItem(userId);

            _context.Habits.Add(habit);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHabit), new { id = habit.Id }, habit.ToHabitResponse());
        }

        /// <summary>
        /// Update an existing habit.
        /// </summary>
        /// <param name="id">The ID of the habit to update.</param>
        /// <param name="request">The updated habit details.</param>
        /// <returns>The updated habit.</returns>
        /// <response code="200">Habit updated successfully.</response>
        /// <response code="400">Validation failed for the supplied request.</response>
        /// <response code="404">Habit not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(HabitResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateHabit(int id, UpdateHabitRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var userId = GetUserId();

            var habit = await _context.Habits
                .FirstOrDefaultAsync(h => h.Id == id && h.UserId == userId);

            if (habit == null)
                return NotFound();

            habit.UpdateHabitItem(request);

            await _context.SaveChangesAsync();

            return Ok(habit.ToHabitResponse());
        }

        /// <summary>
        /// Delete a habit by ID.
        /// </summary>
        /// <param name="id">The ID of the habit to delete.</param>
        /// <response code="204">Habit deleted successfully.</response>
        /// <response code="404">Habit not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteHabit(int id)
        {
            var userId = GetUserId();

            var habit = await _context.Habits
                .FirstOrDefaultAsync(h => h.Id == id && h.UserId == userId);

            if (habit == null)
                return NotFound();

            _context.Habits.Remove(habit);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
