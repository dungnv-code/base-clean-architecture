using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class JwtTokenService : IJwtTokenService
{
    private static readonly JwtSecurityTokenHandler Handler = new();

    private readonly JwtSettings _jwt;
    private readonly SymmetricSecurityKey _key;
    private readonly SigningCredentials _credentials;

    public JwtTokenService(IOptions<JwtSettings> jwt)
    {
        _jwt = jwt.Value;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Secret));
        _credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
    }

    public string GenerateAccessToken(User user, IEnumerable<string> roles)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(roles);

        var now = DateTime.UtcNow;

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat,
                new DateTimeOffset(now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        if (!string.IsNullOrWhiteSpace(user.Email))
            claims.Add(new(JwtRegisteredClaimNames.Email, user.Email));

        if (!string.IsNullOrWhiteSpace(user.Name))
            claims.Add(new(JwtRegisteredClaimNames.Name, user.Name));

        claims.AddRange(
            roles.Where(r => !string.IsNullOrWhiteSpace(r))
                 .Distinct(StringComparer.OrdinalIgnoreCase)
                 .Select(r => new Claim(ClaimTypes.Role, r)));

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            notBefore: now,
            expires: now.AddMinutes(_jwt.ExpirationMinutes),
            signingCredentials: _credentials);

        return Handler.WriteToken(token);
    }

    public RefreshToken GenerateRefreshToken(Guid userId, string? ip = null) => new()
    {
        UserId = userId,
        Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
        ExpiresAt = DateTime.UtcNow.AddMinutes(_jwt.RefreshTokenExpirationDays * 24 * 60),
        CreatedByIp = ip
    };
}