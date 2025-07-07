using E_CommerceAPI.Application.DTOs.TokenDTOs;
using E_CommerceAPI.Application.DTOs.UserDTOs;
using E_CommerceAPI.Application.DTOs.Users;
using E_CommerceAPI.Application.Shared;
using E_CommerceAPI.Domain.Entities;
using static E_CommerceAPI.Application.Shared.Permissions;

namespace E_CommerceAPI.Application.Abstracts.Services;

public interface IUserService
{
    public interface IUserService
    {
        Task<BaseResponse<TokenResponse>> LoginAsync(UserLoginDto dto);
        Task<BaseResponse<TokenResponse>> RefreshTokenAsync(string refreshToken);
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task SendWelcomeEmailAsync(AppUser user);
        Task SendOrderNotificationEmailAsync(AppUser buyer, AppUser seller, Product product);
    }
}
