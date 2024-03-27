using Standards.Models.DTOs;
using Standards.Models.Users;

namespace Standards.Services.Interfaces
{
    public interface IAuthService
    {
        public User Authenticate(string username, string password);

        public void AddPasswordHashAndSalt(UserDto userDto);
    }
}
