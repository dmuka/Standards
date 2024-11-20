using System.Text;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Standards.Core.CQRS.Common.GenericCRUD;
using Standards.Core.Models;
using Standards.Core.Models.Departments;
using Standards.Core.Models.Housings;
using Standards.Core.Models.MetrologyControl;
using Standards.Core.Models.Persons;
using Standards.Core.Models.Services;
using Standards.Core.Models.Standards;
using Standards.Core.Services.Implementations;
using Standards.Core.Services.Interfaces;
using Standards.Infrastructure.Converters;
using Standards.Infrastructure.Data;
using Standards.Infrastructure.Data.Repositories.Implementations;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Filter.Implementations;
using Standards.Infrastructure.Filter.Interfaces;
using Standards.Infrastructure.Filter.Models;
using Standards.Infrastructure.Mediatr;
using Standards.Infrastructure.Mediatr.Standards.Core.CQRS.Common.Behaviors;
using Standards.Infrastructure.QueryableWrapper.Interface;
using Standards.Infrastructure.QueryableWrapper.Implementation;
using Standards.Infrastructure.Services.Implementations;
using Standards.Infrastructure.Services.Interfaces;

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
            .AddTransient(typeof(IRequestHandler<CreateBaseEntity.Query<Category>, int>), typeof(CreateBaseEntity.QueryHandler<Category>))
            .AddTransient(typeof(IRequestHandler<GetAllBaseEntity.Query<Position>, IList<Position>>), typeof(GetAllBaseEntity.QueryHandler<Position>))
            .AddTransient(typeof(IRequestHandler<CreateBaseEntity.Query<Position>, int>), typeof(CreateBaseEntity.QueryHandler<Position>))
            .AddTransient(typeof(IRequestHandler<GetAllBaseEntity.Query<Characteristic>, IList<Characteristic>>), typeof(GetAllBaseEntity.QueryHandler<Characteristic>))
            .AddTransient(typeof(IRequestHandler<CreateBaseEntity.Query<Characteristic>, int>), typeof(CreateBaseEntity.QueryHandler<Characteristic>))
            .AddTransient(typeof(IRequestHandler<GetAllBaseEntity.Query<Grade>, IList<Grade>>), typeof(GetAllBaseEntity.QueryHandler<Grade>))
            .AddTransient(typeof(IRequestHandler<CreateBaseEntity.Query<Grade>, int>), typeof(CreateBaseEntity.QueryHandler<Grade>))
            .AddTransient(typeof(IRequestHandler<GetAllBaseEntity.Query<Material>, IList<Material>>), typeof(GetAllBaseEntity.QueryHandler<Material>))
            .AddTransient(typeof(IRequestHandler<CreateBaseEntity.Query<Material>, int>), typeof(CreateBaseEntity.QueryHandler<Material>))
            .AddTransient(typeof(IRequestHandler<GetAllBaseEntity.Query<Place>, IList<Place>>), typeof(GetAllBaseEntity.QueryHandler<Place>))
            .AddTransient(typeof(IRequestHandler<CreateBaseEntity.Query<Place>, int>), typeof(CreateBaseEntity.QueryHandler<Place>))
            .AddTransient(typeof(IRequestHandler<GetAllBaseEntity.Query<Quantity>, IList<Quantity>>), typeof(GetAllBaseEntity.QueryHandler<Quantity>))
            .AddTransient(typeof(IRequestHandler<CreateBaseEntity.Query<Quantity>, int>), typeof(CreateBaseEntity.QueryHandler<Quantity>))
            
            .AddTransient(typeof(IRequestHandler<GetById.Query<Position>, Position>), typeof(GetById.QueryHandler<Position>)) 
            .AddTransient(typeof(IRequestHandler<GetById.Query<Characteristic>, Characteristic>), typeof(GetById.QueryHandler<Characteristic>))   
            .AddTransient(typeof(IRequestHandler<GetById.Query<Grade>, Grade>), typeof(GetById.QueryHandler<Grade>))  
            .AddTransient(typeof(IRequestHandler<GetById.Query<Category>, Category>), typeof(GetById.QueryHandler<Category>))    
            .AddTransient(typeof(IRequestHandler<GetById.Query<Department>, Department>), typeof(GetById.QueryHandler<Department>))   
            .AddTransient(typeof(IRequestHandler<GetById.Query<Sector>, Sector>), typeof(GetById.QueryHandler<Sector>)) 
            .AddTransient(typeof(IRequestHandler<GetById.Query<Material>, Material>), typeof(GetById.QueryHandler<Material>)) 
            .AddTransient(typeof(IRequestHandler<GetById.Query<Quantity>, Quantity>), typeof(GetById.QueryHandler<Quantity>));

        return services;
    }       
    
    /// <summary>
    /// Adds mediatr pipeline behaviors
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <returns>Collection of service descriptors</returns>
    internal static IServiceCollection AddMediatrPipelineBehaviors(this IServiceCollection services)
    {
        services
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

        return services;
    }    
    
    /// <summary>
    /// Adds validators from assembly of the type specified by generic parameter
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <typeparam name="T">Type of the assembly</typeparam>
    /// <returns>Collection of service descriptors</returns>
    internal static IServiceCollection AddValidators<T>(this IServiceCollection services)
    {           
        services.AddValidatorsFromAssemblyContaining<T>();

        return services;
    }    
    
    /// <summary>
    /// Adds application services
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <returns>Collection of service descriptors</returns>
    internal static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services
            .AddTransient<IRepository, Repository<ApplicationDbContext>>()
            .AddSingleton<IConfigService, ConfigService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IAuthService, AuthService>();

        return services;
    }    
    
    /// <summary>
    /// Adds cache
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <returns>Collection of service descriptors</returns>
    internal static IServiceCollection AddCache(this IServiceCollection services)
    {
        services
            .AddMemoryCache()
            .AddSingleton<ICacheService, CacheService>();

        return services;
    }    
    
    /// <summary>
    /// Registers query builder and wrapper types
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <returns>Collection of service descriptors</returns>
    internal static IServiceCollection RegisterQueryBuilder(this IServiceCollection services)
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