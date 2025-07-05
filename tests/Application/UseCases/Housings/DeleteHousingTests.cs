using Application.UseCases.Housings;
using Domain.Aggregates.Floors;
using Domain.Aggregates.Housings;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
    
    private ApplicationDbContext _dbContext;
    private DeleteHousing.CommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _validHousingId = new HousingId(_validHousingIdGuid);
        _invalidHousingId = new HousingId(_invalidHousingIdGuid);
        
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _handler = new DeleteHousing.CommandHandler(_dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task Handle_HousingExists_DeletesHousingAndReturnsSuccess()
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

        var command = new DeleteHousing.Command(_validHousingId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.EqualTo(1));
            Assert.That(await _dbContext.Housings2.FindAsync(_validHousingId), Is.Null);
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