using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Standards.Core.CQRS.Departments;
using Standards.Core.Models.Departments;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Housings;
using Standards.CQRS.Tests.Common;
using Standards.CQRS.Tests.Constants;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;

namespace Standards.CQRS.Tests.BaseEntities.Grades
{
    [TestFixture]
    public class EditTests : BaseTestFixture
    {
        private const int ValidId = 1;
        private const int IdNotInDb = 2;

        private DepartmentDto _department;

        private Mock<IRepository> _repositoryMock;
        private CancellationToken _cancellationToken;
        private Mock<ICacheService> _cacheService;

        private IRequestHandler<Edit.Query, int> _handler;
        private IValidator<Edit.Query> _validator;

        [SetUp]
        public void Setup()
        {
            _department = DepartmentDtos[0];

            _cancellationToken = new CancellationToken();

            _repositoryMock = new Mock<IRepository>();
            _repositoryMock.Setup(_ => _.GetByIdAsync<DepartmentDto>(ValidId, _cancellationToken)).Returns(Task.FromResult(_department));
            _repositoryMock.Setup(_ => _.Update(_department));
            _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));

            _cacheService = new Mock<ICacheService>();
            
            _handler = new Edit.QueryHandler(_repositoryMock.Object, _cacheService.Object);
            _validator = new Edit.QueryValidator(_repositoryMock.Object);
        }

        [Test]
        public void Handler_IfAllDataIsValid_ReturnResult()
        {
            // Arrange
            var query = new Edit.Query(_department);
            var expected = 1;

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Handler_IfAllDataIsValid_AllCallsToDbShouldBeMade()
        {
            // Arrange
            var query = new Edit.Query(_department);

            // Act
            _handler.Handle(query, _cancellationToken);

            // Assert
            _repositoryMock.Verify(repository => repository.GetQueryable<Housing>(), Times.Once);
            _repositoryMock.Verify(repository => repository.GetQueryable<Sector>(), Times.Once);
            _repositoryMock.Verify(repository => repository.Update(It.IsAny<Department>()), Times.Once);
            _repositoryMock.Verify(repository => repository.SaveChangesAsync(_cancellationToken), Times.Once);
            _cacheService.Verify(cache => cache.Remove(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Handler_IfCancellationTokenIsActive_ReturnNull()
        {
            // Arrange
            var query = new Edit.Query(_department);
            _cancellationToken = new CancellationToken(true);
            _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(default(int)));

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            Assert.That(result, Is.EqualTo(default(int)));
        }

        [Test]
        public void Validator_IfHousingDtoIsNull_ShouldHaveValidationError()
        {
            // Arrange
            _department = null;

            var query = new Edit.Query(_department);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.DepartmentDto);
        }

        [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
        [TestCase(IdNotInDb)]
        public void Validator_IfIdIsInvalid_ShouldHaveValidationError(int id)
        {
            // Arrange
            _department.Id = default;

            var query = new Edit.Query(_department);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.DepartmentDto.Id);
        }

        [Test, TestCaseSource(nameof(NullOrEmptyString))]
        public void Validator_IfNameIsNullOrEmpty_ShouldHaveValidationError(string? name)
        {
            // Arrange
            _department.Name = name;

            var query = new Edit.Query(_department);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.DepartmentDto.Name);
        }

        [Test]
        public void Validator_IfNameIsLongerThanRequired_ShouldHaveValidationError()
        {
            // Arrange
            _department.Name = Cases.Length201;

            var query = new Edit.Query(_department);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.DepartmentDto.Name);
        }

        [Test, TestCaseSource(nameof(NullOrEmptyString))]
        public void Validator_IfShortNameIsNullOrEmpty_ShouldHaveValidationError(string? shortName)
        {
            // Arrange
            _department.ShortName = shortName;

            var query = new Edit.Query(_department);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.DepartmentDto.ShortName);
        }

        [Test]
        public void Validator_IfShortNameIsLongerThanRequired_ShouldHaveValidationError()
        {
            // Arrange
            _department.Name = Cases.Length101;

            var query = new Edit.Query(_department);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.DepartmentDto.Name);
        }

        [Test, TestCaseSource(nameof(NullOrEmptyString))]
        public void Validator_IfHousingIdsIsEmpty_ShouldHaveValidationError(string? address)
        {
            // Arrange
            _department.HousingIds = new List<int>();

            var query = new Edit.Query(_department);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.DepartmentDto.HousingIds);
        }

        [Test]
        public void Validator_IfSectorIdsIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            _department.SectorIds = new List<int>();

            var query = new Edit.Query(_department);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.DepartmentDto.SectorIds);
        }
    }
}