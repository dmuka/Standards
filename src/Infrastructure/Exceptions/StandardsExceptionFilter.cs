using System.Net;
using System.Security;
using System.Web.Http.Filters;
using Infrastructure.Exceptions.Enum;
using Infrastructure.Logging;

namespace Infrastructure.Exceptions
{
    public class StandardsExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is SecurityException)
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            if (actionExecutedContext.Exception is StandardsException applicationException)
            {
                actionExecutedContext.Response = new HttpResponseMessage()
                {
                    StatusCode = GetStatusCode(applicationException.Error),
                    Content = applicationException.MessageForUser != null
                        ? new StringContent(applicationException.MessageForUser)
                        : null
                };
            }

            actionExecutedContext.Response ??= new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent("Internal server error.")
                };

            if (actionExecutedContext.Exception is not TaskCanceledException
                && actionExecutedContext.Exception is not OperationCanceledException)
            {
                var userId = string.Empty;

                var logger = NLog.LogManager.GetLogger("UnhandledErrorsLog");

                if (actionExecutedContext.Exception is StandardsException appException)
                {
                    if (!appException.IsHandled)
                    {
                        var logEntry = new LogEntry
                        {
                            TimeStamp = DateTime.UtcNow,
                            UserName = string.IsNullOrEmpty(userId) ? null : $"UserId:{userId}",
                            RequestUrl = actionExecutedContext.Request.RequestUri.AbsolutePath,
                            HttpMethod = actionExecutedContext.Request.Method.Method,
                            Status = appException.Error.ToString(),
                            ErrorMessage = appException.MessageForLog,
                        };

                        logger.Error(logEntry.ToString);
                    }
                }
                else
                {
                    var logEntry = new LogEntry
                    {
                        TimeStamp = DateTime.UtcNow,
                        UserName = string.IsNullOrEmpty(userId) ? null : $"UserId:{userId}",
                        RequestUrl = actionExecutedContext.Request.RequestUri.AbsolutePath,
                        HttpMethod = actionExecutedContext.Request.Method.Method,
                        Status = "500: Internal error UnhandledError",
                        ErrorMessage = $"{actionExecutedContext.Exception.Message}; exception type: {actionExecutedContext.Exception.GetType().FullName}"
                    };

                    logger.Error(logEntry.ToString);
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
