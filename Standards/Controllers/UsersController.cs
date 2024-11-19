using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Users;
using Standards.Core.Services.Interfaces;

namespace Standards.Controllers;

//[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    //[Authorize(Roles = Role.Admin)]
    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> Create(UserDto userDto)
    {
        var user = await userService.Create(userDto);

        return Ok(user);
    }

    //[Authorize(Roles = Role.Admin)]
    [HttpGet]
    [Route("list")]
    public IActionResult GetAll()
    {
        var users = userService.GetAll();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var user = userService.GetById(id);

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