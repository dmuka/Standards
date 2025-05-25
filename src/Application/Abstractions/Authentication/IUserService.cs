using Domain.Models.DTOs;
using Domain.Models.Users;

namespace Application.Abstractions.Authentication
{
    public interface IUserService
    {
        Task<User> Create(UserDto user);

        Task<IEnumerable<User>> GetAll();

        Task<User?> GetById(int id);
    }
}
