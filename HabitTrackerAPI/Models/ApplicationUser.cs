using Microsoft.AspNetCore.Identity;

namespace HabitTrackerAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
