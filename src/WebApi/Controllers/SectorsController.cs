﻿using Application.Abstractions.Data.Filter;
using Application.Abstractions.Data.Filter.Models;
using Application.UseCases.Common.GenericCRUD;
using Application.UseCases.DTOs;
using Application.UseCases.Sectors;
using Domain.Models.Departments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SectorsController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetSectors()
    {
        var query = new GetAll.Query();

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetSector(int id)
    {
        var query = new GetById.Query<Sector>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> CreateSector([FromBody] SectorDto sector)
    {
        var query = new Create.Query(sector);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult>  EditSector([FromBody] SectorDto sector)
    {
        var query = new Edit.Query(sector);
            
        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpDelete]
    [Route("delete/{id:int}")]
    public async Task<IActionResult> DeleteSector(int id)
    {
        var query = new Delete.Command<Sector>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("filter")]
    public async Task<IActionResult> GetSectorsByFilter([FromBody] QueryParameters parameters)
    {
        var query = new GetFiltered<Sector>.Query(parameters);

        var result = await sender.Send(query);

        return Ok(result);
    }
}