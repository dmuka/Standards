using Application.UseCases.Common.GenericCRUD;
using Application.UseCases.DTOs;
using Application.UseCases.Rooms;
using Domain.Models.Housings;
using Infrastructure.Filter.Implementations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomsController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetRooms()
    {
        var query = new GetAll.Query();

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetRoom(int id)
    {
        var query = new GetById.Query<Room>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> CreateRoom([FromBody] RoomDto room)
    {
        var query = new Create.Query(room);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> EditRoom([FromBody] RoomDto room)
    {
        var query = new Edit.Query(room);
            
        var result = await sender.Send(query);

        return Ok(result);
    }


    [HttpDelete]
    [Route("delete/{id:int}")]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        var query = new Delete.Command<Room>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("filter")]
    public async Task<IActionResult> GetRoomsByFilter([FromBody] QueryParameters parameters)
    {
        var query = new GetFiltered<Room>.Query(parameters);

        var result = await sender.Send(query);

        return Ok(result);
    }
}