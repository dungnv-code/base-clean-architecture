
using Microsoft.AspNetCore.Identity;

namespace Application.Helpers;

public static class HashPass
{
    private static readonly PasswordHasher<string> Hasher = new();

    public static string HashPassword(string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        return Hasher.HashPassword(string.Empty, password);
    }

    public static bool VerifyPassword(
        string hashedPassword,
        string providedPassword)
    {
        if (string.IsNullOrWhiteSpace(hashedPassword) ||
            string.IsNullOrEmpty(providedPassword))
        {
            return false;
        }

        var result = Hasher.VerifyHashedPassword(
            string.Empty,
            hashedPassword,
            providedPassword);

        return result is PasswordVerificationResult.Success
            or PasswordVerificationResult.SuccessRehashNeeded;
    }
}
