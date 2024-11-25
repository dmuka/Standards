using MediatR;
using Microsoft.AspNetCore.Mvc;
using Standards.Core.CQRS.Common.GenericCRUD;
using Standards.Core.CQRS.Persons;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Persons;
using Standards.Infrastructure.Filter.Implementations;

namespace Standards.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PersonsController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetPersons()
    {
        var query = new GetAll.Query();

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetPerson(int id)
    {
        var query = new GetById.Query<Person>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> CreatePerson([FromBody] PersonDto person)
    {
        var query = new Create.Query(person);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> EditPerson([FromBody] PersonDto person)
    {
        var query = new Edit.Query(person);
            
        var result = await sender.Send(query);

        return Ok(result);
    }


    [HttpDelete]
    [Route("delete/{id:int}")]
    public async Task<IActionResult> DeletePerson(int id)
    {
        var query = new Delete.Query<Person>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("filter")]
    public async Task<IActionResult> GetPersonsByFilter([FromBody] QueryParameters parameters)
    {
        var query = new GetFiltered<Person>.Query(parameters);

        var result = await sender.Send(query);

        return Ok(result);
    }
}