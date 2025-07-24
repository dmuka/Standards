using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Application.UseCases.Housings;
using Domain.Aggregates.Common.ValueObjects;
using Domain.Aggregates.Housings;
using Moq;

namespace Tests.Application.UseCases.Housings;

[TestFixture]
public class EditHousingTests
{
    private readonly HousingId _housingId = new (Guid.CreateVersion7());
    private readonly HousingId _nonExistentHousingId = new (Guid.CreateVersion7());
    
    private HousingDto2 _housingDto;
    private Housing _housing;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<IHousingRepository> _housingRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    
    private EditHousing.CommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _housingDto = new HousingDto2
        {
            HousingName = "Housing name",
            HousingShortName = "Housing short name",
            Id = _housingId, 
            Address = "Housing test address"
        };

        _housing = Housing.Create(
            _housingDto.HousingName,
            _housingDto.HousingShortName,
            _housingDto.Address,
            _housingId,
            "Comments").Value;

        _housingRepositoryMock = new Mock<IHousingRepository>();
        _housingRepositoryMock.Setup(repository => repository.ExistsAsync(_housingId, _cancellationToken))
            .ReturnsAsync(true);
        _housingRepositoryMock.Setup(repository => repository.GetByIdAsync(_housingId, _cancellationToken))
            .ReturnsAsync(_housing);

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _handler = new EditHousing.CommandHandler(_housingRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_HousingIdNotExist_ReturnsFailure()
    {
        // Arrange
        _housingDto.Id = _nonExistentHousingId; 
        var command = new EditHousing.Command(_housingDto);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error.Code, Is.EqualTo(HousingErrors.NotFound(_nonExistentHousingId).Code));
        }
    }

    [Test]
    public async Task Handle_HousingSuccessfullyEdited_ReturnsSuccessResult()
    {
        // Arrange
        var command = new EditHousing.Command(_housingDto);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }
}