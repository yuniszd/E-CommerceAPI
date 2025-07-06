using E_CommerceAPI.Application.Abstracts.Services;
using E_CommerceAPI.Application.DTOs.Users;
using E_CommerceAPI.Application.Shared;
using E_CommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_CommerceAPI.Persistence.Services;

public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly AppDbContext _context;

        public UserService(UserManager<AppUser> userManager,
                           SignInManager<AppUser> signInManager,
                           ITokenService tokenService,
                           AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _context = context;
        }

        public async Task<BaseResponse<TokenResponse>> LoginAsync(UserLoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return new BaseResponse<TokenResponse>
                {
                    Success = false,
                    Message = "Invalid email or password."
                };

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded)
                return new BaseResponse<TokenResponse>
                {
                    Success = false,
                    Message = "Invalid email or password."
                };

            var roles = await _userManager.GetRolesAsync(user);

            var accessToken = _tokenService.GenerateJwtToken(user, roles);
            var refreshToken = _tokenService.GenerateRefreshToken(user.Id);

            user.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            var tokenResponse = new TokenResponse
            {
                Token = accessToken,
                RefreshToken = refreshToken.Token
            };

            return new BaseResponse<TokenResponse>
            {
                Success = true,
                Message = "Login successful.",
                Data = tokenResponse
            };
        }

        public async Task<BaseResponse<TokenResponse>> RefreshTokenAsync(string refreshToken)
        {
            var user = await _userManager.Users
                .Where(u => u.RefreshTokens.Any(rt => rt.Token == refreshToken))
                .FirstOrDefaultAsync();

            if (user == null)
                return new BaseResponse<TokenResponse>
                {
                    Success = false,
                    Message = "Invalid refresh token."
                };

            var tokenEntity = user.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshToken);

            if (tokenEntity == null || tokenEntity.Expires < DateTime.UtcNow)
                return new BaseResponse<TokenResponse>
                {
                    Success = false,
                    Message = "Refresh token expired."
                };

            var roles = await _userManager.GetRolesAsync(user);
            var newAccessToken = _tokenService.GenerateJwtToken(user, roles);
            var newRefreshToken = _tokenService.GenerateRefreshToken(user.Id);

            user.RefreshTokens.Remove(tokenEntity);
            user.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync();

            var tokenResponse = new TokenResponse
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken.Token
            };

            return new BaseResponse<TokenResponse>
            {
                Success = true,
                Message = "Token refreshed successfully.",
                Data = tokenResponse
            };
        }
    }
