using Application;
using Infrastructure;
using Infrastructure.Exceptions;
using Infrastructure.Vault;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using WebApi.Infrastructure.Exceptions;
using WebApi.Infrastructure.Extensions;
using WebApi.Infrastructure.Logging;
using WebApi.Infrastructure.Middlewares;

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
                
            builder.Services.AddSwaggerGen(swaggerOptions =>
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

            builder.Services
                .AddApplication()
                .AddPresentation()
                .AddInfrastructure(builder.Configuration);

            // NLog: Setup NLog for Dependency injection
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            builder.UseTelemetry();
            
            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.Add<SecretsConfigurationSource>(source =>
                {
                    var identityUrl = builder.Configuration["Vault:IdentityUrl"];
                    if (identityUrl is null) throw new InvalidOperationException("Identity URL is not set.");
                    source.IdentityUrl = identityUrl;
                    var apiUrl = builder.Configuration["Vault:ApiUrl"];
                    if (apiUrl is null) throw new InvalidOperationException("API URL is not set.");
                    source.ApiUrl = apiUrl;
                    
                    var vaultAccessToken = builder.Configuration["Vault:AccessToken"];
                    if (vaultAccessToken is null) throw new InvalidOperationException("Vault access token is not set.");
                    source.AccessToken = vaultAccessToken;
                    var vaultOrganizationId = builder.Configuration["Vault:OrganizationId"];
                    if (vaultOrganizationId is null) throw new InvalidOperationException("Vault organization id is not set.");
                    source.OrganizationId = vaultOrganizationId;
                });
            }
            else
            {
                builder.Configuration.Add<SecretsConfigurationSource>(source =>
                {
                    var identityUrl = builder.Configuration["Vault:IdentityUrl"] 
                                      ?? (Environment.GetEnvironmentVariable("Vault__IdentityUrl")
                                          ?? throw new InvalidOperationException("Identity URL is not set."));
                    source.IdentityUrl = identityUrl;
                    
                    var apiUrl = builder.Configuration["Vault:ApiUrl"]
                                 ?? (Environment.GetEnvironmentVariable("Vault__ApiUrl")
                                     ?? throw new InvalidOperationException("API URL is not set."));
                    source.ApiUrl = apiUrl;
                    
                    var vaultAccessToken = builder.Configuration["Vault:AccessToken"]
                                           ?? (Environment.GetEnvironmentVariable("Vault__AccessToken")
                                               ?? throw new InvalidOperationException("Vault access token is not set."));
                    source.AccessToken = vaultAccessToken;
                    
                    var vaultOrganizationId = builder.Configuration["Vault:OrganizationId"]
                                              ?? (Environment.GetEnvironmentVariable("Vault__OrganizationId")
                                                  ?? throw new InvalidOperationException("Vault organization id is not set."));
                    source.OrganizationId = vaultOrganizationId;
                });
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

            app.UseMiddleware<TokenRenewalMiddleware>();
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