using Application.UseCases.DTOs;
using Application.UseCases.Housings;
using Domain.Aggregates.Housings;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Tests.Application.UseCases.Housings;

[TestFixture]
public class EditHousingTests
{
    private readonly HousingId _housingId = new (Guid.CreateVersion7());
    private readonly HousingId _nonExistentHousingId = new (Guid.CreateVersion7());
    
    private HousingDto2 _housingDto;
    private Housing _housing;
    
    private ApplicationDbContext _dbContext;
    private EditHousing.CommandHandler _handler;

    [SetUp]
    public async Task Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;
        
        _housingDto = new HousingDto2
        {
            HousingName = HousingName.Create("Housing name").Value,
            HousingShortName = HousingShortName.Create("Housing short name").Value,
            HousingId = _housingId, 
            Address = Address.Create("Housing test address").Value,
        };

        _housing = Housing.Create(
            _housingDto.HousingName,
            _housingDto.HousingShortName,
            _housingDto.Address,
            _housingId,
            "Comments").Value;

        _dbContext = new ApplicationDbContext(options);
        await _dbContext.Housings2.AddAsync(_housing, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        
        _handler = new EditHousing.CommandHandler(_dbContext);
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
        var command = new EditHousing.Command(_housingDto);

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
    public async Task Handle_HousingIdNotExist_ReturnsFailure()
    {
        // Arrange
        _housingDto.HousingId = _nonExistentHousingId; 
        var command = new EditHousing.Command(_housingDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error.Code, Is.EqualTo(HousingErrors.NotFound(_nonExistentHousingId).Code));
        }
    }

    [Test]
    public async Task Handle_HousingSuccessfullyEdited_ReturnsNumberOfChanges()
    {
        // Arrange
        var command = new EditHousing.Command(_housingDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.Value, Is.EqualTo(1));
    }
}