using Microsoft.IdentityModel.Tokens;
using Standards.Data.Repositories.Interfaces;
using Standards.Models.DTOs;
using Standards.Models.Users;
using Standards.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Standards.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<User> _repository;

        public AuthService(IConfiguration configuration, IRepository<User> repository)
        {
            _configuration = configuration;
            _repository = repository;
        }

        public User Authenticate(string username, string password)
        {
            var user = _repository.Select(user => user.UserName == username);

            if (user == null) return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Secrets:JwtBearerKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Name, user.UserName),
                    new(ClaimTypes.Sid, user.Id.ToString()),
                    new(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            user.PasswordHash = null;

            return user;
        }

        public void AddPasswordHashAndSalt(UserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.Password)) 
                throw new ArgumentException("Value can't be null, empty or whitespace only string.", nameof(userDto.Password));

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                userDto.PasswordSalt = hmac.Key;
                userDto.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDto.Password));
            };
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

        private byte[] GetBytes(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);

            return bytes;
        }
    }
}
