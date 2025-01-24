using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace WebApi.Infrastructure.Otel;

/// <summary>
/// Custom type to hold references for ActivitySource and Instruments.
/// This avoids possible type collisions with other components in the DI container.
/// </summary>
public class Instrumentation : IDisposable
{
    private readonly ActivitySource _activitySource;
    private readonly Meter _defaultMeter;
    private readonly Dictionary<string, object> _instruments = new();

    public Instrumentation(string activitySourceName)
    {
        var version = typeof(Instrumentation).Assembly.GetName().Version?.ToString();
        _defaultMeter = new Meter(activitySourceName, version);
        _activitySource = new ActivitySource(activitySourceName, version);
    }

    public Activity? StartActivity(string activityName, Dictionary<string, object>? tags = null)
    {
        var activity = _activitySource.StartActivity(activityName);
        
        if (activity is null || tags is null) return activity;
        
        foreach (var tag in tags)
        {
            activity.SetTag(tag.Key, tag.Value);
        }

        return activity;
    }

    public Counter<long> CreateCounter(string name, string? description = null)
    {
        var counter = _defaultMeter.CreateCounter<long>(name, description: description);
        _instruments[name] = counter;
        return counter;
    }

    public Histogram<double> CreateHistogram(string name, string? description = null)
    {
        var histogram = _defaultMeter.CreateHistogram<double>(name, description: description);
        _instruments[name] = histogram;
        return histogram;
    }

    public T GetInstrument<T>(string name) where T : class
    {
        if (_instruments.TryGetValue(name, out var instrument) && instrument is T typedInstrument)
        {
            return typedInstrument;
        }
        
        throw new KeyNotFoundException($"No instrument found with the name '{name}'.");
    }

    public void Dispose()
    {
        _activitySource.Dispose();
        GC.SuppressFinalize(this);
    }
}