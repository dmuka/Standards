using System.Net;
using System.Text.Json;
using FluentValidation;
using Infrastructure.Exceptions;

namespace WebApi.Infrastructure.Exceptions;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (ValidationException validationException)
        {
            await HandleException(context, validationException.Message, (int)HttpStatusCode.BadRequest);
        }
        catch (StandardsException standardsException)
        {
            await HandleException(context, standardsException.Message, (int)standardsException.Error);
        }
        catch (Exception exception)
        {
            await HandleException(context, exception.Message, (int)HttpStatusCode.InternalServerError);
        }
    }

    private async Task HandleException(
        HttpContext context, 
        string exceptionMessage, 
        int statusCode)
    {
        var response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = statusCode;

        _logger.LogError("Status code: {statusCode} - exception message: {exceptionMessage}", statusCode, exceptionMessage);
            
        var result = JsonSerializer.Serialize(new { message = exceptionMessage });

        await response.WriteAsync(result);
    }
}