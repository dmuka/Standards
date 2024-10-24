using MediatR;
using Microsoft.AspNetCore.Mvc;
using Standards.Core.CQRS.Common.GenericCRUD;
using Standards.Core.CQRS.Housings;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Housings;
using Standards.Infrastructure.Filter.Implementations;

namespace Standards.Controllers
{
    [ApiController]
    [Route("api/housings")]
    public class HousingsController(ISender sender) : ApiBaseController
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
        [Route("{id:int}")]
        public async Task<IActionResult> GetHousing(int id)
        {
            var query = new GetById.Query<Housing>(id);

            var result = await sender.Send(query);

            return Ok(result);
        }

        [HttpPost]
        [Route("filter")]
        public async Task<IActionResult> GetHousingsByFilter([FromBody] QueryParameters parameters)
        {
            var query = new GetFiltered<Housing>.Query(parameters);

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
        [Route("delete/{id:int}")]
        public async Task<IActionResult> DeleteHousing(int id)
        {
            var query = new Delete.Query<Housing>(id);

            var result = await sender.Send(query);

            return Ok(result);
        }
    }
}
