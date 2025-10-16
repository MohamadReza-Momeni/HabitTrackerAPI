using HabitTrackerAPI.Models;

namespace HabitTrackerAPI.Services
{
    public record RefreshTokenResult(RefreshToken Token, string PlainTextToken);
}
