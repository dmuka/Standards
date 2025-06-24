using Domain.Aggregates.Housings;

namespace Tests.Domain.Aggregates.Housings;

[TestFixture]
public class HousingNameTests
{
    private const string InvalidHousingName = "";
    private const string ValidHousingName = "Housing";
    
    [Test]
    public void Create_ShouldReturnFailure_WhenHousingNameIsInvalid()
    {
        // Arrange & Act
        var result = HousingName.Create(InvalidHousingName);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.EqualTo(HousingErrors.EmptyHousingName));
        }
    }

    [Test]
    public void Create_ShouldReturnSuccess_WhenHousingNameIsValid()
    {
        // Arrange & Act
        var result = HousingName.Create(ValidHousingName);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Value, Is.EqualTo(ValidHousingName));
        }
    }
}