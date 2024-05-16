using Standards.Core.Models.Users;
using Standards.Core.Services.Enums;
using System.Security.Claims;

namespace Standards.Core.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User> Authenticate(string username, string password);

        ClaimsPrincipal ValidateToken(string token);

        Task<string> GenerateToken(int userId, TokenType tokenType);

        (byte[] salt, byte[] hash) GetPasswordHashAndSalt(string password);
    }
}
