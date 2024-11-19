using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Standards.Core.Models.Departments;
using Standards.Infrastructure.Data;

namespace Standards.Controllers;

[Route("api/workplaces")]
[ApiController]
public class WorkPlacesController(ApplicationDbContext repository) : ControllerBase
{
    [HttpGet]
    [Route("list")]
    public IActionResult GetWorkPlaces()
    {
        var workPlaces = repository.WorkPlaces.ToList();

        return Ok(workPlaces);
    }

    [HttpGet]
    [Route("")]
    public IActionResult GetWorkPlace(int id = 0)
    {
        if (id == 0)
        {
            return BadRequest();
        }

        var workPlace = repository.WorkPlaces.FirstOrDefault(w => w.Id == id);

        if (workPlace is null)
        {
            return NotFound($"Not found workplace with id: {id}");
        }

        return Ok(workPlace);
    }

    [HttpPost]
    [Route("add")]
    public void CreateWorkPlace([FromBody] Workplace workplace)
    {
        repository.Add(new Workplace
        {
            Name = workplace.Name,
            Responcible = workplace.Responcible,
            ImagePath = workplace.ImagePath,
            Comments = workplace.Comments
        });

        repository.SaveChanges();
    }

    [HttpPut]
    [Route("edit")]
    public void EditWorkPlace(int id, [FromBody] Workplace workplace)
    {
        if (workplace.Id != id) return;
        repository.Entry(workplace).State = EntityState.Modified;

        repository.SaveChanges();
    }

    [HttpDelete]
    [Route("delete")]
    public void DeleteWorkPlace(int id)
    {
        var workPlace = repository.WorkPlaces.Find(id);

        if (workPlace is null) return;
        repository.WorkPlaces.Remove(workPlace);

        repository.SaveChanges();
    }
}