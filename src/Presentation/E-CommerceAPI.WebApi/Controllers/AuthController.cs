// File: Controllers/AuthController.cs

using E_CommerceAPI.Application.Abstracts.Services;
using E_CommerceAPI.Application.DTOs.TokenDTOs;
using E_CommerceAPI.Application.DTOs.Users;
using E_CommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_CommerceAPI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public AuthController(UserManager<AppUser> userManager,
                              ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            var user = new AppUser
            {
                Fullname = dto.Fullname,
                UserName = dto.Email,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            if (!await _userManager.IsInRoleAsync(user, dto.Role))
            {
                await _userManager.AddToRoleAsync(user, dto.Role);
            }

            return Ok("Registration successful.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return Unauthorized("Invalid credentials.");

            var userClaims = new List<string>();

            var roles = await _userManager.GetRolesAsync(user);

            var accessToken = _tokenService.GenerateJwtToken(user, roles, userClaims);

            var refreshToken = _tokenService.GenerateRefreshToken(user.Id);

            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            return Ok(new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            });
        }

        [HttpPost("refresh token")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto dto)
        {
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == dto.RefreshToken));

            if (user == null)
                return Unauthorized("Invalid refresh token.");

            var refreshToken = user.RefreshTokens.First(x => x.Token == dto.RefreshToken);

            if (refreshToken.Expires < DateTime.UtcNow)
                return Unauthorized("Refresh token expired.");

            var roles = await _userManager.GetRolesAsync(user);
            var userClaims = new List<string>();

            var newAccessToken = _tokenService.GenerateJwtToken(user, roles, userClaims );
            var newRefreshToken = _tokenService.GenerateRefreshToken(user.Id);

            user.RefreshTokens.Remove(refreshToken);
            user.RefreshTokens.Add(newRefreshToken);

            await _userManager.UpdateAsync(user);

            return Ok(new AuthResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token
            });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound("User not found.");

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                user.Id,
                user.Fullname,
                user.Email,
                Roles = roles
            });
        }
    }
}
