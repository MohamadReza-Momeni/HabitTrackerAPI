using Microsoft.EntityFrameworkCore;
using Task_Management_API.Models;

namespace Task_Management_API.Data
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }

        public DbSet<TaskItem> Tasks { get; set; }   // Maps to Tasks table
    }
}
