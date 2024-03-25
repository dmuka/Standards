using Standards.Data.Repositories.Interfaces;
using Standards.Exceptions;
using Standards.Models.Users;
using Standards.Services.Interfaces;
using System.Data;

namespace Standards.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _repository;
        private readonly IAuthService _authService;

        public UserService(IAuthService authService, IRepository<User> repository)
        {
            _authService = authService;
            _repository = repository;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var users = await _repository.GetAllAsync();
            
            return users.Select(user =>
            {
                user.Password = null;

                return user;
            });
        }

        public async Task<User> Create(User requestor, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new StandardsException("Password is required");

            var userInDB = await _repository.SelectAsync(x => x.Username == requestor.Username);

            if (userInDB is not null)
                throw new StandardsException("Username \"" + userInDB.Username + "\" is already taken");


            _repository.Add(requestor);
            await _repository.SaveAsync();

            return requestor;
        }


        public async Task Update(User requestor, string? password = null)
        {
            var userInDB = _repository.GetById(requestor.Id);

            if (userInDB is null) throw new StandardsException("User not found");

            if (requestor.Username != userInDB.Username)
            {
                var isUserNameExist = await _repository.SelectAsync(x => x.Username == requestor.Username) is not null;

                if (isUserNameExist)    
                    throw new StandardsException("Username " + requestor.Username + " is already taken.");
            }

            userInDB.FirstName = requestor.FirstName;
            userInDB.LastName = requestor.LastName;
            userInDB.Username = requestor.Username;

            _authService.AddPasswordHashAndSalt(userInDB);

            _repository.Update(userInDB);
            await _repository.SaveAsync();
        }

        public async Task Delete(int id)
        {
            var userInDB = _repository.GetById(id);

            if (userInDB is not null)
            {
                _repository.Remove(id);
                await _repository.SaveAsync();
            }
        }

        public async Task<User> GetById(int id)
        {
            var user = await _repository.GetByIdAsync(id);

            if (user is not null) user.Password = null;

            return user;
        }
    }
}