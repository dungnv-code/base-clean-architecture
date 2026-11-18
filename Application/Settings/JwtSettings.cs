
// Application/Settings/JwtSettings.cs
public class JwtSettings
{
    public const string SectionName = "Jwt";
    public string Secret { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpirationMinutes { get; set; } = 15; 
    public int RefreshTokenExpirationDays { get; set; } = 7;
}
