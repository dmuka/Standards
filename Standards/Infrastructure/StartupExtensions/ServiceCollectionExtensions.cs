using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Standards.Core.CQRS.Common.GenericCRUD;
using Standards.Core.Models.Departments;
using Standards.Core.Models.Persons;
using Standards.Infrastructure.Data;

namespace Standards.Infrastructure.StartupExtensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add database connection
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configurationManager">Mutable configuration object</param>
    /// <returns>Collection of service descriptors</returns>
    /// <exception cref="InvalidOperationException">Throws if connection string not found.</exception>
    internal static IServiceCollection AddDbConnection(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        var connectionString = configurationManager.GetConnectionString("DefaultConnection") 
                               ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        var connectionStringPasswordSecret = configurationManager["Secrets:DefaultConnectionPassword"];
        connectionString = connectionString.Replace("passwordvalue", connectionStringPasswordSecret);
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Transient);
        services.AddDatabaseDeveloperPageExceptionFilter();

        return services;
    }
    
    /// <summary>
    /// Configure jwt authentication
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="configurationManager">Mutable configuration object</param>
    /// <returns>Collection of service descriptors</returns>
    /// <exception cref="InvalidOperationException">Throws if configuration path with bearer key not found.</exception>
    internal static IServiceCollection AddJwtAuth(this IServiceCollection services, ConfigurationManager configurationManager)
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
    /// Registers handlers and mediator types from assembly with Program.cs
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <returns>Collection of service descriptors</returns>
    internal static IServiceCollection AddMediatrAutoRegister(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

        return services;
    }    
    
    /// <summary>
    /// Registers handlers and mediator types specified manually 
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <returns>Collection of service descriptors</returns>
    internal static IServiceCollection AddMediatrManualRegister(this IServiceCollection services)
    {
        services
            .AddTransient(typeof(IRequestHandler<GetAllBaseEntity.Query<Category>, IList<Category>>), typeof(GetAllBaseEntity.QueryHandler<Category>))
            .AddTransient(typeof(IRequestHandler<GetAllBaseEntity.Query<Position>, IList<Position>>), typeof(GetAllBaseEntity.QueryHandler<Position>))
            
            .AddTransient(typeof(IRequestHandler<GetById.Query<Position>, Position>), typeof(GetById.QueryHandler<Position>))   
            .AddTransient(typeof(IRequestHandler<GetById.Query<Category>, Category>), typeof(GetById.QueryHandler<Category>))    
            .AddTransient(typeof(IRequestHandler<GetById.Query<Department>, Department>), typeof(GetById.QueryHandler<Department>))   
            .AddTransient(typeof(IRequestHandler<GetById.Query<Sector>, Sector>), typeof(GetById.QueryHandler<Sector>));

        return services;
    }
}