using E_CommerceAPI.Application.Abstracts.Services;
using E_CommerceAPI.Application.DTOs.RoleDTOs;
using E_CommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace E_CommerceAPI.WebApi.Controllers
{
    namespace E_CommerceAPI.WebApi.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        [Authorize(Roles = "Admin,Moderator")]
        public class RoleController : ControllerBase
        {
            private readonly IRoleService _roleService;

            public RoleController(IRoleService roleService)
            {
                _roleService = roleService;
            }

            // ✅ GET: api/role
            [HttpGet]
            public async Task<IActionResult> GetRoles()
            {
                var roles = await _roleService.GetRolesAsync();
                return Ok(roles);
            }

            // ✅ POST: api/role
            [HttpPost]
            [Authorize(Roles = "Admin")]
            public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto dto)
            {
                var success = await _roleService.CreateRoleAsync(dto.RoleName);
                if (success)
                    return Ok($"Role '{dto.RoleName}' has been created.");

                return BadRequest($"The role '{dto.RoleName}' already exists.");
            }

            // ✅ DELETE: api/role/{roleName}
            [HttpDelete("{roleName}")]
            [Authorize(Roles = "Admin")]
            public async Task<IActionResult> DeleteRole(string roleName)
            {
                var success = await _roleService.DeleteRoleAsync(roleName);
                if (success)
                    return Ok($"Role '{roleName}' has been deleted.");

                return NotFound($"Role '{roleName}' not found.");
            }

            // ✅ POST: api/role/assign
            [HttpPost("assign")]
            [Authorize(Roles = "Admin")]
            public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto dto)
            {
                var (success, errors) = await _roleService.AssignRolesAsync(dto.UserId, dto.Roles);

                if (success)
                    return Ok($"Roles [{string.Join(", ", dto.Roles)}] have been assigned to the user.");

                return BadRequest(errors);
            }
        }
    }
}
