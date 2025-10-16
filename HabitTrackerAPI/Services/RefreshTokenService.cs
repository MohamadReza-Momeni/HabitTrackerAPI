using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using HabitTrackerAPI.Data;
using HabitTrackerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitTrackerAPI.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly TaskDbContext _dbContext;

        public RefreshTokenService(TaskDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RefreshTokenResult> CreateRefreshTokenAsync(ApplicationUser user, TimeSpan lifetime)
        {
            await RevokeExpiredTokensAsync(user.Id);

            var (plain, tokenId, hash) = GenerateTokenArtifacts();

            var refreshToken = new RefreshToken
            {
                TokenId = tokenId,
                TokenHash = hash,
                UserId = user.Id,
                CreatedAtUtc = DateTime.UtcNow,
                ExpiresAtUtc = DateTime.UtcNow.Add(lifetime)
            };

            _dbContext.RefreshTokens.Add(refreshToken);
            await _dbContext.SaveChangesAsync();

            return new RefreshTokenResult(refreshToken, plain);
        }

        public async Task<RefreshTokenRotationResult?> RotateRefreshTokenAsync(string refreshToken, TimeSpan lifetime)
        {
            if (!TryParseRefreshToken(refreshToken, out var tokenId, out var secret))
            {
                return null;
            }

            var storedToken = await _dbContext.RefreshTokens
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.TokenId == tokenId && t.RevokedAtUtc == null);

            if (storedToken == null || storedToken.IsRevoked || storedToken.User == null)
            {
                return null;
            }

            await RevokeExpiredTokensAsync(storedToken.UserId);

            if (storedToken.IsRevoked)
            {
                return null;
            }

            var expectedHash = ComputeHash(secret, tokenId);
            if (!TimingSafeEquals(expectedHash, storedToken.TokenHash))
            {
                return null;
            }

            storedToken.RevokedAtUtc = DateTime.UtcNow;

            var (plain, newTokenId, hash) = GenerateTokenArtifacts();
            var newToken = new RefreshToken
            {
                TokenId = newTokenId,
                TokenHash = hash,
                UserId = storedToken.UserId,
                CreatedAtUtc = DateTime.UtcNow,
                ExpiresAtUtc = DateTime.UtcNow.Add(lifetime)
            };

            _dbContext.RefreshTokens.Add(newToken);
            await _dbContext.SaveChangesAsync();

            var result = new RefreshTokenResult(newToken, plain);
            return new RefreshTokenRotationResult(storedToken.User, result);
        }

        public async Task RevokeRefreshTokensForUserAsync(string userId)
        {
            var activeTokens = await _dbContext.RefreshTokens
                .Where(t => t.UserId == userId && t.RevokedAtUtc == null && t.ExpiresAtUtc > DateTime.UtcNow)
                .ToListAsync();

            foreach (var token in activeTokens)
            {
                token.RevokedAtUtc = DateTime.UtcNow;
            }

            if (activeTokens.Count > 0)
            {
                await _dbContext.SaveChangesAsync();
            }
        }

        private async Task RevokeExpiredTokensAsync(string userId)
        {
            var expiredTokens = await _dbContext.RefreshTokens
                .Where(t => t.UserId == userId && t.RevokedAtUtc == null && t.ExpiresAtUtc <= DateTime.UtcNow)
                .ToListAsync();

            foreach (var token in expiredTokens)
            {
                token.RevokedAtUtc = token.ExpiresAtUtc;
            }

            if (expiredTokens.Count > 0)
            {
                await _dbContext.SaveChangesAsync();
            }
        }

        private static (string Plain, string TokenId, string Hash) GenerateTokenArtifacts()
        {
            var tokenId = Guid.NewGuid().ToString("N");
            var secret = GenerateSecretValue();
            var plain = $"{tokenId}.{secret}";
            var hash = ComputeHash(secret, tokenId);
            return (plain, tokenId, hash);
        }

        private static bool TryParseRefreshToken(string refreshToken, out string tokenId, out string secret)
        {
            tokenId = string.Empty;
            secret = string.Empty;

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return false;
            }

            var parts = refreshToken.Split('.', 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
            {
                return false;
            }

            tokenId = parts[0];
            secret = parts[1];
            return true;
        }

        private static string GenerateSecretValue()
        {
            var bytes = new byte[64];
            RandomNumberGenerator.Fill(bytes);
            return Convert.ToBase64String(bytes);
        }

        private static string ComputeHash(string secret, string tokenId)
        {
            var data = Encoding.UTF8.GetBytes(secret + tokenId);
            var hashed = SHA256.HashData(data);
            return Convert.ToBase64String(hashed);
        }

        private static bool TimingSafeEquals(string a, string b)
        {
            var aBytes = Encoding.UTF8.GetBytes(a);
            var bBytes = Encoding.UTF8.GetBytes(b);

            if (aBytes.Length != bBytes.Length)
            {
                return false;
            }

            var result = 0;
            for (int i = 0; i < aBytes.Length; i++)
            {
                result |= aBytes[i] ^ bBytes[i];
            }

            return result == 0;
        }
    }
}

