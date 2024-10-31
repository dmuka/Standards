using Microsoft.EntityFrameworkCore;
using Standards.Infrastructure.Data;

namespace Standards.Infrastructure.StartupExtensions;

public static class ServiceCollectionExtensions
{
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
}