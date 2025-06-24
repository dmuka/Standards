using Domain.Aggregates.Housings;
using Domain.Aggregates.Housings.Specifications;
using Domain.Aggregates.Housings.Constants;

namespace Tests.Domain.Aggregates.Housings.Specifications;

[TestFixture]
public class AddressMustBeValidTests
{
    [Test]
    public void IsSatisfied_ShouldReturnFailure_WhenAddressIsEmpty()
    {
        // Arrange
        var specification = new AddressMustBeValid(string.Empty);

        // Act
        var result = specification.IsSatisfied();

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.EqualTo(HousingErrors.EmptyAddress));
        }
    }

    [Test]
    public void IsSatisfied_ShouldReturnFailure_WhenAddressIsTooShort()
    {
        // Arrange
        var shortAddress = new string('a', HousingConstants.AddressMinLength - 1);
        var specification = new AddressMustBeValid(shortAddress);

        // Act
        var result = specification.IsSatisfied();

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Error, Is.EqualTo(HousingErrors.TooShortAddress));
    }

    [Test]
    public void IsSatisfied_ShouldReturnFailure_WhenAddressIsTooLong()
    {
        // Arrange
        var longAddress = new string('a', HousingConstants.AddressMaxLength + 1);
        var specification = new AddressMustBeValid(longAddress);

        // Act
        var result = specification.IsSatisfied();

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.EqualTo(HousingErrors.TooLargeAddress));
        }
    }

    [Test]
    public void IsSatisfied_ShouldReturnSuccess_WhenAddressIsValid()
    {
        // Arrange
        var validAddress = new string('a', (HousingConstants.AddressMinLength + HousingConstants.AddressMaxLength) / 2);
        var specification = new AddressMustBeValid(validAddress);

        // Act
        var result = specification.IsSatisfied();

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }
}