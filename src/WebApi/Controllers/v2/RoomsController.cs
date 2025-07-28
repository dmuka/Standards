using Application.Abstractions.Data.Filter.Models;
using Application.UseCases.Common;
using Application.UseCases.DTOs;
using Application.UseCases.Rooms;
using Domain.Aggregates.Rooms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v2;

[ApiController]
[Route("api/v2/RoomsController")]
public class RoomsController(ISender sender) : ApiBaseController
{
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetRooms()
    {
        var query = new GetAllRooms.Query();

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetRoom(Guid id)
    {
        var query = new GetRoomById.Query(new RoomId(id));

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> AddRoom([FromBody] RoomDto2 person)
    {
        var query = new AddRoom.Command(person);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> EditRoom([FromBody]RoomDto2 person)
    {
        var query = new EditRoom.Command(person);
            
        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpDelete]
    [Route("delete/{id:Guid}")]
    public async Task<IActionResult> DeleteRoom(Guid id)
    {
        var query = new DeleteRoom.Command(new RoomId(id));

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