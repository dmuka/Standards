using Standards.Data.Repositories.Interfaces;
using Standards.Exceptions;
using Standards.Models.DTOs;
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
                user.PasswordHash = null;

                return user;
            });
        }

        public async Task<User> Create(UserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.Password))
                throw new StandardsException("Password is required");

            var userInDB = await _repository.SelectAsync(x => x.UserName == userDto.UserName);

            if (userInDB is not null)
                throw new StandardsException("Username \"" + userInDB.UserName + "\" is already taken");

            _authService.AddPasswordHashAndSalt(userDto);

            var user = new User
            {
                UserName = userDto.UserName,
                PasswordHash = userDto.PasswordHash,
                PasswordSalt = userDto.PasswordSalt,
                Email = userDto.Email
            };

            _repository.Add(user);
            await _repository.SaveAsync();

            return user;
        }


        public async Task Update(UserDto userDto)
        {
            var userInDB = _repository.GetById(userDto.Id) ?? throw new StandardsException("User not found");

            if (userDto.UserName != userInDB.UserName)
            {
                var isUserNameExist = await _repository.SelectAsync(x => x.UserName == userDto.UserName) is not null;

                if (isUserNameExist)    
                    throw new StandardsException("Username " + userDto.UserName + " is already taken.");
            }

            _authService.AddPasswordHashAndSalt(userDto);

            userInDB.UserName = userDto.UserName;
            userInDB.PasswordHash = userDto.PasswordHash;
            userInDB.PasswordSalt = userDto.PasswordSalt;
            userInDB.Email = userDto.Email;

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

            if (user is not null) user.PasswordHash = null;

            return user;
        }
    }
}