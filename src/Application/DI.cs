using Application.Abstractions.Behaviors;
using Application.CQRS.Common.GenericCRUD;
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

        services.AddValidatorsFromAssembly(typeof(DI).Assembly, includeInternalTypes: true);

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