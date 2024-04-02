using Microsoft.AspNetCore.Mvc;
using Standards.Data.Repositories.Interfaces;
using Standards.Models.DTOs;
using Standards.Models.Users;

namespace Standards.Controllers
{
    [Route("api/housings")]
    [ApiController]
    public class HousingsController : ControllerBase
    {
        private readonly IRepository _repository;

        public HousingsController(IRepository repository)
        {
            _repository = repository;
        }
        
        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> GetHousings()
        {
            var housings = await _repository.GetListAsync<HousingDto>();

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

            var housing = await _repository.GetByIdAsync<HousingDto>(id);

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

            _repository.SaveChangesAsync();
        }

        [HttpPut]
        [Route("edit/{id}")]
        public void EditHousing(int id, [FromBody]HousingDto housing)
        {
            if (housing.Id == id)
            {
                _repository.Update(housing);

                _repository.SaveChangesAsync();
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task DeleteHousing(int id)
        {
            var housing = await _repository.GetByIdAsync<HousingDto>(id);

            await _repository.DeleteAsync(housing);

            await _repository.SaveChangesAsync();
        }
    }
}
