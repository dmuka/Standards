﻿using Application;
using Infrastructure;
using Infrastructure.Middleware;
using Microsoft.Extensions.FileProviders;
using NLog;
using NLog.Web;
using Swashbuckle.AspNetCore.Swagger;
using WebApi.Infrastructure.Exceptions;
using WebApi.Infrastructure.Extensions;
using WebApi.Infrastructure.Logging;

namespace WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        // Early init of NLog to allow startup and exception logging before the host is built
        var logger = LogManager.Setup().LoadConfigurationFromFile().GetCurrentClassLogger();
        logger.Debug("init main");

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services
                .AddApplication()
                .AddPresentation()
                .AddInfrastructure(builder.Configuration);

            // NLog: Setup NLog for Dependency injection
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            //builder.UseTelemetry();
            
            if (builder.Environment.IsDevelopment())
            {
                builder.UseDevelopmentSecrets();
            }
            else
            {
                builder.UseProductionSecrets();
            }
            
            var app = builder.Build();
            
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage()
                    .UseMigrationsEndPoint()
                    .UseSwaggerWithUi();

                app.ApplyMigrations();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(
                new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
                    RequestPath = new PathString("/wwwroot")
                });

            app.UseRouting();

            //app.UseMiddleware<TokenRenewalMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "/api/{controller}/{action=Index}/{id?}");

            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();

            app.MapFallbackToFile("index.html");
            
            app.Run();
        }
        catch (Exception exception)
        {
            // NLog: catch setup errors
            logger.Error(exception, "Stopped program because of exception ({Source}, {Message}, {Trace}, {Inner})", exception.Source, exception.Message, exception.StackTrace, exception.HResult);
            throw;
        }
        finally
        {
            // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
            LogManager.Shutdown();
        }
    }
}