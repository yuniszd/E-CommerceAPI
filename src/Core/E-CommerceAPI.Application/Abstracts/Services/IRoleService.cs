using E_CommerceAPI.Application.DTOs.RoleDTOs;

namespace E_CommerceAPI.Application.Abstracts.Services;

public interface IRoleService
{
    Task<List<RoleResponseDto>> GetRolesAsync();
    Task<bool> CreateRoleAsync(string roleName);
    Task<bool> DeleteRoleAsync(string roleName);
    Task<(bool Success, List<string> Errors)> AssignRolesAsync(string userId, List<string> roles);
}
