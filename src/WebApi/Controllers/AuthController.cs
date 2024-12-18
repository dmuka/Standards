using Application.Abstractions.Authentication;
using Application.Abstractions.Authentication.Enums;
using Domain.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ApiBaseController
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] UserDto userDto)
        {
            var user = await authService.Authenticate(userDto.UserName, userDto.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [HttpPost("/token/refresh")]
        public IActionResult RefreshToken(string refreshToken)
        {
            var principal = authService.ValidateToken(refreshToken);

            if (principal != null)
            {
                var userId = int.Parse(principal.Claims.FirstOrDefault(x => x.Type.Equals("sid", StringComparison.OrdinalIgnoreCase))?.Value);

                var accessToken = authService.GenerateToken(userId, TokenType.Access);
                var newRefreshToken = authService.GenerateToken(userId, TokenType.Refresh);

                return Ok(new { AccessToken = accessToken, RefreshToken = newRefreshToken });
            }
            else
            {
                return Unauthorized("Invalid refresh token");
            }
        }

        [AllowAnonymous]
        [HttpGet("/token/validate")]
        public IActionResult ValidateToken()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var principal = authService.ValidateToken(token);

            if (principal != null)
            {
                // Token is valid
                return Ok(new { message = "Token is valid" });
            }
            else
            {
                // Token is invalid
                return Unauthorized(new { message = "Invalid token" });
            }
        }
    }
}
