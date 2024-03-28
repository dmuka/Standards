using Standards.Models.Users;
using System.Security.Claims;

namespace Standards.Services.Interfaces
{
    public interface IAuthService
    {
        User Authenticate(string username, string password);

        ClaimsPrincipal ValidateRefreshToken(string refreshToken);

        string GenerateAccessToken(int userId);

        string GenerateRefreshToken(int userId);

        (byte[] salt, byte[] hash) GetPasswordHashAndSalt(string password);
    }
}
