using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Standards.Models.Users;
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
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] User requestor)
        {
            var user = _authService.Authenticate(requestor.Username, requestor.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

    }
}
