using Standards.Core.Models.DTOs;
using Standards.Core.Models.Users;

namespace Standards.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> Create(UserDto user);

        Task<IEnumerable<User>> GetAll();

        Task<User> GetById(int id);
    }
}
