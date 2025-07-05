namespace E_CommerceAPI.Application.DTOs.Users;

public record UserRegisterDto
{
    public string Fullname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Role { get; set; } = null!;
}
