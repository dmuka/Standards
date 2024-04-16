using Standards.Core.Models.Users;
using System.Security.Claims;

namespace Standards.Core.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User> Authenticate(string username, string password);

        ClaimsPrincipal ValidateRefreshToken(string refreshToken);

        Task<string> GenerateAccessToken(int userId);

        Task<string> GenerateRefreshToken(int userId);

        (byte[] salt, byte[] hash) GetPasswordHashAndSalt(string password);
    }
}
