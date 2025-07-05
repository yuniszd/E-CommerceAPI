using E_CommerceAPI.Application.Abstracts.Services;
using E_CommerceAPI.Application.DTOs.TokenDTOs;
using E_CommerceAPI.Application.DTOs.UserDTOs;
using E_CommerceAPI.Application.DTOs.Users;
using E_CommerceAPI.Application.Shared;
using E_CommerceAPI.Domain.Entities;
using E_CommerceAPI.Persistence.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace E_CommerceAPI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly TokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public AccountsController(UserManager<AppUser> userManager,
                                  SignInManager<AppUser> signInManager,
                                  TokenService tokenService,
                                  IConfiguration configuration,
                                  AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _configuration = configuration;
            _context = context;
        }

        // ✅ POST: api/accounts/register
        [ProducesResponseType(typeof(BaseResponse<TokenResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status401Unauthorized)]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            if (dto.Role != "Buyer" && dto.Role != "Seller")
                return BadRequest("Role must be either 'Buyer' or 'Seller'.");

            var user = new AppUser
            {
                Fullname = dto.Fullname,
                UserName = dto.Email,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, dto.Role);

            return Ok("User registered successfully as " + dto.Role + ".");
        }
        
        // ✅ POST: api/accounts/login
        [ProducesResponseType(typeof(BaseResponse<TokenResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status401Unauthorized)]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized("Invalid email or password.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded)
                return Unauthorized("Invalid email or password.");

            var roles = await _userManager.GetRolesAsync(user);

            // ✅ Access Token Generate
            var accessToken = _tokenService.GenerateJwtToken(user, roles);

            // ✅ Refresh Token Generate
            var refreshToken = _tokenService.GenerateRefreshToken(user.Id);

            // Refresh Token-i user obyektinə əlavə et
            user.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Token = accessToken,
                RefreshToken = refreshToken.Token
            });
        }


        // ✅ GET: api/accounts/profile
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
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

        // ✅ POST: api/accounts/change-password
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound("User not found.");

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Password changed successfully.");
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto dto)
        {
            var user = await _userManager.Users
                .Where(u => u.RefreshTokens.Any(rt => rt.Token == dto.RefreshToken))
                .FirstOrDefaultAsync();

            if (user == null)
                return Unauthorized("Invalid refresh token.");

            var refreshToken = user.RefreshTokens.FirstOrDefault(rt => rt.Token == dto.RefreshToken);

            if (refreshToken == null || refreshToken.Expires < DateTime.UtcNow)
                return Unauthorized("Refresh token expired.");

            var roles = await _userManager.GetRolesAsync(user);
            var newAccessToken = _tokenService.GenerateJwtToken(user, roles);

            // Yeni refresh token yaradırıq
            var newRefreshToken = _tokenService.GenerateRefreshToken(user.Id);

            // Köhnəsini sil, yenisini əlavə et
            user.RefreshTokens.Remove(refreshToken);
            user.RefreshTokens.Add(newRefreshToken);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken.Token
            });
        }

    }
}
