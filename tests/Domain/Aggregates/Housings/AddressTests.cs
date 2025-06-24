using Domain.Aggregates.Housings;

namespace Tests.Domain.Aggregates.Housings;

[TestFixture]
public class AddressTests
{
    private const string InvalidAddress = "";
    private const string ValidAddress = "123 Main St, London";
    
    [Test]
    public void Create_ShouldReturnFailure_WhenAddressIsInvalid()
    {
        // Arrange & Act
        var result = Address.Create(InvalidAddress);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.EqualTo(HousingErrors.EmptyAddress)); // Assuming this is the error for empty address
        }
    }

    [Test]
    public void Create_ShouldReturnSuccess_WhenAddressIsValid()
    {
        // Arrange & Act
        var result = Address.Create(ValidAddress);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Value, Is.EqualTo(ValidAddress));
        }
    }
}