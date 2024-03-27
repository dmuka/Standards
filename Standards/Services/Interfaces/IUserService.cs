using Standards.Models.DTOs;
using Standards.Models.Users;

namespace Standards.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> Create(UserDto user);

        Task<IEnumerable<User>> GetAll();

        Task<User> GetById(int id);
    }
}
