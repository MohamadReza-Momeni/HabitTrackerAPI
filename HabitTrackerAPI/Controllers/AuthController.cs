using HabitTrackerAPI.DTOs.Auth;
using HabitTrackerAPI.Models;
using HabitTrackerAPI.Options;
using HabitTrackerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HabitTrackerAPI.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    [Tags("Authentication")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _tokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly JwtSettings _jwtSettings;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            IJwtTokenService tokenService,
            IRefreshTokenService refreshTokenService,
            IOptions<JwtSettings> jwtOptions)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
            _jwtSettings = jwtOptions.Value;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(nameof(request.Email), "Email is already registered.");
                return ValidationProblem(ModelState);
            }

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return ValidationProblem(ModelState);
            }

            var accessToken = await _tokenService.CreateTokenAsync(user);
            var refreshLifetime = TimeSpan.FromMinutes(_jwtSettings.RefreshTokenExpirationMinutes);
            await _refreshTokenService.RevokeRefreshTokensForUserAsync(user.Id);
            var refreshToken = await _refreshTokenService.CreateRefreshTokenAsync(user, refreshLifetime);

            return Ok(new AuthResponse
            {
                AccessToken = accessToken.AccessToken,
                ExpiresAtUtc = accessToken.ExpiresAtUtc,
                UserId = user.Id,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName,
                RefreshToken = refreshToken.PlainTextToken,
                RefreshTokenExpiresAtUtc = refreshToken.Token.ExpiresAtUtc
            });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Unauthorized(new { error = "Invalid credentials." });

            var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
                return Unauthorized(new { error = "Invalid credentials." });

            var accessToken = await _tokenService.CreateTokenAsync(user);
            var refreshLifetime = TimeSpan.FromMinutes(_jwtSettings.RefreshTokenExpirationMinutes);
            await _refreshTokenService.RevokeRefreshTokensForUserAsync(user.Id);
            var refreshToken = await _refreshTokenService.CreateRefreshTokenAsync(user, refreshLifetime);

            return Ok(new AuthResponse
            {
                AccessToken = accessToken.AccessToken,
                ExpiresAtUtc = accessToken.ExpiresAtUtc,
                UserId = user.Id,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName,
                RefreshToken = refreshToken.PlainTextToken,
                RefreshTokenExpiresAtUtc = refreshToken.Token.ExpiresAtUtc
            });
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<AuthResponse>> Refresh(RefreshRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var rotation = await _refreshTokenService.RotateRefreshTokenAsync(request.RefreshToken, TimeSpan.FromMinutes(_jwtSettings.RefreshTokenExpirationMinutes));
            if (rotation == null)
                return Unauthorized(new { error = "Invalid or expired refresh token." });

            var accessToken = await _tokenService.CreateTokenAsync(rotation.User);

            return Ok(new AuthResponse
            {
                AccessToken = accessToken.AccessToken,
                ExpiresAtUtc = accessToken.ExpiresAtUtc,
                UserId = rotation.User.Id,
                Email = rotation.User.Email ?? string.Empty,
                FullName = rotation.User.FullName,
                RefreshToken = rotation.TokenResult.PlainTextToken,
                RefreshTokenExpiresAtUtc = rotation.TokenResult.Token.ExpiresAtUtc
            });
        }
    }
}







