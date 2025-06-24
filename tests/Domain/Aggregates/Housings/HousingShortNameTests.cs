using Domain.Aggregates.Housings;

namespace Tests.Domain.Aggregates.Housings;

[TestFixture]
public class HousingShortNameTests
{
    private const string InvalidHousingShortName = "";
    private const string ValidHousingShortName = "Housing";
    
    [Test]
    public void Create_ShouldReturnFailure_WhenHousingShortNameIsInvalid()
    {
        // Arrange & Act
        var result = HousingShortName.Create(InvalidHousingShortName);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.EqualTo(HousingErrors.EmptyHousingShortName));
        }
    }

    [Test]
    public void Create_ShouldReturnSuccess_WhenHousingShortNameIsValid()
    {
        // Arrange & Act
        var result = HousingName.Create(ValidHousingShortName);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Value, Is.EqualTo(ValidHousingShortName));
        }
    }
}