namespace E_CommerceAPI.Application.DTOs.Users;

public record UserLoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}