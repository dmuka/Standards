using Application.UseCases.Housings;
using Domain.Aggregates.Housings;
using Moq;

namespace Tests.Application.UseCases.Housings;

[TestFixture]
public class GetAllHousingsTests
{
    private readonly HousingId _housingId1 = new (Guid.CreateVersion7());
    private readonly HousingId _housingId2 = new (Guid.CreateVersion7());
 
    private Housing _housing1;
    private Housing _housing2;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<IHousingRepository> _housingRepositoryMock;
    
    private GetAllHousings.QueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _housing1 = Housing.Create(
            "Housing name 1",
            "Housing short name 1",
            "Housing address line 1",
            _housingId1).Value;
        _housing2 = Housing.Create(
            "Housing name 2",
            "Housing short name 2",
            "Housing address line 2",
            _housingId2).Value;

        _housingRepositoryMock = new Mock<IHousingRepository>();
        _housingRepositoryMock.Setup(repository => repository.GetAllAsync(_cancellationToken))
            .ReturnsAsync([_housing1, _housing2]);
        
        _handler = new GetAllHousings.QueryHandler(_housingRepositoryMock.Object);
    }

    [Test]
    public async Task Handle_WhenCalled_ReturnsAllHousings()
    {
        // Arrange
        var query = new GetAllHousings.Query();

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result, Has.Exactly(1).Matches<Housing>(h => h.Id == _housing1.Id));
            Assert.That(result, Has.Exactly(1).Matches<Housing>(h => h.Id == _housing2.Id));
        }
    }

    [Test]
    public async Task Handle_WhenNoHousingsExist_ReturnsEmptyList()
    {
        // Arrange
        var query = new GetAllHousings.Query();
        _housingRepositoryMock.Setup(repository => repository.GetAllAsync(_cancellationToken))
            .ReturnsAsync([]);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result, Is.Empty);
        }
    }
}