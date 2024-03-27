using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Standards.Models.DTOs;
using Standards.Models.Users;
using Standards.Services.Interfaces;

namespace Standards.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        //[Authorize(Roles = Role.Admin)]
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Create(UserDto userDto)
        {
            var user = await _userService.Create(userDto);

            return Ok(user);
        }

        //[Authorize(Roles = Role.Admin)]
        [HttpGet]
        [Route("list")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);

            if (user == null)
            {
                return NotFound();
            }

            if (User.IsInRole(Role.Admin))
            {
                return Forbid();
            }

            return Ok(user);
        }
    }
}