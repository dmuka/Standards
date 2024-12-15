using Application.CQRS.Common.GenericCRUD;
using Application.CQRS.Departments;
using Domain.Models.Departments;
using Domain.Models.DTOs;
using Infrastructure.Filter.Implementations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepartmentsController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetDepartments()
    {
        var query = new GetAll.Query();

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetDepartment(int id)
    {
        var query = new GetById.Query<Department>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> CreateDepartment([FromBody] DepartmentDto department)
    {
        var query = new Create.Query(department);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> EditDepartment([FromBody] DepartmentDto department)
    {
        var query = new Edit.Query(department);
            
        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpDelete]
    [Route("delete/{id:int}")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        var query = new Delete.Query<Department>(id);

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