using Domain.Aggregates.Housings;
using Domain.Constants;

namespace Tests.Domain.Aggregates.Housings;

[TestFixture]
public class HousingTests
{
    private readonly HousingId _housingId = new (Guid.CreateVersion7());
    private readonly HousingName _housingName = HousingName.Create("Valid Housing Name").Value;
    private readonly HousingShortName _housingShortName = HousingShortName.Create("Valid Housing Short Name").Value;
    private readonly Address _address = Address.Create("Valid Address Value").Value;
    private const string Comments = "Some comments";
    
    [Test]
    public void Create_ShouldReturnSuccess_WhenAllParametersAreValid()
    {
        // Arrange & Act
        var result = Housing.Create(_housingId, _housingName, _housingShortName, _address, Comments);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Id, Is.EqualTo(_housingId));
            Assert.That(result.Value.HousingName, Is.EqualTo(_housingName));
            Assert.That(result.Value.HousingShortName, Is.EqualTo(_housingShortName));
            Assert.That(result.Value.Address, Is.EqualTo(_address));
            Assert.That(result.Value.Comments, Is.EqualTo(Comments));
        }
    }

    [Test]
    public void Create_ShouldReturnSuccess_WhenCommentsAreNull()
    {
        // Arrange & Act
        var result = Housing.Create(_housingId, _housingName, _housingShortName, _address);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Comments, Is.Null);
        }
    }

    [Test]
    public void GetCacheKey_ShouldReturnCorrectCacheKey()
    {
        // Act
        var cacheKey = Housing.GetCacheKey();

        // Assert
        Assert.That(cacheKey, Is.EqualTo(Cache.Housings));
    }
}