using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Standards.Data;
using Standards.Models.DTOs;
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
            var housings = _repository.Housings.ToList();
            if (housings is null)
            {
                return NotFound();
            }

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

            var housing = _repository.Housings.FirstOrDefault(h => h.Id == id);

            if (housing is null)
            {
                return NotFound();
            }
            
            return Ok(housing);
        }

        [HttpPost]
        [Route("add")]
        public void CreateHousing([FromBody] HousingDto housing)
        {
            _repository.Add(new Housing
            {
                Name = housing.Name,
                ShortName = housing.ShortName,
                Address = housing.Address,
                FloorsCount = housing.FloorsCount,
                Comments = housing.Comments
            });

            _repository.SaveChanges();
        }

        [HttpPut]
        [Route("edit/{id}")]
        public void EditHousing(int id, [FromBody]HousingDto housing)
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
