using Domain.Aggregates.Floors;
using Domain.Aggregates.Housings;
using Moq;

namespace Tests.Domain.Aggregates.Floors;

[TestFixture]
public class FloorTests
{
    private const int InvalidFloorNumber = 0;
    private const int ValidFloorNumber = 1;
    
    private readonly HousingId _housingId = new (Guid.NewGuid());

    [Test]
    public void Create_ShouldReturnFailure_WhenFloorNumberIsInvalid()
    {
        // Arrange & Act
        var result = Floor.Create(InvalidFloorNumber, _housingId);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.EqualTo(FloorErrors.InvalidFloorCount));
        }
    }

    [Test]
    public void Create_ShouldReturnSuccess_WhenFloorNumberIsValid()
    {
        // Arrange & Act
        var result = Floor.Create(ValidFloorNumber, _housingId);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Number, Is.EqualTo(ValidFloorNumber));
            Assert.That(result.Value.HousingId, Is.EqualTo(_housingId));
        }
    }
}