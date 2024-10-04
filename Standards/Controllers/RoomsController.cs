using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Standards.Core.Models.Housings;
using Standards.Infrastructure.Data;

namespace Standards.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class RoomsController(ApplicationDbContext repository) : ControllerBase
    {
        [HttpGet]
        [Route("list")]
        public IActionResult GetRooms()
        {
            var rooms = repository.Rooms.ToList();

            if (rooms is null)
            {
                return NotFound();
            }

            return Ok(rooms);
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetRoom(int id = 0)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var room = repository.Rooms.FirstOrDefault(r => r.Id == id);

            if (room is null)
            {
                return NotFound();
            }

            return Ok(room);
        }

        [HttpPost]
        [Route("add")]
        public void CreateRoom([FromBody] Room room)
        {
            var persons = repository.Persons.Where(person => person.Sector.Id == room.SectorId).ToList();

            var workplaces = repository.WorkPlaces.Where(workplace => workplace.RoomId == room.Id).ToList();
            
            repository.Add(new Room
            {
                Name = room.Name,
                SectorId = room.SectorId,
                HousingId = room.HousingId,
                Persons = persons,
                WorkPlaces = workplaces,
                Floor = room.Floor,
                Height = room.Height,
                Width = room.Width,
                Length = room.Length,
                Comments = room.Comments
            });

            repository.SaveChanges();
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
