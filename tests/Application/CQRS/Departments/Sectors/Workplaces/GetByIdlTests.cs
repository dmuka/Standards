using Application.Abstractions.Cache;
using Application.UseCases.Common.GenericCRUD;
using Domain.Constants;
using Domain.Models.Departments;
using FluentValidation;
using FluentValidation.TestHelper;
using Infrastructure.Data.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Tests.Common;

namespace Tests.Application.CQRS.Departments.Sectors.Workplaces;

[TestFixture]
public class GetByIdTests : BaseTestFixture
{
    private const int IdInDb = 1;
    private const int IdNotInDb = 10;

    private Mock<IRepository> _repository;
    private Mock<ICacheService> _cacheMock;
    private Mock<ILogger<GetById>> _logger;
    
    private CancellationToken _cancellationToken;
    
    private List<Workplace> _sectors;
    
    private IRequestHandler<GetById.Query<Workplace>, Workplace> _handler;
    private IValidator<GetById.Query<Workplace>> _validator;

    [SetUp]
    public void Setup()
    {
        _sectors = Workplaces;

        _cancellationToken = CancellationToken.None;

        _repository = new Mock<IRepository>();
        _repository.Setup(_ => _.GetByIdAsync<Workplace>(IdInDb, _cancellationToken))
            .Returns(Task.FromResult(_sectors.First(_ => _.Id == IdInDb)));

        _cacheMock = new Mock<ICacheService>();
        _cacheMock.Setup(cache => cache.GetById<Workplace>(Cache.Workplaces, IdInDb)).Returns(Workplaces[0]);

        _logger = new Mock<ILogger<GetById>>();

        _handler = new GetById.QueryHandler<Workplace>(_repository.Object, _cacheMock.Object, _logger.Object); 
        _validator = new GetById.QueryValidator<Workplace>(_repository.Object); 
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new GetById.Query<Workplace>(IdInDb);
        var expected = _sectors.First(_ => _.Id == IdInDb);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    [TestCase(IdNotInDb)]
    public void Validator_IfIdIsInvalid_ReturnResult(int id)
    {
        // Arrange
        var query = new GetById.Query<Workplace>(id);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Id);
    }

    [Test]
    public void Handler_IfWorkplaceInCache_ReturnCachedValue()
    {
        // Arrange
        var query = new GetById.Query<Workplace>(IdInDb);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(Workplaces[0]));
        _repository.Verify(repository => repository.GetByIdAsync<Workplace>(IdInDb, _cancellationToken), Times.Never);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnNull()
    {
        // Arrange
        var query = new GetById.Query<Workplace>(IdInDb);
        _cancellationToken = new CancellationToken(true);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.Null);
    }
}