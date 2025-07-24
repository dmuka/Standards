using Domain.Aggregates.Common.ValueObjects;
using Domain.Aggregates.Housings;
using Domain.Constants;

namespace Tests.Domain.Aggregates.Housings;

[TestFixture]
public class HousingTests
{
    private const string HousingNameValue = "Housing name";
    private const string HousingShortNameValue = "Housing short name";
    private const string AddressValue = "Housing address";
    
    private readonly HousingId _housingId = new (Guid.CreateVersion7());
    private readonly HousingName _housingName = HousingName.Create(HousingNameValue).Value;
    private readonly HousingShortName _housingShortName = HousingShortName.Create(HousingShortNameValue).Value;
    private readonly Address _address = Address.Create(AddressValue).Value;
    private const string Comments = "Some comments";
    
    [Test]
    public void Create_ShouldReturnSuccess_WhenAllParametersAreValid()
    {
        // Arrange & Act
        var result = Housing.Create(HousingNameValue, HousingShortNameValue, AddressValue, _housingId, Comments);

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
        var result = Housing.Create(HousingNameValue, HousingShortNameValue, AddressValue, _housingId);

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