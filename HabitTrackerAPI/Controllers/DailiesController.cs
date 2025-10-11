using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HabitTrackerAPI.Data;
using HabitTrackerAPI.DTOs;
using HabitTrackerAPI.Mappers;
using HabitTrackerAPI.Models.Enums;

namespace HabitTrackerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Tags("Dailies")]
    public class DailiesController : ControllerBase
    {
        private readonly TaskDbContext _context;

        public DailiesController(TaskDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieve all daily items with optional filtering, sorting, and pagination.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DailyResponse>), 200)]
        public async Task<ActionResult<object>> GetDailies(
            Priority? priority,
            RepeatDuration? repeatDuration,
            DateTime? startDateFrom,
            DateTime? startDateTo,
            int page = 1,
            int pageSize = 10,
            string sortBy = "CreatedAt",
            string order = "desc")
        {
            var query = _context.DailyItems
                .Include(d => d.Checklists)
                .AsQueryable();

            if (priority.HasValue)
                query = query.Where(d => d.Priority == priority.Value);

            if (repeatDuration.HasValue)
                query = query.Where(d => d.RepeatDuration == repeatDuration.Value);

            if (startDateFrom.HasValue)
                query = query.Where(d => d.StartDate >= startDateFrom.Value);

            if (startDateTo.HasValue)
                query = query.Where(d => d.StartDate <= startDateTo.Value);

            query = sortBy.ToLower() switch
            {
                "startdate" => order == "asc"
                    ? query.OrderBy(d => d.StartDate)
                    : query.OrderByDescending(d => d.StartDate),
                "priority" => order == "asc"
                    ? query.OrderBy(d => d.Priority)
                    : query.OrderByDescending(d => d.Priority),
                _ => order == "asc"
                    ? query.OrderBy(d => d.CreatedAt)
                    : query.OrderByDescending(d => d.CreatedAt)
            };

            var totalCount = await query.CountAsync();
            var dailies = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                page,
                pageSize,
                totalCount,
                dailies = dailies.Select(d => d.ToDailyResponse())
            });
        }

        /// <summary>
        /// Retrieve a specific daily item by ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DailyResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<DailyResponse>> GetDaily(int id)
        {
            var daily = await _context.DailyItems
                .Include(d => d.Checklists)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (daily == null)
                return NotFound();

            return Ok(daily.ToDailyResponse());
        }

        /// <summary>
        /// Create a new daily item with optional checklist entries.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(DailyResponse), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<DailyResponse>> CreateDaily(CreateDailyRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var daily = request.ToDailyItem();

            _context.DailyItems.Add(daily);
            await _context.SaveChangesAsync();

            daily = await _context.DailyItems
                .Include(d => d.Checklists)
                .FirstAsync(d => d.Id == daily.Id);

            return CreatedAtAction(nameof(GetDaily), new { id = daily.Id }, daily.ToDailyResponse());
        }

        /// <summary>
        /// Update an existing daily item.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(DailyResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateDaily(int id, UpdateDailyRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var daily = await _context.DailyItems
                .Include(d => d.Checklists)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (daily == null)
                return NotFound();

            daily.UpdateDailyItem(request);

            await _context.SaveChangesAsync();

            daily = await _context.DailyItems
                .Include(d => d.Checklists)
                .FirstAsync(d => d.Id == id);

            return Ok(daily.ToDailyResponse());
        }

        /// <summary>
        /// Delete a daily item by ID.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteDaily(int id)
        {
            var daily = await _context.DailyItems
                .Include(d => d.Checklists)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (daily == null)
                return NotFound();

            _context.DailyItems.Remove(daily);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
