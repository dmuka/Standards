using Application.Abstractions.Data;
using Application.Exceptions;
using Domain.Aggregates.Persons;
using Domain.Aggregates.Users;
using Domain.Aggregates.Users.Events.Integration;
using Infrastructure.Exceptions.Enum;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Users.EventHandlers;

public sealed class UserRegisteredIntegrationEventHandler(
    IUserRepository userRepository,
    IPersonRepository personRepository,
    IUnitOfWork unitOfWork,
    ILogger<UserRegisteredIntegrationEventHandler> logger) : INotificationHandler<UserRegisteredIntegrationEvent>
{
    public async Task Handle(
        UserRegisteredIntegrationEvent notification, 
        CancellationToken cancellationToken)
    {
        var isUserExists = await userRepository.ExistsAsync(notification.UserId, cancellationToken);
        if (isUserExists) throw new StandardsException(
            StatusCodeByError.InternalServerError,
            $"User {notification.UserId}: already exists ({nameof(UserRegisteredIntegrationEventHandler)}).",
            "Internal server error");
        
        var userCreationResult = User.Create(
            notification.UserId,
            notification.FirstName,
            notification.LastName,
            notification.Email);
        if (userCreationResult.IsFailure) throw new StandardsException(
            StatusCodeByError.InternalServerError,
            $"User {notification.UserId} creation error ({nameof(UserRegisteredIntegrationEventHandler)}, {userCreationResult.Error.Description}).",
            "Internal server error");
        
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await userRepository.AddAsync(userCreationResult.Value, cancellationToken);

            var personCreationResult = Person.Create(
                notification.FirstName,
                "",
                notification.LastName,
                DateOnly.FromDateTime(DateTime.UtcNow),
                notification.UserId);
            if (personCreationResult.IsFailure)
                throw new StandardsException(
                    StatusCodeByError.InternalServerError,
                    $"Person {userCreationResult.Value.PersonId}: creation error ({nameof(UserRegisteredIntegrationEventHandler)}, {personCreationResult.Error.Description}).",
                    "Internal server error");

            await personRepository.AddAsync(personCreationResult.Value, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            logger.LogError("Rollback changes: handler - {Handler}, user id - {Id}, message - {Message}", 
                nameof(UserRegisteredIntegrationEventHandler), notification.UserId, ex.Message);
                
            throw;
        }
    }
}