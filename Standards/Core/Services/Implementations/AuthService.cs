using Microsoft.IdentityModel.Tokens;
using Standards.Core.Models.Users;
using Standards.Core.Services.Enums;
using Standards.Core.Services.Interfaces;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Standards.Core.Services.Implementations
{
    public class AuthService(IConfiguration configuration, IRepository repository) : IAuthService
    {
        private const int AccessTokenExpirationInMinutes = 15;
        private const int RefreshTokenExpirationInMonths = 1;

        public async Task<User> Authenticate(string username, string password)
        {
            var user = await repository.GetAsync<User>(user => user.UserName == username);

            if (user == null) return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)) return null;

            user.RefreshToken = await GenerateToken(user.Id, TokenType.Refresh);
            user.AccessToken = await GenerateToken(user.Id, TokenType.Access);

            user.PasswordHash = null;
            user.PasswordSalt = null;

            return user;
        }

        public (byte[] salt, byte[] hash) GetPasswordHashAndSalt(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value can't be null, empty or whitespace only string.", nameof(password));

            byte[] salt;
            byte[] hash;

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                salt = hmac.Key;
                hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }

            return (salt, hash);
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = GetKey();

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                return principal;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> GenerateToken(int userId, TokenType tokenType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = GetKey();

            var user = await repository.GetByIdAsync<User>(userId);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GetClaimsIdentity(user),
                Expires = tokenType == TokenType.Access 
                    ? DateTime.UtcNow.AddMinutes(AccessTokenExpirationInMinutes) 
                    : DateTime.UtcNow.AddMonths(RefreshTokenExpirationInMonths),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var accessToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(accessToken);
        }

        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));
            if (passwordHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", nameof(passwordHash));
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", nameof(storedSalt));

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }

            return true;
        }

        private byte[]? GetKey()
        {
            return Encoding.ASCII.GetBytes(configuration["Secrets:JwtBearerKey"]);
        }

        private static ClaimsIdentity GetClaimsIdentity(User user)
        {
            return new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Name, user.UserName),
                    new(ClaimTypes.Sid, user.Id.ToString()),
                    new(ClaimTypes.Email, user.Email)
                });
        }
    }
}