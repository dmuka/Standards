using Application.Abstractions.Behaviors;
using Application.EventConsumers;
using Application.UseCases.Common.GenericCRUD;
using Core;
using Domain.Models;
using Domain.Models.Departments;
using Domain.Models.MetrologyControl;
using Domain.Models.Persons;
using Domain.Models.Services;
using Domain.Models.Standards;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DI
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(DI).Assembly);

            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            config.AddOpenBehavior(typeof(TransactionBehavior<,>));
        });
        
        services
            .AddTransient<IRequestHandler<GetAllBaseEntity.Query<Category>, Result<List<Category>>>,
                GetAllBaseEntity.QueryHandler<Category>>();
        
        services
            .AddTransient<IRequestHandler<GetAllBaseEntity.Query<Category>, Result<List<Category>>>, GetAllBaseEntity.QueryHandler<Category>>()
            .AddTransient<IRequestHandler<GetAllBaseEntity.Query<ServiceType>, Result<List<ServiceType>>>, GetAllBaseEntity.QueryHandler<ServiceType>>()
            .AddTransient<IRequestHandler<CreateBaseEntity.Query<ServiceType>, int>, CreateBaseEntity.QueryHandler<ServiceType>>()
            .AddTransient<IRequestHandler<GetAllBaseEntity.Query<Position>, Result<List<Position>>>, GetAllBaseEntity.QueryHandler<Position>>()
            .AddTransient<IRequestHandler<CreateBaseEntity.Query<Position>, int>, CreateBaseEntity.QueryHandler<Position>>()
            .AddTransient<IRequestHandler<GetAllBaseEntity.Query<Characteristic>, Result<List<Characteristic>>>, GetAllBaseEntity.QueryHandler<Characteristic>>()
            .AddTransient<IRequestHandler<CreateBaseEntity.Query<Characteristic>, int>, CreateBaseEntity.QueryHandler<Characteristic>>()
            .AddTransient<IRequestHandler<GetAllBaseEntity.Query<Grade>, Result<List<Grade>>>, GetAllBaseEntity.QueryHandler<Grade>>()
            .AddTransient<IRequestHandler<CreateBaseEntity.Query<Grade>, int>, CreateBaseEntity.QueryHandler<Grade>>()
            .AddTransient<IRequestHandler<GetAllBaseEntity.Query<Material>, Result<List<Material>>>, GetAllBaseEntity.QueryHandler<Material>>()
            .AddTransient<IRequestHandler<CreateBaseEntity.Query<Material>, int>, CreateBaseEntity.QueryHandler<Material>>()
            .AddTransient<IRequestHandler<GetAllBaseEntity.Query<Quantity>, Result<List<Quantity>>>, GetAllBaseEntity.QueryHandler<Quantity>>()
            .AddTransient<IRequestHandler<CreateBaseEntity.Query<Quantity>, int>, CreateBaseEntity.QueryHandler<Quantity>>()
            
            .AddTransient<IRequestHandler<GetById.Query<Position>, Position?>, GetById.QueryHandler<Position>>() 
            .AddTransient<IRequestHandler<GetById.Query<Characteristic>, Characteristic?>, GetById.QueryHandler<Characteristic>>()   
            .AddTransient<IRequestHandler<GetById.Query<Grade>, Grade?>, GetById.QueryHandler<Grade>>()  
            .AddTransient<IRequestHandler<GetById.Query<Category>, Category?>, GetById.QueryHandler<Category>>()    
            .AddTransient<IRequestHandler<GetById.Query<Department>, Department?>, GetById.QueryHandler<Department>>()   
            .AddTransient<IRequestHandler<GetById.Query<Sector>, Sector?>, GetById.QueryHandler<Sector>>() 
            .AddTransient<IRequestHandler<GetById.Query<Material>, Material?>, GetById.QueryHandler<Material>>()
            .AddTransient<IRequestHandler<GetById.Query<Quantity>, Quantity?>, GetById.QueryHandler<Quantity>>()
            .AddTransient<IRequestHandler<GetById.Query<Place>, Place?>, GetById.QueryHandler<Place>>();

        services.AddValidatorsFromAssembly(typeof(DI).Assembly, includeInternalTypes: true);

        services.AddKafkaConsumerHostedService();
        
        return services;
    }
    

    /// <summary>
    /// Registers query builder and wrapper types
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <returns>Collection of service descriptors</returns>
    private static IServiceCollection AddKafkaConsumerHostedService(this IServiceCollection services)
    {
        services.AddHostedService<UserRegisteredEventConsumer>();

        return services;
    }
    
    // /// <summary>
    // /// Adds application services
    // /// </summary>
    // /// <param name="services">Collection of service descriptors</param>
    // /// <returns>Collection of service descriptors</returns>
    // internal static IServiceCollection AddAppServices(this IServiceCollection services)
    // {
    //     services
    //         .AddTransient<IRepository, Repository<ApplicationDbContext>>()
    //         .AddSingleton<IConfigService, ConfigService>()
    //         .AddScoped<IUserService, UserService>()
    //         .AddScoped<IAuthService, AuthService>();
    //
    //     return services;
    // }    
}