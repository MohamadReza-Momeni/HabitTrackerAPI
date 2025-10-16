using System;
using HabitTrackerAPI.Models;

namespace HabitTrackerAPI.Services
{
    public interface IRefreshTokenService
    {
        Task<RefreshTokenResult> CreateRefreshTokenAsync(ApplicationUser user, TimeSpan lifetime);

        Task<RefreshTokenRotationResult?> RotateRefreshTokenAsync(string refreshToken, TimeSpan lifetime);

        Task RevokeRefreshTokensForUserAsync(string userId);
    }
}

