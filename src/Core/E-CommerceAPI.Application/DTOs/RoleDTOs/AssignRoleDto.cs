namespace E_CommerceAPI.Application.DTOs.RoleDTOs;

public record AssignRoleDto
{
    public string UserId { get; set; }
    public List<string> Roles { get; set; }
}
