namespace E_CommerceAPI.Application.DTOs.UserDTOs;

public record UserChangePasswordDto
{
    public string CurrentPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}
