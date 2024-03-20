using Microsoft.AspNetCore.Mvc;
using Standards.Data.Repositories.Interfaces;
using Standards.Models.DTOs;

namespace Standards.Controllers
{
    [Route("api/housings")]
    [ApiController]
    public class HousingsController : ControllerBase
    {
        private readonly IRepository<HousingDto> _repository;

        public HousingsController(IRepository<HousingDto> repository)
        {
            _repository = repository;
        }
        
        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> GetHousings()
        {
            var housings = await _repository.GetAllAsync();
            if (housings is null)
            {
                return NotFound();
            }

            return Ok(housings);
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetHousing(int id = 0)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var housing = await _repository.GetByIdAsync(id);

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
            _repository.Add(new HousingDto
            {
                Name = housing.Name,
                ShortName = housing.ShortName,
                Address = housing.Address,
                FloorsCount = housing.FloorsCount,
                Comments = housing.Comments
            });

            _repository.SaveAsync();
        }

        [HttpPut]
        [Route("edit/{id}")]
        public void EditHousing(int id, [FromBody]HousingDto housing)
        {
            if (housing.Id == id)
            {
                _repository.Update(housing);

                _repository.Save();
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public void DeleteHousing(int id)
        {
            _repository.Remove(id);

            _repository.Save();
        }
    }
}
