using Application.Abstractions.Authentication;
using Domain.Models.DTOs;
using Domain.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

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
    public async Task<IActionResult> GetAll()
    {
        var users = await userService.GetAll();
        
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await userService.GetById(id);

        if (user is null) return NotFound();

        if (User.IsInRole(Role.Admin)) return Forbid();

        return Ok(user);
    }
}