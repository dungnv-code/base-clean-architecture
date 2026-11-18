using Domain.Entities;
public interface IJwtTokenService
{
    string GenerateAccessToken(User user, IEnumerable<string> roles);
    RefreshToken GenerateRefreshToken(Guid userId, string? ip = null);
}