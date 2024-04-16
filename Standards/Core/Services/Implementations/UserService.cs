using Standards.Core.Models.DTOs;
using Standards.Core.Models.Users;
using Standards.Core.Services.Interfaces;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Exceptions;
using Standards.Infrastructure.Exceptions.Enum;
using System.Data;

namespace Standards.Core.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IRepository _repository;
        private readonly IAuthService _authService;

        public UserService(IAuthService authService, IRepository repository)
        {
            _authService = authService;
            _repository = repository;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var users = await _repository.GetListAsync<User>();

            return users.Select(user =>
            {
                user.PasswordHash = null;

                return user;
            });
        }

        public async Task<User> Create(UserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.Password))
                throw new StandardsException(StatusCodeByError.InternalServerError, "Password is required", null, null);

            var userInDB = await _repository.GetAsync<User>(x => x.UserName == userDto.UserName);

            if (userInDB is not null)
                throw new StandardsException(StatusCodeByError.InternalServerError, "Username \"" + userInDB.UserName + "\" is already taken", null, null);

            var passwordSecrets = _authService.GetPasswordHashAndSalt(userDto.Password);

            var user = new User
            {
                UserName = userDto.UserName,
                PasswordHash = passwordSecrets.hash,
                PasswordSalt = passwordSecrets.salt,
                Email = userDto.Email
            };

            _repository.Add(user);
            await _repository.SaveChangesAsync();

            return user;
        }


        public async Task Update(UserDto userDto)
        {
            var userInDB = await _repository.GetByIdAsync<User>(userDto.Id) ?? throw new StandardsException(StatusCodeByError.NotFound, "User not found", null, null);

            if (userDto.UserName != userInDB.UserName)
            {
                var isUserNameExist = await _repository.GetAsync<User>(user => user.UserName == userDto.UserName) is not null;

                if (isUserNameExist)
                    throw new StandardsException(StatusCodeByError.Conflict, "Username " + userDto.UserName + " is already taken.", null, null);
            }

            var passwordSecrets = _authService.GetPasswordHashAndSalt(userDto.Password);

            userInDB.UserName = userDto.UserName;
            userInDB.PasswordHash = passwordSecrets.hash;
            userInDB.PasswordSalt = passwordSecrets.salt;
            userInDB.Email = userDto.Email;

            _repository.Update(userInDB);

            await _repository.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var userInDB = await _repository.GetByIdAsync<User>(id);

            if (userInDB is not null)
            {
                await _repository.DeleteAsync(userInDB);
                await _repository.SaveChangesAsync();
            }
        }

        public async Task<User> GetById(int id)
        {
            var user = await _repository.GetByIdAsync<User>(id);

            if (user is not null) user.PasswordHash = null;

            return user;
        }
    }
}