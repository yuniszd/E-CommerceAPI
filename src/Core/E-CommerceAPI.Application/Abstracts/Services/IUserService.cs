using E_CommerceAPI.Application.DTOs.TokenDTOs;
using E_CommerceAPI.Application.DTOs.UserDTOs;
using E_CommerceAPI.Application.DTOs.Users;
using E_CommerceAPI.Application.Shared;
using E_CommerceAPI.Domain.Entities;

namespace E_CommerceAPI.Application.Abstracts.Services;

public interface IUserService
{
    Task<BaseResponse<TokenResponse>> LoginAsync(UserLoginDto dto);
    Task<BaseResponse<string>> RegisterAsync(UserRegisterDto dto);
    Task<BaseResponse<TokenResponse>> RefreshTokenAsync(RefreshTokenRequestDto dto);
    Task<BaseResponse<string>> AddRole(UserAddRoleDto dto);
    Task<BaseResponse<string>> ResetPassword(UserChangePasswordDto dto);
    Task<BaseResponse<string>> ConfirmEmail(string userId,string token);
}
