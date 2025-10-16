using HabitTrackerAPI.Models;

namespace HabitTrackerAPI.Services
{
    public interface IJwtTokenService
    {
        Task<JwtTokenResult> CreateTokenAsync(ApplicationUser user);
    }
}
