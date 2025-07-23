using Application.Abstractions.Authentication;
using Application.Abstractions.Cache;
using Application.Abstractions.Configuration;
using Infrastructure.Converters;
using Infrastructure.Filter.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using WebApi.Infrastructure;
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
            .AddAuth()
            .AddCache()
            .AddSwagger()
            .AddConfigService()
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
            .AddScoped<ICacheService, CacheService>();

        return services;
    }

    /// <summary>
    /// Adds config service
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <returns>Collection of service descriptors</returns>
    private static IServiceCollection AddConfigService(this IServiceCollection services)
    {
        services
            .AddSingleton<IConfigService, ConfigService>();

        return services;
    }

    /// <summary>
    /// Adds authentication logic
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <returns>Collection of service descriptors</returns>
    private static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services
            .AddScoped<IUserService, UserService>()
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<ITokenProvider, TokenProvider>();

        return services;
    }

    /// <summary>
    /// Adds Swagger generator
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <returns>Collection of service descriptors</returns>
    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {          
        services.AddSwaggerGen(swaggerOptions =>
        {
            swaggerOptions.SwaggerDoc("v1", new OpenApiInfo { Title = "Standards", Version = "v1" });
                
            var security = new OpenApiSecurityScheme
            {
                Name = HeaderNames.Authorization, 
                Type = SecuritySchemeType.ApiKey, 
                In = ParameterLocation.Header,
                Description = "JWT Authorization header", 
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme, 
                    Type = ReferenceType.SecurityScheme 
                }
            };
            swaggerOptions.AddSecurityDefinition(security.Reference.Id, security); 
            swaggerOptions.AddSecurityRequirement(new OpenApiSecurityRequirement {{security, []}});
        });

        return services;
    }
    
    private static IServiceCollection AddControllersServices(this IServiceCollection services)
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