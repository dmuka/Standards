using Application.Abstractions.Authentication;
using Application.UseCases.DTOs;
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
            throw new StandardsException(StatusCodeByError.BadRequest, "Password is required", $"Password can't be null or empty ({userDto.Id})", null!);

        var userInDb = await repository.GetAsync<User>(x => x.UserName == userDto.UserName);

        if (userInDb is not null)
            throw new StandardsException(StatusCodeByError.Conflict, "Username \"" + userInDb.UserName + "\" is already taken", "Username \"" + userInDb.UserName + "\" is already taken", null!);

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
        var userInDb = await repository.GetByIdAsync<User>(userDto.Id) 
                       ?? throw new StandardsException(StatusCodeByError.NotFound, $"User not found (id: {userDto.Id})", "This user not found in the db.", null!);

        if (userDto.UserName != userInDb.UserName)
        {
            var isUserNameExist = await repository.GetAsync<User>(user => user.UserName == userDto.UserName) is not null;

            if (isUserNameExist)
                throw new StandardsException(StatusCodeByError.Conflict, "Username " + userDto.UserName + " is already taken.", "This user name already used.", null!);
        }

        var passwordSecrets = authService.GetPasswordHashAndSalt(userDto.Password);

        userInDb.UserName = userDto.UserName;
        userInDb.PasswordHash = passwordSecrets.hash;
        userInDb.PasswordSalt = passwordSecrets.salt;
        userInDb.Email = userDto.Email;

        repository.Update(userInDb);

        await repository.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var userInDb = await repository.GetByIdAsync<User>(id);

        if (userInDb is not null)
        {
            await repository.DeleteAsync(userInDb);
            await repository.SaveChangesAsync();
        }
    }

    public async Task<User?> GetById(int id)
    {
        var user = await repository.GetByIdAsync<User>(id);

        if (user is not null) user.PasswordHash = null;

        return user;
    }
}