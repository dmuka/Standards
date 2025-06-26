using Application.UseCases.Common.GenericCRUD;
using Application.UseCases.DTOs;
using Application.UseCases.Housings;
using Domain.Aggregates.Housings;
using Infrastructure.Filter.Implementations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Housing = Domain.Models.Housings.Housing;

namespace WebApi.Controllers.v2;

[ApiController]
[Route("api/v2/HousingsController")]
public class HousingsController(ISender sender) : ApiBaseController
{
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetHousings()
    {
        var query = new GetAllHousings.Query();

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetHousing(Guid id)
    {
        var query = new GetHousingById.Query(new HousingId(id));

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> AddHousing([FromBody] HousingDto2 housing)
    {
        var query = new AddHousing.Command(housing);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> EditHousing([FromBody]HousingDto2 housing)
    {
        var query = new EditHousing.Command(housing);
            
        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpDelete]
    [Route("delete/{id:Guid}")]
    public async Task<IActionResult> DeleteHousing(Guid id)
    {
        var query = new DeleteHousing.Command(new HousingId(id));

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
}