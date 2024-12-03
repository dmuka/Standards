using Application.Abstractions.Authentication;
using Application.Abstractions.Cache;
using Application.Abstractions.Configuration;
using Infrastructure.Converters;
using Infrastructure.Filter.Models;
using WebApi.Infrastructure.Services.Implementations;

namespace WebApi;

public static class DI
{    
    /// <summary>
    /// Adds application services
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <returns>Collection of service descriptors</returns>
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services
            .AddScoped<IUserService, UserService>()
            .AddScoped<IAuthService, AuthService>()
            .AddSingleton<IConfigService, ConfigService>()
            .AddCache()
            .AddControllersServices();
        
        services.AddDatabaseDeveloperPageExceptionFilter();
    
        return services;
    }

    /// <summary>
    /// Adds cache
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <returns>Collection of service descriptors</returns>
    private static IServiceCollection AddCache(this IServiceCollection services)
    {
        services
            .AddMemoryCache()
            .AddSingleton<ICacheService, CacheService>();

        return services;
    }
    
    internal static IServiceCollection AddControllersServices(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new NullableEnumConverter<FilterBy>());
                opt.JsonSerializerOptions.Converters.Add(new NullableEnumConverter<SortBy>());
            });
            
        services.AddEndpointsApiExplorer();

        return services;
    }   
}