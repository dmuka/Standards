using System.IdentityModel.Tokens.Jwt;
using Infrastructure.Options.Authentication;
using Microsoft.Extensions.Options;

namespace WebApi.Infrastructure;

public interface ITokenProvider
{
    bool IsAccessTokenExpired(string accessToken);
    Task<RefreshTokenResponse> RefreshAccessTokenAsync(string sessionId);
    Task UpdateTokensAsync(RefreshTokenResponse response);
}

public class TokenProvider(
    IHttpClientFactory httpClientFactory,
    IHttpContextAccessor context, 
    IConfiguration configuration,
    IOptions<AuthOptions> authOptions) : ITokenProvider
{
    public bool IsAccessTokenExpired(string accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
            return true;

        try
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var token = jwtHandler.ReadJwtToken(accessToken);
            return token.ValidTo < DateTime.UtcNow;
        }
        catch
        {
            return true;
        }
    }

    public async Task<RefreshTokenResponse> RefreshAccessTokenAsync(string sessionId)
    {
        var request = new RefreshTokenRequest { SessionId = sessionId };

        using var client = httpClientFactory.CreateClient();
        var response = await client.PostAsJsonAsync(
            $"{configuration["UsersService:BaseUrl"]}/api/users/signinbysessionid",
            request);

        if (!response.IsSuccessStatusCode) { throw new HttpRequestException($"Failed to refresh token: {response.ReasonPhrase}"); }

        var refreshResponse = await response.Content.ReadFromJsonAsync<RefreshTokenResponse>();
        if (refreshResponse == null) { throw new InvalidOperationException("Invalid refresh token response."); }

        return refreshResponse;
    }

    public async Task UpdateTokensAsync(RefreshTokenResponse response)
    {                
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddHours(authOptions.Value.SessionIdCookieExpirationInHours)
        };

        if (context.HttpContext is not null)
        {
            context.HttpContext.Response.Cookies.Append(CookiesNames.SessionId, response.SessionId, cookieOptions);

            context.HttpContext.Request.Headers.Authorization = $"Bearer {response.AccessToken}";
        }

        await Task.CompletedTask;
    }
}

public class RefreshTokenRequest
{
    public string SessionId { get; set; } = string.Empty;
}

public class RefreshTokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string SessionId { get; set; } = string.Empty;
}