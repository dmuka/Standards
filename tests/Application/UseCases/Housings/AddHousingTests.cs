using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Application.UseCases.Housings;
using Domain.Aggregates.Housings;
using Moq;

namespace Tests.Application.UseCases.Housings;

[TestFixture]
public class AddHousingTests
{
    private readonly HousingId _housingId = new (Guid.CreateVersion7());
    private readonly HousingName _housingName = HousingName.Create("Housing name").Value;
    private readonly HousingShortName _housingShortName = HousingShortName.Create("Housing short name").Value;
    private readonly Address _address = Address.Create("Housing test address").Value;
    
    private HousingDto2 _housingDto;
    private Housing _housing;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<IHousingRepository> _housingRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    
    private AddHousing.CommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _housingDto = new HousingDto2
        {
            HousingName = _housingName,
            HousingShortName = _housingShortName,
            Id = _housingId, 
            Address = _address
        };
    
        _housing = Housing.Create(
            _housingName,
            _housingShortName,
            _address,
            _housingId).Value;
        
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
        }
    }
}