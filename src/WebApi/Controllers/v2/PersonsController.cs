using Application.Abstractions.Data.Filter.Models;
using Application.UseCases.Common;
using Application.UseCases.DTOs;
using Application.UseCases.Persons;
using Domain.Aggregates.Persons;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v2;

[ApiController]
[Route("api/v2/PersonsController")]
public class PersonsController(ISender sender) : ApiBaseController
{
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetPersons()
    {
        var query = new GetAllPersons.Query();

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetPerson(Guid id)
    {
        var query = new GetPersonById.Query(new PersonId(id));

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> AddPerson([FromBody] PersonDto2 person)
    {
        var query = new AddPerson.Command(person);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> EditPerson([FromBody]PersonDto2 person)
    {
        var query = new EditPerson.Command(person);
            
        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpDelete]
    [Route("delete/{id:Guid}")]
    public async Task<IActionResult> DeletePerson(Guid id)
    {
        var query = new DeletePerson.Command(new PersonId(id));

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