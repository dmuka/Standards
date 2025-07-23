using System.Reflection;
using Infrastructure.Vault;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using WebApi.Infrastructure.Otel;

namespace WebApi.Infrastructure.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static IHostApplicationBuilder UseTelemetry(this IHostApplicationBuilder builder)
    {
        // Note: Switch between OTLP/Console by setting UseTracingExporter in appsettings.json.
        var tracingExporter = builder.Configuration.GetValue("UseTracingExporter", defaultValue: "console")!.ToLowerInvariant();

        // Note: Switch between Prometheus/OTLP/Console by setting UseMetricsExporter in appsettings.json.
        var metricsExporter = builder.Configuration.GetValue("UseMetricsExporter", defaultValue: "console")!.ToLowerInvariant();

        // Note: Switch between Console/OTLP by setting UseLogExporter in appsettings.json.
        var logExporter = builder.Configuration.GetValue("UseLogExporter", defaultValue: "console")!.ToLowerInvariant();

        // Note: Switch between Explicit/Exponential by setting HistogramAggregation in appsettings.json
        var histogramAggregation = builder.Configuration.GetValue("HistogramAggregation", defaultValue: "explicit")!.ToLowerInvariant();
        
        var serviceName = Assembly.GetEntryAssembly()?.GetName().Name ?? "Standards";

        // Create a service to expose ActivitySource, and Metric Instruments for manual instrumentation
        builder.Services.AddSingleton(new Instrumentation(serviceName));

        builder.Logging.AddOpenTelemetry(options =>
        {
            options.SetResourceBuilder(ResourceBuilder.CreateDefault()
                    .AddService(serviceName))
                .AddConsoleExporter()
                .AddOtlpExporter();
        });
            
        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName))
            .WithTracing(tracing => tracing
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddConsoleExporter()
                .AddOtlpExporter())
            .WithMetrics(metrics => metrics
                .AddRuntimeInstrumentation()
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddConsoleExporter()
                .AddOtlpExporter());

        return builder;
    }
    
    public static IHostApplicationBuilder UseDevelopmentSecrets(this IHostApplicationBuilder builder)
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

        return builder;
    }
    
    public static IHostApplicationBuilder UseProductionSecrets(this IHostApplicationBuilder builder)
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

        return builder;
    }
}