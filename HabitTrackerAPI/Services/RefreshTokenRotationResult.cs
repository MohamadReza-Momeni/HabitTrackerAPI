using HabitTrackerAPI.Models;

namespace HabitTrackerAPI.Services
{
    public record RefreshTokenRotationResult(ApplicationUser User, RefreshTokenResult TokenResult);
}
