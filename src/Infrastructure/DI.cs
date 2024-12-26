using System.Text;
using Domain.Models.Departments;
using Domain.Models.Housings;
using Infrastructure.Data;
using Infrastructure.Data.Repositories.Implementations;
using Infrastructure.Data.Repositories.Interfaces;
using Infrastructure.Filter.Implementations;
using Infrastructure.Filter.Interfaces;
using Infrastructure.QueryableWrapper.Implementation;
using Infrastructure.QueryableWrapper.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public static class DI
{    
    /// <summary>
    /// Adds application services
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <returns>Collection of service descriptors</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddTransient<IRepository, Repository<ApplicationDbContext>>()
            .AddDbConnection(configuration)
            .AddJwtAuth(configuration)
            .RegisterQueryBuilder();
    
        return services;
    }
    
    /// <summary>
    /// Add database connection
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration">Mutable configuration object</param>
    /// <returns>Collection of service descriptors</returns>
    /// <exception cref="InvalidOperationException">Throws if connection string not found.</exception>
    private static IServiceCollection AddDbConnection(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
                               ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        var connectionStringPasswordSecret = configuration["Secrets:DefaultConnectionPassword"];
        connectionString = connectionString.Replace("passwordvalue", connectionStringPasswordSecret);
        
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString, builder => builder.MigrationsAssembly("WebApi")));

        return services;
    }
    
    /// <summary>
    /// Configure jwt authentication
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="configurationManager">Mutable configuration object</param>
    /// <returns>Collection of service descriptors</returns>
    /// <exception cref="InvalidOperationException">Throws if configuration path with bearer key not found.</exception>
    private static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration configurationManager)
    {
        var jwtBearerSecret = configurationManager["Secrets:JwtBearerKey"] 
                              ?? throw new InvalidOperationException("Configuration path with bearer key not found.");;
        var key = Encoding.ASCII.GetBytes(jwtBearerSecret);
        services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.RequireHttpsMetadata = false;
                jwtBearerOptions.SaveToken = true;
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

        return services;
    }

    /// <summary>
    /// Registers query builder and wrapper types
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <returns>Collection of service descriptors</returns>
    private static IServiceCollection RegisterQueryBuilder(this IServiceCollection services)
    {
        services
            .AddScoped<IQueryBuilder<Housing>, QueryBuilder<Housing>>()
            .AddScoped<IQueryBuilder<Room>, QueryBuilder<Room>>()
            .AddScoped<IQueryBuilder<Department>, QueryBuilder<Department>>()
            
            .AddScoped<IQueryableWrapper<Housing>, QueryableWrapper<Housing>>()
            .AddScoped<IQueryableWrapper<Room>, QueryableWrapper<Room>>()
            .AddScoped<IQueryableWrapper<Department>, QueryableWrapper<Department>>();

        return services;
    }
}