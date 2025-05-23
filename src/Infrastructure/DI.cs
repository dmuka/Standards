using System.Text;
using Domain.Models.Departments;
using Domain.Models.Housings;
using Infrastructure.Data;
using Infrastructure.Data.Repositories.Implementations;
using Infrastructure.Data.Repositories.Interfaces;
using Infrastructure.Filter.Implementations;
using Infrastructure.Filter.Interfaces;
using Infrastructure.Options.Authentication;
using Infrastructure.QueryableWrapper.Implementation;
using Infrastructure.QueryableWrapper.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Fluent;

namespace Infrastructure;

public static class DI
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    
    /// <summary>
    /// Adds application services
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="configuration">Represents a set of key/value application configuration properties</param>
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
    /// <param name="configuration">Mutable configuration object</param>
    /// <returns>Collection of service descriptors</returns>
    /// <exception cref="InvalidOperationException">Throws if the configuration path with the bearer key not found.</exception>
    private static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<AuthOptions>()
            .BindConfiguration("Jwt")
            .ValidateDataAnnotations()
            .ValidateOnStart()
            .PostConfigure(options =>
            {
                var jwtSecret = configuration["Jwt__Secret"];
                if (string.IsNullOrEmpty(jwtSecret)) throw new InvalidOperationException("JWT secret value is not set in the configuration.");
                 
                options.Secret = jwtSecret;
            });
        
        services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwtBearerOptions =>
            {
                
                var jwtSecret = configuration["Jwt__Secret"];
                if (string.IsNullOrEmpty(jwtSecret)) throw new InvalidOperationException("JWT secret value is not set in the configuration.");
                
                jwtBearerOptions.RequireHttpsMetadata = false;
                jwtBearerOptions.SaveToken = true;
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero
                };
                jwtBearerOptions.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var user = context.Principal;
                        Logger.Info("Token validated for user: {UserName}", user?.Identity?.Name);
                         
                        if (user?.Claims is null) return Task.CompletedTask;
                         
                        foreach (var claim in user.Claims)
                        {
                            Logger.Info("Principal claim type: {ClaimType}, claim value: {ClaimValue}", claim.Type, claim.Value);
                        }

                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        var user = context.Principal;
                         
                        Logger.Error(context.Exception, "Authentication failed ({User})", user?.Identity is null ? "Unknown" : user.Identity.Name);

                        return Task.CompletedTask;
                    }
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