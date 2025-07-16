using System.Text;
using System.Text.Json;
using Infrastructure.Converters;

namespace Tests.Infrastructure.Converters;

[TestFixture]
public class NullableEnumConverterTests
{
    public enum TestEnum
    {
        One,
        Two,
        Three
    };
    private NullableEnumConverter<TestEnum> _converter;
    private JsonReaderOptions _readerOptions;
    private JsonSerializerOptions _serializerOptions;

    [SetUp]
    public void Setup()
    {
        _converter = new NullableEnumConverter<TestEnum>();
        _readerOptions = new JsonReaderOptions
        {
            AllowTrailingCommas = true,
            CommentHandling = JsonCommentHandling.Skip
        };
        _serializerOptions = new JsonSerializerOptions();
    }

    [TestCase("\"One\"", TestEnum.One)]
    [TestCase("\"Two\"", TestEnum.Two)]
    [TestCase("\"Three\"", TestEnum.Three)]
    public void Read_IfValidStringEnumValue_ShouldReturnValue(string json, TestEnum expected)
    {
        // Arrange
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json), _readerOptions);
        
        // Act
        reader.Read(); // position reader to the first token
        var result = _converter.Read(ref reader, typeof(TestEnum), _serializerOptions);
        
        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public void Read_IfInvalidStringEnumValue_ShouldReturnValue()
    {
        // Arrange
        const string json = "\"Wrong\"";
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json), _readerOptions);
        
        // Act
        reader.Read(); // position reader to the first token
        var result = _converter.Read(ref reader, typeof(TestEnum), _serializerOptions);
        
        // Assert
        Assert.That(result, Is.Null);
    }

    [TestCase("\"One\"", TestEnum.One)]
    [TestCase("\"Two\"", TestEnum.Two)]
    [TestCase("\"Three\"", TestEnum.Three)]
    public void ReadExtension_IfValidStringEnumValue_ShouldReturnValue(string json, TestEnum expected)
    {
        // Arrange
        // Act
        var result = _converter.Read(json, _serializerOptions);
        
        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public void ReadExtension_IfInvalidStringEnumValue_ShouldReturnValue()
    {
        // Arrange
        const string json = "\"Wrong\"";
        
        // Act
        var result = _converter.Read(json, _serializerOptions);
        
        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void Write_IfValueIsNotNull_ShouldCallWriteStringValue()
    {
        // Arrange
        var value = TestEnum.One;
        
        // Act
        var result = _converter.Write(value);
        
        // Assert
        Assert.That(result, Is.EqualTo("\"One\""));
    }

    [Test]
    public void Write_IfValueIsNull_ShouldCallWriteNullValue()
    {
        // Arrange
        TestEnum? value = null;
        
        // Act
        var result = _converter.Write(value);
        
        // Assert
        Assert.That(result, Is.EqualTo("null"));
    }
}