using Application.Abstractions.Configuration;
using Infrastructure.Exceptions;
using Infrastructure.Exceptions.Enum;

namespace WebApi.Infrastructure.Services.Implementations;

public class ConfigService(IConfiguration configuration) : IConfigService
{
    public T GetValue<T>(string valuePath)
    {
        var path = valuePath.Split(':');
        
        var value = configuration.GetSection(path[0]).GetValue<T>(path[1]);

        if (value is not null)
        {
            return value;
        }
        
        throw new StandardsConfigValueNotFoundException(
            StatusCodeByError.NotFound,
            $"Wrong configuration value path ({valuePath}) or no such value.",
            null);
    }
}