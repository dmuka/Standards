using Application.UseCases.DTOs;
using Application.UseCases.Housings;
using Domain.Aggregates.Housings;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.Application.UseCases.Housings;

[TestFixture]
public class AddHousingTests
{
    private HousingDto2 _housingDto;

    private Mock<HousingName> _housingNameMock;
    
    private ApplicationDbContext _dbContext;
    private AddHousing.CommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        _dbContext = new ApplicationDbContext(options);

        _housingNameMock = new Mock<HousingName>();
        
        _housingDto = new HousingDto2
        {
            HousingName = HousingName.Create("Housing name").Value,
            HousingShortName = HousingShortName.Create("Housing short name").Value,
            HousingId = new HousingId(Guid.CreateVersion7()), 
            Address = Address.Create("Housing test address").Value
        };
        
        _handler = new AddHousing.CommandHandler(_dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task Handle_HousingCreationFails_ReturnsFailure()
    {
        // Arrange
        _housingDto.HousingName = null!;
        var command = new AddHousing.Command(_housingDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error.Code, Is.EqualTo(HousingErrors.EmptyHousingName.Code));
        }
    }

    [Test]
    public async Task Handle_HousingSuccessfullyAdded_ReturnsNumberOfChanges()
    {
        // Arrange
        var command = new AddHousing.Command(_housingDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.Value, Is.EqualTo(1));
    }
}