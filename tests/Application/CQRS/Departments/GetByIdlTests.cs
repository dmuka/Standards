using Application.Abstractions.Cache;
using Application.Abstractions.Data;
using Application.UseCases.Common.GenericCRUD;
using Domain.Constants;
using Domain.Models.Departments;
using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Tests.Common;

namespace Tests.Application.CQRS.Departments;

[TestFixture]
public class GetByIdTests : BaseTestFixture
{
    private const int IdInDb = 1;
    private const int IdNotInDb = 10;
        
    private IList<Department> _departments;

    private Mock<IRepository> _repository;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;
    private Mock<ILogger<GetById>> _logger;
        
    private IRequestHandler<GetById.Query<Department>, Department?> _handler;
    private IValidator<GetById.Query<Department>> _validator;

    [SetUp]
    public void Setup()
    {
        _departments = Departments;

        _cancellationToken = CancellationToken.None;

        _repository = new Mock<IRepository>();
        _repository.Setup(_ => _.GetByIdAsync<Department>(IdInDb, _cancellationToken))
            .ReturnsAsync(_departments.First(_ => _.Id == IdInDb));

        _cacheService = new Mock<ICacheService>();
        _cacheService.Setup(cache => cache.GetById<Department>(Cache.Departments, IdInDb)).Returns(Departments[0]);

        _logger = new Mock<ILogger<GetById>>();
        
        _handler = new GetById.QueryHandler<Department>(_repository.Object, _cacheService.Object, _logger.Object);
        _validator = new GetById.QueryValidator<Department>(_repository.Object); 
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new GetById.Query<Department>(IdInDb);
        var expected = _departments.First(_ => _.Id == IdInDb);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfIdIsInvalid_ReturnResult(int id)
    {
        // Arrange
        var query = new GetById.Query<Department>(id);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Id);
    }

    [Test]
    public void Handler_IfDepartmentInCache_ReturnCachedValue()
    {
        // Arrange
        var query = new GetById.Query<Department>(IdInDb);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(Departments[0]));
        _repository.Verify(repository => repository.GetByIdAsync<Department>(IdInDb, _cancellationToken), Times.Never);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnNull()
    {
        // Arrange
        var query = new GetById.Query<Department>(IdInDb);
        _cancellationToken = new CancellationToken(true);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.Null);
    }
}