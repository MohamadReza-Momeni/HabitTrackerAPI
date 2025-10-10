using Microsoft.EntityFrameworkCore;
using HabitTrackerAPI.Models;

namespace HabitTrackerAPI.Data
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }

        public DbSet<TaskItem> Tasks { get; set; }   // Maps to Tasks table

        public DbSet<HabitItem> Habits { get; set; } // Maps to Habits table
    }
}
