using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using Standards.Core.Models.DTOs;
using Standards.Core.Services.Implementations;
using Standards.Core.Services.Interfaces;
using Standards.Infrastructure.Data;
using Standards.Infrastructure.Data.Repositories.Implementations;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Exceptions;
using Standards.Infrastructure.Filter.Implementations;
using Standards.Infrastructure.Filter.Interfaces;
using Standards.Infrastructure.Logging;
using Standards.Infrastructure.Mediatr;
using Standards.Infrastructure.Mediatr.Standards.Core.CQRS.Common.Behaviors;
using System.Text;
using Standards.Core.Models.Housings;
using Standards.Infrastructure.Converters;
using Standards.Infrastructure.Filter.Models;
using Standards.Infrastructure.QueryableWrapper.Implementation;
using Standards.Infrastructure.QueryableWrapper.Interface;
using Standards.Infrastructure.Services.Implementations;
using Standards.Infrastructure.Services.Interfaces;

namespace Standards
{
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

                ConfigureServices(builder);

                // NLog: Setup NLog for Dependency injection
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

                var app = builder.Build();

                if (app.Environment.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    app.UseMigrationsEndPoint();
                    app.UseSwagger();
                    app.UseSwaggerUI();
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

                //app.MapRazorPages();

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

        private static void ConfigureServices(WebApplicationBuilder? builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            var connectionStringPasswordSecret = builder.Configuration["Secrets:DefaultConnectionPassword"];
            connectionString = connectionString.Replace("passwordvalue", connectionStringPasswordSecret);

            // configure jwt authentication
            var jwtBearerSecret = builder.Configuration["Secrets:JwtBearerKey"];
            var key = Encoding.ASCII.GetBytes(jwtBearerSecret);
            builder.Services.AddAuthentication(authOptions =>
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

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Transient);
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

            builder.Services.AddValidatorsFromAssemblyContaining<Program>();
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

            builder.Services.AddTransient<IRepository, Repository<ApplicationDbContext>>();

            builder.Services.AddScoped<IConfigService, ConfigService>();
            
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            
            builder.Services.AddMemoryCache();
            builder.Services.AddScoped<ICacheService, CacheService>();
            
            builder.Services.AddScoped<IQueryBuilder<HousingDto>, QueryBuilder<HousingDto>>();
            builder.Services.AddScoped<IQueryBuilder<Housing>, QueryBuilder<Housing>>();
            builder.Services.AddScoped<IQueryBuilder<Room>, QueryBuilder<Room>>();
            
            builder.Services.AddScoped<IQueryableWrapper<Housing>, QueryableWrapper<Housing>>();
            builder.Services.AddScoped<IQueryableWrapper<Room>, QueryableWrapper<Room>>();
            
            builder.Services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.Converters.Add(new NullableEnumConverter<FilterBy>());
                    opt.JsonSerializerOptions.Converters.Add(new NullableEnumConverter<SortBy>());
                });
            builder.Services.AddRazorPages();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }
    }
}