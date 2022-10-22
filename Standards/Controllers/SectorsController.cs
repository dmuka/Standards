using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Standards.Data;
using Standards.Models;
using Standards.Models.Departments;

namespace Standards.Controllers
{
    [Route("api/sectors")]
    [ApiController]
    public class SectorsController : ControllerBase
    {
        readonly ApplicationDbContext _repository;

        public SectorsController(ApplicationDbContext repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("list")]
        public IActionResult GetSectors()
        {
            var rooms = _repository.Rooms.ToList();
            var sectors = _repository.Sectors.ToList();
            sectors.ForEach(s => s.Rooms.ToList().AddRange(rooms.FindAll(r => r.SectorId == s.Id)));

            return Ok(sectors);
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetSector(int id = 0)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var rooms = _repository.Rooms.ToList();
            var sector = _repository.Sectors.FirstOrDefault(s => s.Id == id);

            if (sector is null)
            {
                return NotFound($"Not found sector with id: {id}");
            }

            sector.Rooms = rooms.FindAll(r => r.SectorId == sector.Id);

            return Ok(sector);
        }

        [HttpPost]
        [Route("add")]
        public void CreateSector([FromBody] Sector sector)
        {
            _repository.Add(new Sector
            {
                Name = sector.Name,
                ShortName = sector.ShortName,
                WorkPlaces = sector.WorkPlaces,
                Persons = sector.Persons,
                Rooms = sector.Rooms
            });

            _repository.SaveChanges();
        }

        [HttpPut]
        [Route("edit")]
        public void EditSector(int id, [FromBody] Sector sector)
        {
            if (sector.Id == id)
            {
                _repository.Entry(sector).State = EntityState.Modified;

                _repository.SaveChanges();
            }
        }

        [HttpDelete]
        [Route("delete")]
        public void DeleteSector(int id)
        {
            var sector = _repository.Sectors.Find(id);

            if (sector is not null)
            {
                _repository.Sectors.Remove(sector);

                _repository.SaveChanges();
            }
        }
    }
}
