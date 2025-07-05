namespace E_CommerceAPI.Application.Shared.Settings;

public class JWTSettings
{
    public string Key { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public double AccessTokenExpirationMinutes { get; set; }
    public double RefreshTokenExpirationDays { get; set; }
}
