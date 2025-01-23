using System.Security.Claims;
using Application.Abstractions.Authentication.Enums;
using Domain.Models.Users;

namespace Application.Abstractions.Authentication
{
    public interface IAuthService
    {
        Task<User?> Authenticate(string username, string password);

        ClaimsPrincipal ValidateToken(string token);

        Task<string> GenerateToken(int userId, TokenType tokenType);

        (byte[] salt, byte[] hash) GetPasswordHashAndSalt(string password);
    }
}
