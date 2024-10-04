using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Standards.Core.Models.Departments;
using Standards.Infrastructure.Data;

namespace Standards.Controllers
{
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
        public void CreateWorkPlace([FromBody] WorkPlace workPlace)
        {
            repository.Add(new WorkPlace
            {
                Name = workPlace.Name,
                Responcible = workPlace.Responcible,
                ImagePath = workPlace.ImagePath,
                Comments = workPlace.Comments
            });

            repository.SaveChanges();
        }

        [HttpPut]
        [Route("edit")]
        public void EditWorkPlace(int id, [FromBody] WorkPlace workPlace)
        {
            if (workPlace.Id != id) return;
            repository.Entry(workPlace).State = EntityState.Modified;

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

}