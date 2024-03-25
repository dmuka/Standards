using Standards.Models.Users;

namespace Standards.Services.Interfaces
{
    public interface IUserService
    {
        //User Authenticate(string username, string password);

        Task<IEnumerable<User>> GetAll();

        Task<User> GetById(int id);
    }
}
