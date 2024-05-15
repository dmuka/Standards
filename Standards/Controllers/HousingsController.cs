using MediatR;
using Microsoft.AspNetCore.Mvc;
using Standards.Core.CQRS.Housings;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.DTOs.Filters;
using Standards.Infrastructure.Data.Repositories.Interfaces;

namespace Standards.Controllers
{
    [ApiController]
    [Route("api/housings")]
    public class HousingsController : ApiBaseController
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
        [Route("filter")]
        public async Task<IActionResult> GetHousingsByFilter([FromBody] HousingsFilterDto filter)
        {
            var query = new GetFiltered.Query(filter);

            var result = await _sender.Send(query);

            return Ok(result);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> CreateHousing([FromBody] HousingDto housing)
        {
            var query = new Create.Query(housing);

            var result = await _sender.Send(query);

            return Ok(result);
        }

        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> EditHousing([FromBody]HousingDto housing)
        {
            var query = new Edit.Query(housing);
            
            var result = await _sender.Send(query);

            return Ok(result);
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
