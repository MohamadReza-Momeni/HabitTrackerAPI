namespace HabitTrackerAPI.Options
{
    public class JwtSettings
    {
        public string Issuer { get; set; } = string.Empty;

        public string Audience { get; set; } = string.Empty;

        public string SecretKey { get; set; } = string.Empty;

        public int ExpirationMinutes { get; set; } = 60;

        public int RefreshTokenExpirationMinutes { get; set; } = 4320;
    }
}
