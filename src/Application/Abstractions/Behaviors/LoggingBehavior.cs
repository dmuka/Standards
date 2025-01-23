using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Abstractions.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestType = typeof(TRequest);

        logger.LogInformation("Handling {requestTypeName}", requestType.Name);

        foreach (var requestProperty in requestType.GetProperties())
        {
            var propertyValue = requestProperty.GetValue(request, null);

            logger.LogInformation("{requestPropertyName} : { propertyValue }", requestProperty.Name, propertyValue);
        }

        var response = await next();

        var responseType = typeof(TRequest);

        logger.LogInformation("Handled { responseTypeName }", responseType.Name);

        return response;
    }
}