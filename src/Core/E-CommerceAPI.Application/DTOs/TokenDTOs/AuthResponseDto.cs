namespace E_CommerceAPI.Application.DTOs.TokenDTOs;

public record AuthResponseDto
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}
