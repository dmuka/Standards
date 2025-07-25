using Application.Abstractions.Data;
using Application.Exceptions;
using Application.UseCases.Users.EventHandlers;
using Domain.Aggregates.Persons;
using Domain.Aggregates.Users;
using Domain.Aggregates.Users.Events.Integration;
using Infrastructure.Exceptions.Enum;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.Application.UseCases.Users.EventHandlers
{
    [TestFixture]
    public class UserRegisteredIntegrationEventHandlerTests
    {
        private UserRegisteredIntegrationEvent _event;
        private static readonly CancellationToken CancellationToken = CancellationToken.None;
        
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IPersonRepository> _personRepositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<ILogger<UserRegisteredIntegrationEventHandler>> _loggerMock;
        
        private UserRegisteredIntegrationEventHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _event = new UserRegisteredIntegrationEvent(
                Guid.CreateVersion7(), 
                "John", 
                "Doe", 
                "john.doe@example.com",
                DateTime.UtcNow);

            
            _userRepositoryMock = new Mock<IUserRepository>();
            _userRepositoryMock.Setup(repo => repo.ExistsAsync(_event.UserId, CancellationToken))
                .ReturnsAsync(false);
            
            _personRepositoryMock = new Mock<IPersonRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<UserRegisteredIntegrationEventHandler>>();

            _handler = new UserRegisteredIntegrationEventHandler(
                _userRepositoryMock.Object,
                _personRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _loggerMock.Object);
        }

        [Test]
        public void Handle_UserAlreadyExists_ThrowsStandardsException()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.ExistsAsync(_event.UserId, CancellationToken))
                .ReturnsAsync(true);

            // Act & Assert
            var ex = Assert.ThrowsAsync<StandardsException>(() => _handler.Handle(_event, CancellationToken));
            using (Assert.EnterMultipleScope())
            {
                Assert.That(ex.Error, Is.EqualTo(StatusCodeByError.InternalServerError));
                Assert.That(ex.Message, Does.Contain("already exists"));
            }
        }

        [Test]
        public void Handle_UserCreationFails_ThrowsStandardsException()
        {
            // Arrange
            var @event = new UserRegisteredIntegrationEvent(
                Guid.CreateVersion7(), 
                "", 
                "Doe", 
                "john.doe@example.com",
                DateTime.UtcNow);

            // Act & Assert
            var ex = Assert.ThrowsAsync<StandardsException>(async () => await _handler.Handle(@event, CancellationToken));
            using (Assert.EnterMultipleScope())
            {
                Assert.That(ex.Error, Is.EqualTo(StatusCodeByError.InternalServerError));
                Assert.That(ex.Message, Does.Contain("creation error"));
            }
        }

        [Test]
        public void Handle_PersonCreationFails_ThrowsStandardsException()
        {
            // Arrange
            _personRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Person>(), CancellationToken))
                .ThrowsAsync(new StandardsException(StatusCodeByError.InternalServerError, "Person creation error", "Internal server error"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<StandardsException>(() => _handler.Handle(_event, CancellationToken));
            using (Assert.EnterMultipleScope())
            {
                Assert.That(ex.Error, Is.EqualTo(StatusCodeByError.InternalServerError));
                Assert.That(ex.Message, Does.Contain("creation error"));
            }
        }

        [Test]
        public async Task Handle_SuccessfulRegistration_CommitsTransaction()
        {
            // Arrange & Act
            await _handler.Handle(_event, CancellationToken);

            // Assert
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(CancellationToken), Times.Once);
        }
    }
}