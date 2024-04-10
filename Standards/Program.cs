using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using Standards.Data;
using Standards.Data.Repositories.Implementations;
using Standards.Data.Repositories.Interfaces;
using Standards.Services.Implementations;
using Standards.Services.Interfaces;
using System.Text;

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
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
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
                    ValidateAudience = false
                };
            });

            builder.Services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddScoped<IRepository, Repository<ApplicationDbContext>>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddControllers();
            builder.Services.AddRazorPages();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }
    }
}