using System.Net;
using System.Security;
using Infrastructure.Exceptions.Enum;
using Infrastructure.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Exceptions
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                if (exception is SecurityException)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    logger.LogInformation(exception, "Unauthorized request: {exceptionMessage}", exception.Message);
                    return;
                }

                if (exception is StandardsException applicationException)
                {
                    context.Response.StatusCode = (int)GetStatusCode(applicationException.Error);
                    logger.LogInformation(exception, "Application error: {exceptionMessage}", exception.Message);
                    await context.Response.WriteAsync(applicationException.MessageForUser);
                    return;
                }

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                logger.LogWarning(exception, "Internal server error: {exceptionMessage}", exception.Message);
                await context.Response.WriteAsync("Internal server error.");

                if (exception is not TaskCanceledException
                    && exception is not OperationCanceledException)
                {
                    var userId = string.Empty;

                    var unhandledlogger = NLog.LogManager.GetLogger("UnhandledErrorsLog");

                    var logEntry = new LogEntry
                    {
                        TimeStamp = DateTime.UtcNow,
                        UserName = string.IsNullOrEmpty(userId) ? "unknown" : $"UserId:{userId}",
                        RequestUrl = context.Request.Path,
                        HttpMethod = context.Request.Method,
                    };
                    
                    if (exception is StandardsException appException)
                    {
                        if (!appException.IsHandled)
                        {
                            logEntry.Status = appException.Error.ToString();
                            logEntry.ErrorMessage = appException.MessageForLog;
                        };
                        
                        unhandledlogger.Error(logEntry.ToString);
                    }
                    else
                    {
                        logEntry.Status = "500: Internal error UnhandledError";
                        logEntry.ErrorMessage = $"{exception.Message}; exception type: {exception.GetType().FullName}";
                    }
                    
                    unhandledlogger.Error(logEntry.ToString);
                }
            }
        }

        private static HttpStatusCode GetStatusCode(StatusCodeByError error)
        {
            return error switch
            {
                StatusCodeByError.NotFound => HttpStatusCode.NotFound,
                StatusCodeByError.Forbidden => HttpStatusCode.Forbidden,
                StatusCodeByError.BadRequest => HttpStatusCode.BadRequest,
                StatusCodeByError.Unauthorized => HttpStatusCode.Unauthorized,
                StatusCodeByError.Conflict => HttpStatusCode.Conflict,
                StatusCodeByError.InternalServerError => HttpStatusCode.InternalServerError,
                StatusCodeByError.ServiceUnavailable => HttpStatusCode.ServiceUnavailable,
                StatusCodeByError.BadGateway => HttpStatusCode.BadGateway,
                _ => HttpStatusCode.InternalServerError,
            };
        }
    }
}
