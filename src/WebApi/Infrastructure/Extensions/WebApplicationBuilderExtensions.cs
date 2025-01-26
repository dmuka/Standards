using System.Reflection;
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
}