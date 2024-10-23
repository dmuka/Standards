using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Standards.Core.CQRS.Common.Constants;
using Standards.Core.CQRS.Common.GenericCRUD;
using Standards.Core.CQRS.Rooms;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Housings;
using Standards.Infrastructure.Data;

namespace Standards.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class RoomsController(ApplicationDbContext repository, ISender sender) : ControllerBase
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
            var query = new GetById.Query<Room>(id, Cache.Rooms);

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
        public void EditRoom(int id, [FromBody] Room room)
        {
            if (room.Id != id) return;
            
            repository.Entry(room).State = EntityState.Modified;

            repository.SaveChanges();
        }


        [HttpDelete]
        [Route("delete/{id:int}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var query = new Delete<Room>.Query(id);

            var result = await sender.Send(query);

            return Ok(result);
        }
    }
}
