using MediatR;

namespace Standards.Infrastructure.Mediatr
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestType = typeof(TRequest);

            _logger.LogInformation($"Handling {requestType.Name}");

            foreach (var requestProperty in requestType.GetProperties())
            {
                var propertyValue = requestProperty.GetValue(request, null);

                _logger.LogInformation($"{requestProperty.Name} : { propertyValue }");
            }

            var response = await next();

            var responseType = typeof(TRequest);

            _logger.LogInformation($"Handled { responseType.Name }");

            return response;
        }
    }
}
