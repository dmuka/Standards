namespace WebApi.Infrastructure.Middlewares;

public class TokenRenewalMiddleware(RequestDelegate next)
{

    public async Task InvokeAsync(
        HttpContext context, 
        ITokenProvider tokenProvider,
        IConfiguration configuration)
    {
        var sessionId = context.Request.Cookies[CookiesNames.SessionId];

        if (sessionId is null)
        {
            var returnUrl = Uri.EscapeDataString($"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}");
            context.Response.Redirect($"{configuration["UsersService:BaseUrl"]}/pages/users/signin?returnUrl={returnUrl}");
            
            return;
        }
        
        var accessToken = context.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

        if (string.IsNullOrEmpty(accessToken) || tokenProvider.IsAccessTokenExpired(accessToken))
        {
            try
            {
                if (string.IsNullOrEmpty(sessionId))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Session ID not found.");
                    
                    return;
                }

                var refreshResponse = await tokenProvider.RefreshAccessTokenAsync(sessionId);
                await tokenProvider.UpdateTokensAsync(refreshResponse);
                
                context.Request.Headers.Authorization = $"Bearer {refreshResponse.AccessToken}";
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync($"Token refresh failed: {ex.Message}");
                
                return;
            }
        }

        await next(context);
    }
}