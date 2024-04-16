using System.Text.Json;

namespace Standards.Infrastructure.Exceptions
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                //switch (exception)
                //{
                //    case StandardsException:
                //        response.StatusCode = (int)HttpStatusCode.BadRequest;
                //        _logger.LogError(exception.Message);
                //        break;

                //    case KeyNotFoundException:
                //        response.StatusCode = (int)HttpStatusCode.NotFound;
                //        _logger.LogError(exception.Message);
                //        break;

                //    default:
                //        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //        _logger.LogError(exception.Message);
                //        break;
                //}
                _logger.LogError(exception.Message);

                var result = JsonSerializer.Serialize(new { message = exception.Message });

                await response.WriteAsync(result);
            }
        }
    }
}