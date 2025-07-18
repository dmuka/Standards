using Domain.Aggregates.Housings;
using Domain.Aggregates.Housings.Specifications;
using Domain.Aggregates.Housings.Constants;

namespace Tests.Domain.Aggregates.Housings.Specifications;

[TestFixture]
public class HousingNameLengthMustBeValidTests
{
    [Test]
    public void IsSatisfied_ShouldReturnFailure_WhenNameIsEmpty()
    {
        // Arrange
        var specification = new HousingNameLengthMustBeValid(string.Empty);

        // Act
        var result = specification.IsSatisfied();

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.EqualTo(HousingErrors.EmptyHousingName));
        }
    }

    [Test]
    public void IsSatisfied_ShouldReturnFailure_WhenNameIsTooShort()
    {
        // Arrange
        var shortName = new string('a', HousingConstants.HousingNameMinLength - 1);
        var specification = new HousingNameLengthMustBeValid(shortName);

        // Act
        var result = specification.IsSatisfied();

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.EqualTo(HousingErrors.TooShortHousingName));
        }
    }

    [Test]
    public void IsSatisfied_ShouldReturnFailure_WhenNameIsTooLong()
    {
        // Arrange
        var longName = new string('a', HousingConstants.HousingNameMaxLength + 1);
        var specification = new HousingNameLengthMustBeValid(longName);

        // Act
        var result = specification.IsSatisfied();

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.EqualTo(HousingErrors.TooLargeHousingName));
        }
    }

    [Test]
    public void IsSatisfied_ShouldReturnSuccess_WhenNameIsValid()
    {
        // Arrange
        var validName = new string('a', (HousingConstants.HousingNameMinLength + HousingConstants.HousingNameMaxLength) / 2);
        var specification = new HousingNameLengthMustBeValid(validName);

        // Act
        var result = specification.IsSatisfied();

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }
}