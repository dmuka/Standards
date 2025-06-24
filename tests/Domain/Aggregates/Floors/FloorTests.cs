using Domain.Aggregates.Floors;
using Domain.Aggregates.Housings;
using Moq;

namespace Tests.Domain.Aggregates.Floors;

[TestFixture]
public class FloorTests
{
    private const int InvalidFloorNumber = 0;
    private const int ValidFloorNumber = 1;
    
    private Mock<IFloorUniqueness> _uniquenessCheckerMock;
    private HousingId _housingId;

    [SetUp]
    public void SetUp()
    {
        _uniquenessCheckerMock = new Mock<IFloorUniqueness>();
        _housingId = new HousingId(Guid.NewGuid());
    }

    [Test]
    public async Task Create_ShouldReturnFailure_WhenFloorNumberIsInvalid()
    {
        // Arrange & Act
        var result = await Floor.Create(InvalidFloorNumber, _housingId, _uniquenessCheckerMock.Object);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.EqualTo(FloorErrors.InvalidFloorCount));
        }
    }

    [Test]
    public async Task Create_ShouldReturnFailure_WhenFloorIsNotUnique()
    {
        // Arrange
        _uniquenessCheckerMock.Setup(x => x.IsUniqueAsync(ValidFloorNumber, _housingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await Floor.Create(ValidFloorNumber, _housingId, _uniquenessCheckerMock.Object);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.EqualTo(FloorErrors.FloorAlreadyExist));
        }
    }

    [Test]
    public async Task Create_ShouldReturnSuccess_WhenFloorNumberIsValidAndUnique()
    {
        // Arrange
        _uniquenessCheckerMock.Setup(x => x.IsUniqueAsync(ValidFloorNumber, _housingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await Floor.Create(ValidFloorNumber, _housingId, _uniquenessCheckerMock.Object);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Number, Is.EqualTo(ValidFloorNumber));
            Assert.That(result.Value.HousingId, Is.EqualTo(_housingId));
        }
    }
}