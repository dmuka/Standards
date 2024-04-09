using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Standards.Models.DTOs;
using Standards.Services.Interfaces;

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

                // Refresh token is valid, issue new access token and possibly new refresh token
                var accessToken = _authService.GenerateAccessToken(userId);
                var newRefreshToken = _authService.GenerateRefreshToken(userId);

                // You may also want to update the user's refresh token in your database or storage

                return Ok(new { AccessToken = accessToken, RefreshToken = newRefreshToken });
            }
            else
            {
                return Unauthorized("Invalid refresh token");
            }
        }
    }
}
