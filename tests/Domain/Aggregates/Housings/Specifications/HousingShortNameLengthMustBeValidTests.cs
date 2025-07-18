using Domain.Aggregates.Housings;
using Domain.Aggregates.Housings.Specifications;
using Domain.Aggregates.Housings.Constants;

namespace Tests.Domain.Aggregates.Housings.Specifications;

[TestFixture]
public class HousingShortNameLengthMustBeValidTests
{
    [Test]
    public void IsSatisfied_ShouldReturnFailure_WhenShortNameIsEmpty()
    {
        // Arrange
        var specification = new HousingShortNameLengthMustBeValid(string.Empty);

        // Act
        var result = specification.IsSatisfied();

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.EqualTo(HousingErrors.EmptyHousingShortName));
        }
    }

    [Test]
    public void IsSatisfied_ShouldReturnFailure_WhenShortNameIsTooShort()
    {
        // Arrange
        var shortName = new string('a', HousingConstants.HousingShortNameMinLength - 1);
        var specification = new HousingShortNameLengthMustBeValid(shortName);

        // Act
        var result = specification.IsSatisfied();

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.EqualTo(HousingErrors.TooShortHousingShortName));
        }
    }

    [Test]
    public void IsSatisfied_ShouldReturnFailure_WhenShortNameIsTooLong()
    {
        // Arrange
        var longName = new string('a', HousingConstants.HousingShortNameMaxLength + 1);
        var specification = new HousingShortNameLengthMustBeValid(longName);

        // Act
        var result = specification.IsSatisfied();

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.EqualTo(HousingErrors.TooLargeHousingShortName));
        }
    }

    [Test]
    public void IsSatisfied_ShouldReturnSuccess_WhenShortNameIsValid()
    {
        // Arrange
        var validName = new string('a', (HousingConstants.HousingShortNameMinLength + HousingConstants.HousingShortNameMaxLength) / 2);
        var specification = new HousingShortNameLengthMustBeValid(validName);

        // Act
        var result = specification.IsSatisfied();

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }
}