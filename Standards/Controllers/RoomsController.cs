using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Standards.Core.Models.Housings;
using Standards.Data;

namespace Standards.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        readonly ApplicationDbContext _repository;

        public RoomsController(ApplicationDbContext repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("list")]
        public IActionResult GetRooms()
        {
            var rooms = _repository.Rooms.ToList();

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

            var room = _repository.Rooms.FirstOrDefault(r => r.Id == id);

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
            var persons = _repository.Persons.Where(person => person.Sector.Id == room.SectorId).ToList();

            var workplaces = _repository.WorkPlaces.Where(workplace => workplace.RoomId == room.Id).ToList();
            
            _repository.Add(new Room
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

            _repository.SaveChanges();
        }

        [HttpPut]
        [Route("edit")]
        public void EditRoom(int id, [FromBody] Room room)
        {
            if (room.Id == id)
            {
                _repository.Entry(room).State = EntityState.Modified;

                _repository.SaveChanges();
            }
        }

        [HttpDelete]
        [Route("delete")]
        public void DeleteRoom(int id)
        {
            var room = _repository.Rooms.Find(id);

            if (room is not null)
            {
                _repository.Rooms.Remove(room);

                _repository.SaveChanges();
            }
        }
    }
}
