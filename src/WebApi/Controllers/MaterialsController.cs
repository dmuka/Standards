﻿using Application.Abstractions.Data.Filter;
using Application.Abstractions.Data.Filter.Models;
using Application.UseCases.Common.GenericCRUD;
using Application.UseCases.DTOs;
using Application.UseCases.Materials;
using Domain.Models.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MaterialsController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetMaterials()
    {
        var query = new GetAll.Query();

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetMaterial(int id)
    {
        var query = new GetById.Query<Material>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> CreateMaterial([FromBody] MaterialDto material)
    {
        var query = new Create.Query(material);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> EditMaterial([FromBody] MaterialDto material)
    {
        var query = new Edit.Query(material);
            
        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpDelete]
    [Route("delete/{id:int}")]
    public async Task<IActionResult> DeleteMaterial(int id)
    {
        var query = new Delete.Command<Material>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("filter")]
    public async Task<IActionResult> GetMaterialsByFilter([FromBody] QueryParameters parameters)
    {
        var query = new GetFiltered<Material>.Query(parameters);

        var result = await sender.Send(query);

        return Ok(result);
    }
}