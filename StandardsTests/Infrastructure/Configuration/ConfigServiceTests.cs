using Microsoft.Extensions.Configuration;
using Standards.Infrastructure.Exceptions;
using Standards.Infrastructure.Services.Implementations;
using Standards.Infrastructure.Services.Interfaces;

namespace Standards.CQRS.Tests.Infrastructure.Configuration;

public class ConfigServiceTests
{
    private const string FirstConfigurationPath = "Section:first";
    private const string SecondConfigurationPath = "Section:second";
    private const string WrongConfigurationPath = "Section:wrong";

    private const string FirstValue = "First Value";
    private const string SecondValue = "1";

    private IConfigurationRoot _configuration;
    private IConfigService _configService;

    [SetUp]
    public void Setup()
    {
        var builder = new ConfigurationBuilder();
        _configuration = builder.AddInMemoryCollection(
            new Dictionary<string, string?>
            {
                [FirstConfigurationPath] = FirstValue,
                [SecondConfigurationPath] = SecondValue
            }).Build();

        _configService = new ConfigService(_configuration);
    }

    [Test]
    public void GetValue_IfValidConfigurationPath_ShouldReturnValue()
    {
        // Arrange
        // Act
        var result = _configService.GetValue<string>(FirstConfigurationPath);

        // Assert
        Assert.That(result, Is.EqualTo(FirstValue));
    }

    [Test]
    public void GetValue_IfInvalidConfigurationPath_ShouldThrowException()
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<StandardsConfigValueNotFoundException>(
            () => _configService.GetValue<string>(WrongConfigurationPath));
    }
}