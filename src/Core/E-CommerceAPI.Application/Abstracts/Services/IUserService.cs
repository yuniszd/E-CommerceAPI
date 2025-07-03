using E_CommerceAPI.Application.DTOs.Users;
using E_CommerceAPI.Application.Shared;

namespace E_CommerceAPI.Application.Abstracts.Services;

public interface IUserService
{
    Task<BaseResponse<string>> Register(UserRegisterDto dto);
}
