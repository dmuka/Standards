using Domain.Aggregates.Floors;
using Domain.Aggregates.Floors.Specifications;

namespace Tests.Domain.Aggregates.Floors.Specifications;

[TestFixture]
public class FloorNumberMustBeGreaterThanZeroTests
{
    [Test]
    public void IsSatisfied_ShouldReturnFailure_WhenFloorNumberIsLessThanOne()
    {
        // Arrange
        var specification = new FloorNumberMustBeGreaterThanZero(0);

        // Act
        var result = specification.IsSatisfied();

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.EqualTo(FloorErrors.InvalidFloorCount));
        }
    }

    [Test]
    public void IsSatisfied_ShouldReturnSuccess_WhenFloorNumberIsOneOrGreater()
    {
        // Arrange
        var specification = new FloorNumberMustBeGreaterThanZero(1);

        // Act
        var result = specification.IsSatisfied();

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }
}