using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Application.UseCases.Housings;
using Core.Results;
using Domain.Aggregates.Housings;
using Moq;

namespace Tests.Application.UseCases.Housings;

[TestFixture]
public class AddHousingTests
{
    private const string HousingNameValue = "Housing name";
    private const string HousingShortNameValue = "Housing short name";
    private const string AddressValue = "Housing address";
    
    private readonly HousingId _housingId = new (Guid.CreateVersion7());
    
    private HousingDto2 _housingDto;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<IHousingRepository> _housingRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    
    private AddHousing.CommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _housingDto = new HousingDto2
        {
            HousingName = HousingNameValue,
            HousingShortName = HousingShortNameValue,
            Id = _housingId, 
            Address = AddressValue
        };
        
        _housingRepositoryMock = new Mock<IHousingRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _handler = new AddHousing.CommandHandler(_housingRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_HousingSuccessfullyAdded_ReturnsSuccessResult()
    {
        // Arrange
        var command = new AddHousing.Command(_housingDto);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            _housingRepositoryMock.Verify(repository => repository.AddAsync(It.IsAny<Housing>(), _cancellationToken), Times.Once);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.CommitAsync(_cancellationToken), Times.Once);
        }
    }

    [Test]
    public async Task Handle_HousingCreationFails_ReturnsZero()
    {
        // Arrange
        _housingDto.HousingName = "";
        var command = new AddHousing.Command(_housingDto);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
            Assert.That(result.Error.Description, Is.EqualTo("One or more validation errors occurred"));
        }
    }
}