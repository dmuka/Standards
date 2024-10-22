using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Standards.Core.CQRS.Common.GenericCRUD;
using Standards.Core.Models.Departments;
using Standards.Infrastructure.Data;

namespace Standards.Controllers
{
    [Route("api/sectors")]
    [ApiController]
    public class SectorsController(ApplicationDbContext repository, ISender sender) : ControllerBase
    {
        [HttpGet]
        [Route("list")]
        public IActionResult GetSectors()
        {
            var rooms = repository.Rooms.ToList();
            var sectors = repository.Sectors.ToList();
            sectors.ForEach(s => s.Rooms.ToList().AddRange(rooms.FindAll(r => r.Sector.Id == s.Id)));

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

            var rooms = repository.Rooms.ToList();
            var sector = repository.Sectors.FirstOrDefault(s => s.Id == id);

            if (sector is null)
            {
                return NotFound($"Not found sector with id: {id}");
            }

            sector.Rooms = rooms.FindAll(r => r.Sector.Id == sector.Id);

            return Ok(sector);
        }

        [HttpPost]
        [Route("add")]
        public void CreateSector([FromBody] Sector sector)
        {
            repository.Add(new Sector
            {
                Name = sector.Name,
                ShortName = sector.ShortName,
                WorkPlaces = sector.WorkPlaces,
                Persons = sector.Persons,
                Rooms = sector.Rooms,
                Comments = sector.Comments
            });

            repository.SaveChanges();
        }

        [HttpPut]
        [Route("edit")]
        public void EditSector(int id, [FromBody] Sector sector)
        {
            if (sector.Id != id) return;
            
            repository.Entry(sector).State = EntityState.Modified;

            repository.SaveChanges();
        }

        [HttpDelete]
        [Route("delete/{id:int}")]
        public async Task<IActionResult> DeleteHousing(int id)
        {
            var query = new Delete<Sector>.Query(id);

            var result = await sender.Send(query);

            return Ok(result);
        }
    }
}
