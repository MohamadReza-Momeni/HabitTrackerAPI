using System;

namespace HabitTrackerAPI.Services
{
    public record JwtTokenResult(string AccessToken, DateTime ExpiresAtUtc);
}
