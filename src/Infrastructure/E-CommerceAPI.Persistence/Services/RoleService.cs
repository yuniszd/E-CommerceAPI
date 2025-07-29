using E_CommerceAPI.Application.Abstracts.Services;
using E_CommerceAPI.Application.DTOs.RoleDTOs;
using E_CommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace E_CommerceAPI.Persistence.Services;

public class RoleService : IRoleService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;

    public RoleService(RoleManager<IdentityRole> roleManager,
                       UserManager<AppUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<List<RoleResponseDto>> GetRolesAsync()
    {
        var roles = _roleManager.Roles
            .Select(r => new RoleResponseDto
            {
                Id = r.Id,
                Name = r.Name ?? string.Empty
            })
            .ToList();

        return await Task.FromResult(roles);
    }

    public async Task<bool> CreateRoleAsync(string roleName)
    {
        if (await _roleManager.RoleExistsAsync(roleName))
            return false;

        var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
        return result.Succeeded;
    }

    public async Task<bool> DeleteRoleAsync(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
            return false;

        var result = await _roleManager.DeleteAsync(role);
        return result.Succeeded;
    }

    public async Task<(bool Success, List<string> Errors)> AssignRolesAsync(string userId, List<string> roles)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return (false, new List<string> { "User not found." });

        var errors = new List<string>();

        foreach (var roleName in roles)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                errors.Add($"Role '{roleName}' not found.");
                continue;
            }

            if (!await _userManager.IsInRoleAsync(user, roleName))
            {
                var result = await _userManager.AddToRoleAsync(user, roleName);
                if (!result.Succeeded)
                    errors.AddRange(result.Errors.Select(e => e.Description));
            }
        }

        return (errors.Count == 0, errors);
    }
}