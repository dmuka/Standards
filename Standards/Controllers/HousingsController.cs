using Microsoft.AspNetCore.Mvc;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Users;
using Standards.Core.CQRS.Housings;
using MediatR;
using Standards.Infrastructure.Data.Repositories.Interfaces;

namespace Standards.Controllers
{
    [Route("api/housings")]
    [ApiController]
    public class HousingsController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly ISender _sender;

        public HousingsController(IRepository repository, ISender sender)
        {
            _repository = repository;
            _sender = sender;
        }
        
        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> GetHousings()
        {
            var query = new GetAll.Query();

            var result = await _sender.Send(query);

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetHousing(int id)
        {
            var query = new GetById.Query(id);

            var result = await _sender.Send(query);

            return Ok(result);
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
