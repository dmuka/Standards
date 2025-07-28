using Application.Abstractions.Data.Filter.Models;
using Application.UseCases.Common;
using Application.UseCases.DTOs;
using Application.UseCases.Departments;
using Domain.Aggregates.Departments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v2;

[ApiController]
[Route("api/v2/DepartmentsController")]
public class DepartmentsController(ISender sender) : ApiBaseController
{
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetDepartments()
    {
        var query = new GetAllDepartments.Query();

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetDepartment(Guid id)
    {
        var query = new GetDepartmentById.Query(new DepartmentId(id));

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> AddDepartment([FromBody] DepartmentDto2 housing)
    {
        var query = new AddDepartment.Command(housing);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> EditDepartment([FromBody]DepartmentDto2 housing)
    {
        var query = new EditDepartment.Command(housing);
            
        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpDelete]
    [Route("delete/{id:Guid}")]
    public async Task<IActionResult> DeleteDepartment(Guid id)
    {
        var query = new DeleteDepartment.Command(new DepartmentId(id));

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("filter")]
    public async Task<IActionResult> GetDepartmentsByFilter([FromBody] QueryParameters parameters)
    {
        var query = new GetFiltered<Department>.Query(parameters);

        var result = await sender.Send(query);

        return Ok(result);
    }
}