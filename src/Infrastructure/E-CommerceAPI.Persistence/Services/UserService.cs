using E_CommerceAPI.Application.Abstracts.Services;
using E_CommerceAPI.Application.DTOs.Users;
using E_CommerceAPI.Application.Shared;
using E_CommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace E_CommerceAPI.Persistence.Services;

public class UserService : IUserService
{
    public UserManager<AppUser> _userManager {  get; set; }
    public UserService(UserManager<AppUser> userManager)
    {
        _userManager = userManager; 
    }
    public async Task<BaseResponse<string>> Register(UserRegisterDto dto)
    {
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);

        if (existingUser != null)
        {
            return new BaseResponse<string>
            {
                Success = false,
                Message = "This email is already registered.",
                Errors = new List<string> { "Email already exists" }
            };
        }

        if (dto.Role != "Buyer" && dto.Role != "Seller")
        {
            return new BaseResponse<string>
            {
                Success = false,
                Message = "Invalid role. Only Buyer or Seller can be assigned.",
                Errors = new List<string> { "Role must be Buyer or Seller" }
            };
        }

        var user = new AppUser
        {
            Fullname = dto.Fullname,
            UserName = dto.Email,
            Email = dto.Email
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            return new BaseResponse<string>
            {
                Success = false,
                Message = "Registration failed.",
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        await _userManager.AddToRoleAsync(user, dto.Role);

        return new BaseResponse<string>
        {
            Success = true,
            Message = $"Registration successful. Role assigned: {dto.Role}",
            Data = user.Id
        };
    }
}
