using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Standards.Data;
using Standards.Models.Housings;

namespace Standards.Controllers
{
    [Route("api/housings")]
    [ApiController]
    public class HousingsController : ControllerBase
    {
        readonly ApplicationDbContext _repository;

        public HousingsController(ApplicationDbContext repository)
        {
            _repository = repository;
        }
        
        [HttpGet]
        [Route("list")]
        public IActionResult GetHousings()
        {
            //var rooms = _repository.Rooms.ToList();
            //var sectors = _repository.Sectors.ToList();
            //sectors.ForEach(s => s.Rooms.ToList().AddRange(rooms.FindAll(r => r.SectorId == s.Id)));
            //var departments = _repository.Departments.ToList();
            //departments.ForEach(d => d.Sectors.ToList().AddRange(sectors.FindAll(s => s.DepartmentId == d.Id)));
            var housings = _repository.Housings.ToList();
            if (housings is null)
            {
                return NotFound();
            }

            //housings.ForEach(h => h.Departments.ToList().AddRange(departments.FindAll(d => d.Id == h.Id)));

            return Ok(housings);
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetHousing(int id = 0)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var rooms = _repository.Rooms.ToList();
            var sectors = _repository.Sectors.ToList();
            sectors.ForEach(s => s.Rooms.ToList().AddRange(rooms.FindAll(r => r.SectorId == s.Id)));
            var departments = _repository.Departments.ToList();
            var housing = _repository.Housings.FirstOrDefault(h => h.Id == id);
            departments.ForEach(d => d.Sectors.ToList().AddRange(sectors.FindAll(s => s.DepartmentId == d.Id)));

            if (housing is null)
            {
                return NotFound();
            }
            
            housing.Departments.ToList().AddRange(departments.FindAll(d => d.Id == housing.Id));
            
            return Ok(housing);
        }

        [HttpPost]
        [Route("add")]
        public void CreateHousing([FromBody]Housing housing)
        {
            _repository.Add(new Housing
            {
                Name = housing.Name,
                ShortName = housing.ShortName,
                Address = housing.Address,
                FloorsCount = housing.FloorsCount,
                Departments = housing.Departments,
                Comments = housing.Comments
            });

            _repository.SaveChanges();
        }

        [HttpPut]
        [Route("edit")]
        public void EditHousing(int id, [FromBody]Housing housing)
        {
            if (housing.Id == id)
            {
                _repository.Entry(housing).State = EntityState.Modified;

                _repository.SaveChanges();
            }
        }

        [HttpDelete]
        [Route("delete")]
        public void DeleteHousing(int id)
        {
            var housing = _repository.Housings.Find(id);

            if (housing is not null)
            {
                _repository.Housings.Remove(housing);

                _repository.SaveChanges();
            }
        }
    }
}
