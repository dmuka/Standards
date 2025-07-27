using Application.UseCases.Housings;
using Domain.Aggregates.Housings;
using Moq;

namespace Tests.Application.UseCases.Housings;

[TestFixture]
public class GetHousingByIdTests
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
    
    private GetHousingById.QueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _validHousingId = new HousingId(_validHousingIdGuid);
        _invalidHousingId = new HousingId(_invalidHousingIdGuid);
        
        _housing = Housing.Create(
            HousingNameValue, 
            HousingShortNameValue,
            AddressValue,
            _validHousingId,
            "Comments").Value;        

        _housingRepositoryMock = new Mock<IHousingRepository>();
        _housingRepositoryMock.Setup(repository => repository.ExistsAsync(_validHousingId, _cancellationToken))
            .ReturnsAsync(true);
        _housingRepositoryMock.Setup(repository => repository.GetByIdAsync(_validHousingId, _cancellationToken))
            .ReturnsAsync(_housing);
        
        _handler = new GetHousingById.QueryHandler(_housingRepositoryMock.Object);
    }

    [Test]
    public async Task Handle_HousingExists_ReturnsHousing()
    {
        // Arrange
        var query = new GetHousingById.Query(_validHousingId);

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Id, Is.EqualTo(_validHousingId));
            Assert.That(result.Value.Address.Value, Is.EqualTo(AddressValue));
            Assert.That(result.Value.Address.Value, Is.EqualTo(AddressValue));
            Assert.That(result.Value.HousingName.Value, Is.EqualTo(HousingNameValue));
            Assert.That(result.Value.HousingShortName!.Value, Is.EqualTo(HousingShortNameValue));
        }
    }

    [Test]
    public async Task Handle_HousingDoesNotExist_ReturnsNull()
    {
        // Arrange
        var query = new GetHousingById.Query(_invalidHousingId);

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.EqualTo(HousingErrors.NotFound(_invalidHousingId)));
        }
    }
}