using Application.Abstractions.Authentication;
using Domain.Models.DTOs;
using Domain.Models.Users;
using Infrastructure.Data.Repositories.Interfaces;
using Infrastructure.Exceptions;
using Infrastructure.Exceptions.Enum;

namespace WebApi.Infrastructure.Services.Implementations;

public class UserService(IAuthService authService, IRepository repository) : IUserService
{
    public async Task<IEnumerable<User>> GetAll()
    {
        var users = await repository.GetListAsync<User>();

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

        var userInDB = await repository.GetAsync<User>(x => x.UserName == userDto.UserName);

        if (userInDB is not null)
            throw new StandardsException(StatusCodeByError.InternalServerError, "Username \"" + userInDB.UserName + "\" is already taken", null, null);

        var passwordSecrets = authService.GetPasswordHashAndSalt(userDto.Password);

        var user = new User
        {
            UserName = userDto.UserName,
            PasswordHash = passwordSecrets.hash,
            PasswordSalt = passwordSecrets.salt,
            Email = userDto.Email
        };

        await repository.AddAsync(user);
        await repository.SaveChangesAsync();

        return user;
    }


    public async Task Update(UserDto userDto)
    {
        var userInDB = await repository.GetByIdAsync<User>(userDto.Id) ?? throw new StandardsException(StatusCodeByError.NotFound, "User not found", null, null);

        if (userDto.UserName != userInDB.UserName)
        {
            var isUserNameExist = await repository.GetAsync<User>(user => user.UserName == userDto.UserName) is not null;

            if (isUserNameExist)
                throw new StandardsException(StatusCodeByError.Conflict, "Username " + userDto.UserName + " is already taken.", null, null);
        }

        var passwordSecrets = authService.GetPasswordHashAndSalt(userDto.Password);

        userInDB.UserName = userDto.UserName;
        userInDB.PasswordHash = passwordSecrets.hash;
        userInDB.PasswordSalt = passwordSecrets.salt;
        userInDB.Email = userDto.Email;

        repository.Update(userInDB);

        await repository.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var userInDB = await repository.GetByIdAsync<User>(id);

        if (userInDB is not null)
        {
            await repository.DeleteAsync(userInDB);
            await repository.SaveChangesAsync();
        }
    }

    public async Task<User> GetById(int id)
    {
        var user = await repository.GetByIdAsync<User>(id);

        if (user is not null) user.PasswordHash = null;

        return user;
    }
}