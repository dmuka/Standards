using System.Text;
using System.Text.Json;

namespace Infrastructure.Converters;

public static class JsonConverterTestExtensions
{
    public static TResult? Read<TResult>(
        this NullableEnumConverter<TResult> converter, 
        string token,
        JsonSerializerOptions? options = null) where TResult : struct, Enum
    {
        options ??= JsonSerializerOptions.Default;
        var bytes = Encoding.UTF8.GetBytes(token);
        var reader = new Utf8JsonReader(bytes);
        
        reader.Read();
        var result = converter.Read(ref reader, typeof(TResult), options);
        
        return result;
    }

    public static (bool IsSuccessful, TResult? Result) TryRead<TResult>(
        this NullableEnumConverter<TResult> converter,
        string token,
        JsonSerializerOptions? options = null) where TResult : struct, Enum
    {
        try
        {
            var result = Read(converter, token, options);
            return (true, result);
        }
        catch (Exception)
        {
            return (IsSuccessful: false, Result: default);
        }
    }

    public static string Write<T>(
        this NullableEnumConverter<T> converter, 
        T? value,
        JsonSerializerOptions? options = null) where T : struct, Enum
    {
        options ??= JsonSerializerOptions.Default;
        using var ms = new MemoryStream();
        using var writer = new Utf8JsonWriter(ms);
        converter.Write(writer, value, options);
        writer.Flush();
        var result = Encoding.UTF8.GetString(ms.ToArray());
        return result;
    }

    public static (bool IsSuccessful, string? Result) TryWrite<T>(
        this NullableEnumConverter<T> converter,
        T? value,
        JsonSerializerOptions? options = null) where T : struct, Enum
    {
        try
        {
            var result = Write(converter, value, options);
            return (true, result);
        }
        catch
        {
            return (false, null);
        }
    }
}