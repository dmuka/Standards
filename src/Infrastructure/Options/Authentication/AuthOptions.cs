using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Options.Authentication;

public record AuthOptions
{
    public string Secret { get; set; } = string.Empty;
    [Required, MinLength(2)]
    public string Issuer { get; set; } = string.Empty;
    [Required, MinLength(4)]
    public string Audience { get; set; } = string.Empty;
    [Required, Range(0, 24)]
    public required int SessionIdCookieExpirationInHours { get; set; }
}