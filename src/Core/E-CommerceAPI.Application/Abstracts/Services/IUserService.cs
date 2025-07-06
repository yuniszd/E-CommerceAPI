using E_CommerceAPI.Application.DTOs.Users;
using E_CommerceAPI.Application.Shared;
using E_CommerceAPI.Domain.Entities;

namespace E_CommerceAPI.Application.Abstracts.Services;

public interface IUserService
{
    Task<BaseResponse<TokenResponse>> LoginAsync(UserLoginDto dto);
    Task<BaseResponse<TokenResponse>> RefreshTokenAsync(string refreshToken);
}
