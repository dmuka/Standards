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
    public class HousingsController(IRepository repository, ISender sender) : ApiBaseController
    {
        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> GetHousings()
        {
            var query = new GetAll.Query();

            var result = await sender.Send(query);

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetHousing(int id)
        {
            var query = new GetById.Query(id);

            var result = await sender.Send(query);

            return Ok(result);
        }

        [HttpPost]
        [Route("filter")]
        public async Task<IActionResult> GetHousingsByFilter([FromBody] HousingsFilterDto filter)
        {
            var query = new GetFiltered.Query(filter);

            var result = await sender.Send(query);

            return Ok(result);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> CreateHousing([FromBody] HousingDto housing)
        {
            var query = new Create.Query(housing);

            var result = await sender.Send(query);

            return Ok(result);
        }

        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> EditHousing([FromBody]HousingDto housing)
        {
            var query = new Edit.Query(housing);
            
            var result = await sender.Send(query);

            return Ok(result);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task DeleteHousing(int id)
        {
            var housing = await repository.GetByIdAsync<HousingDto>(id);

            await repository.DeleteAsync(housing);

            await repository.SaveChangesAsync();
        }
    }
}
