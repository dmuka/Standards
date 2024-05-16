using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Standards.Core.Models.DTOs;
using Standards.Core.Services.Interfaces;

namespace Standards.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ApiBaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) 
        {
            _authService = authService;
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] UserDto userDto)
        {
            var user = await _authService.Authenticate(userDto.UserName, userDto.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [HttpPost("/token/refresh")]
        public IActionResult RefreshToken(string refreshToken)
        {
            var principal = _authService.ValidateRefreshToken(refreshToken);

            if (principal != null)
            {
                var userId = int.Parse(principal.Claims.FirstOrDefault(x => x.Type.Equals("sid", StringComparison.OrdinalIgnoreCase))?.Value);

                var accessToken = _authService.GenerateAccessToken(userId);
                var newRefreshToken = _authService.GenerateRefreshToken(userId);

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

            var principal = _authService.ValidateAccessToken(token);

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
