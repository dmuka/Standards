using Application.UseCases.Housings;
using Domain.Aggregates.Housings;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
    
    private ApplicationDbContext _dbContext;
    private GetHousingById.QueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _validHousingId = new HousingId(_validHousingIdGuid);

        _invalidHousingId = new HousingId(_invalidHousingIdGuid);
        
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _handler = new GetHousingById.QueryHandler(_dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task Handle_FloorExists_ReturnsFloor()
    {
        // Arrange
        var housing = Housing.Create(
            HousingName.Create(HousingNameValue).Value, 
            HousingShortName.Create(HousingShortNameValue).Value,
            Address.Create(AddressValue).Value,
            _validHousingId,
            "Comments").Value;
        await _dbContext.Housings2.AddAsync(housing);
        await _dbContext.SaveChangesAsync();

        var query = new GetHousingById.Query(_validHousingId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result!.Id, Is.EqualTo(_validHousingId));
            Assert.That(result.Address.Value, Is.EqualTo(AddressValue));
            Assert.That(result.Address.Value, Is.EqualTo(AddressValue));
            Assert.That(result.HousingName.Value, Is.EqualTo(HousingNameValue));
            Assert.That(result.HousingShortName!.Value, Is.EqualTo(HousingShortNameValue));
        }
    }

    [Test]
    public async Task Handle_FloorDoesNotExist_ReturnsNull()
    {
        // Arrange
        var query = new GetHousingById.Query(_invalidHousingId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Null);
    }
}