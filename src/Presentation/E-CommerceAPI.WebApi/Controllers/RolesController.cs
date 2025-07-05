using E_CommerceAPI.Application.DTOs.RoleDTOs;
using E_CommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace E_CommerceAPI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Moderator")]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // ✅ GET: api/role
        [HttpGet]
        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult GetRoles()
        {
            var roles = _roleManager.Roles
                .Select(r => new RoleResponseDto
                {
                    Id = r.Id,
                    Name = r.Name
                })
                .ToList();

            return Ok(roles);
        }

        // ✅ POST: api/role
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto dto)
        {
            if (await _roleManager.RoleExistsAsync(dto.RoleName))
                return BadRequest($"The role '{dto.RoleName}' already exists.");

            var result = await _roleManager.CreateAsync(new IdentityRole(dto.RoleName));

            if (result.Succeeded)
                return Ok($"Role '{dto.RoleName}' has been created.");

            return BadRequest(result.Errors);
        }

        // ✅ DELETE: api/role/{roleName}
        [HttpDelete("{roleName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
                return NotFound($"Role '{roleName}' not found.");

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
                return Ok($"Role '{roleName}' has been deleted.");

            return BadRequest(result.Errors);
        }

        // ✅ POST: api/role/assign
        [HttpPost("assign")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
                return NotFound("User not found.");

            foreach (var roleName in dto.Roles)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                    return NotFound($"Role '{roleName}' not found.");

                var alreadyInRole = await _userManager.IsInRoleAsync(user, roleName);
                if (!alreadyInRole)
                {
                    var result = await _userManager.AddToRoleAsync(user, roleName);
                    if (!result.Succeeded)
                        return BadRequest(result.Errors);
                }
            }

            return Ok($"Roles [{string.Join(", ", dto.Roles)}] have been assigned to the user.");
        }
    }
}
