using System.Text;

namespace Standards.Infrastructure.Logging
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await LogRequest(context);

            await _next(context);
        }

        private async Task LogRequest(HttpContext context)
        {

            context.Request.EnableBuffering();
            var request = context.Request;
            var response = context.Response;
            var requestBody = await GetRequestBody(request);
            context.Request.Body.Position = 0;

            var logMessage = $"Response status code: {response.StatusCode}, Request Method: {request.Method}, Request Path: {request.Path}, Request Body: {requestBody}";

            _logger.LogInformation($"{logMessage}, {GetHeaders(request.Headers)}");
        }

        private static async Task<string> GetRequestBody(HttpRequest request)
        {
            using (var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                return await reader.ReadToEndAsync();
            }
        }

        private static string GetHeaders(IHeaderDictionary headers)
        {
            var headersText = new StringBuilder();

            foreach (var header in headers)
            {
                headersText.AppendLine($"Request Header - {header.Key}: {header.Value}");
            }

            return headersText.ToString();
        }

}
}