using E_CommerceAPI.Application.Abstracts.Services;
using E_CommerceAPI.Application.DTOs.TokenDTOs;
using E_CommerceAPI.Application.DTOs.UserDTOs;
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
using static E_CommerceAPI.Application.Shared.Permissions;

namespace E_CommerceAPI.Persistence.Services;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ITokenService _tokenService;
    private readonly AppDbContext _context;
    private readonly IEmailService _emailService;

    public UserService(UserManager<AppUser> userManager,
                           SignInManager<AppUser> signInManager,
                           ITokenService tokenService,
                           AppDbContext context,
                           IEmailService emailService,
                           RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _context = context;
        _emailService = emailService;
        _roleManager = roleManager;
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
        var userClaims = new List<string>();
        var accessToken = _tokenService.GenerateJwtToken(user, roles, userClaims);
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
        var userClaims = new List<string>();
        var roles = await _userManager.GetRolesAsync(user);
        var newAccessToken = _tokenService.GenerateJwtToken(user, roles, userClaims);
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

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        await _emailService.SendEmailAsync(toEmail, subject, body);
    }

    public async Task SendWelcomeEmailAsync(AppUser user)
    {
        string subject = "Welcome to Our Marketplace!";
        string body = $"Hello {user.Fullname},<br><br>Welcome to our marketplace platform! We wish you great shopping and selling experiences.";
        await _emailService.SendEmailAsync(user.Email, subject, body);
    }

    public async Task SendOrderNotificationEmailAsync(AppUser buyer, AppUser seller, Product product)
    {
        // Buyer confirmation email
        string buyerSubject = "Order Confirmation";
        string buyerBody = $"Hello {buyer.Fullname},<br><br>Your order for <strong>{product.Title}</strong> has been successfully placed. Thank you for shopping with us!";
        await _emailService.SendEmailAsync(buyer.Email, buyerSubject, buyerBody);

        // Seller new order notification email
        string sellerSubject = "New Order Received";
        string sellerBody = $"Hello {seller.Fullname},<br><br>Your product <strong>{product.Title}</strong> has been ordered by {buyer.Fullname}. Please prepare it for delivery.";
        await _emailService.SendEmailAsync(seller.Email, sellerSubject, sellerBody);
    }
}

    
