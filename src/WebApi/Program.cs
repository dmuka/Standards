using Application;
using Infrastructure;
using Infrastructure.Logging;
using NLog;
using NLog.Web;
using WebApi.Infrastructure.Exceptions;
using WebApi.Infrastructure.Logging;
using WebApi.Infrastructure.StartupExtensions;

namespace WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        // Early init of NLog to allow startup and exception logging, before host is built
        var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
        logger.Debug("init main");

        try
        {
            var builder = WebApplication.CreateBuilder(args);
                
            builder.Services.AddSwaggerGen();

            builder.Services
                .AddApplication()
                .AddPresentation()
                .AddInfrastructure(builder.Configuration);

            // NLog: Setup NLog for Dependency injection
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            var app = builder.Build();
            
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage()
                    .UseMigrationsEndPoint()
                    .UseSwaggerWithUi();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "/api/{controller}/{action=Index}/{id?}");

            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.MapFallbackToFile("index.html");

            app.Run();
        }
        catch (Exception exception)
        {
            // NLog: catch setup errors
            logger.Error(exception, "Stopped program because of exception");
            throw;
        }
        finally
        {
            // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
            LogManager.Shutdown();
        }
    }
}