using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [Route("")]
        public IActionResult GetRoom(int id = 0)
        {
            if (id == 0) return BadRequest();

            var room = repository.Rooms.FirstOrDefault(r => r.Id == id);

            if (room is null)
            {
                return NotFound();
            }

            return Ok(room);
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
        [Route("delete")]
        public void DeleteRoom(int id)
        {
            var room = repository.Rooms.Find(id);

            if (room is null) return;
            
            repository.Rooms.Remove(room);

            repository.SaveChanges();
        }
    }
}
