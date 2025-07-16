using Application.UseCases.Housings;
using Domain.Aggregates.Housings;
using Moq;

namespace Tests.Application.UseCases.Housings;

[TestFixture]
public class DeleteHousingTests
{
    private const string HousingNameValue = "Housing name";
    private const string HousingShortNameValue = "Housing short name";
    private const string AddressValue = "Housing address";
    
    private readonly Guid _invalidHousingIdGuid = Guid.CreateVersion7();
    private HousingId _invalidHousingId;
    
    private readonly Guid _validHousingIdGuid = Guid.CreateVersion7();
    private HousingId _validHousingId;

    private Housing _housing;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    private Mock<IHousingRepository> _housingRepositoryMock;
    
    private DeleteHousing.CommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _validHousingId = new HousingId(_validHousingIdGuid);
        _invalidHousingId = new HousingId(_invalidHousingIdGuid);
        
        _housing = Housing.Create(
                               HousingName.Create(HousingNameValue).Value, 
                               HousingShortName.Create(HousingShortNameValue).Value,
                               Address.Create(AddressValue).Value,
                               _validHousingId,
                               "Comments").Value;
        
        _housingRepositoryMock = new Mock<IHousingRepository>();
        _housingRepositoryMock.Setup(repository => repository.ExistsAsync(_validHousingId, _cancellationToken))
            .ReturnsAsync(true);
        _housingRepositoryMock.Setup(repository => repository.GetByIdAsync(_validHousingId, _cancellationToken))
            .ReturnsAsync(_housing);
        
        _handler = new DeleteHousing.CommandHandler(_housingRepositoryMock.Object);
    }

    [Test]
    public async Task Handle_HousingExists_DeletesHousingAndReturnsSuccess()
    {
        // Arrange
        var command = new DeleteHousing.Command(_validHousingId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            _housingRepositoryMock.Verify(repository => repository.ExistsAsync(_validHousingId, _cancellationToken), Times.Once);
            _housingRepositoryMock.Verify(repository => repository.GetByIdAsync(_validHousingId, _cancellationToken), Times.Once);
        }
    }

    [Test]
    public async Task Handle_HousingDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var command = new DeleteHousing.Command(_invalidHousingId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.EqualTo(HousingErrors.NotFound(_invalidHousingId)));
        }
    }
}