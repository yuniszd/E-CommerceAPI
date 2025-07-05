namespace E_CommerceAPI.Application.DTOs.TokenDTOs;

public record RefreshTokenRequestDto
{
    public string RefreshToken { get; set; } = null!;
}