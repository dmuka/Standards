using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Standards.Core.Models.Departments;
using Standards.Data;

namespace Standards.Controllers
{
    [Route("api/workplaces")]
    [ApiController]
    public class WorkPlacesController : ControllerBase
    {
        readonly ApplicationDbContext _repository;

        public WorkPlacesController(ApplicationDbContext repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("list")]
        public IActionResult GetWorkPlaces()
        {
            var workPlaces = _repository.WorkPlaces.ToList();

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

            var workPlace = _repository.WorkPlaces.FirstOrDefault(w => w.Id == id);

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
            _repository.Add(new WorkPlace
            {
                Name = workPlace.Name,
                Responcible = workPlace.Responcible,
                ImagePath = workPlace.ImagePath,
                Comments = workPlace.Comments
            });

            _repository.SaveChanges();
        }

        [HttpPut]
        [Route("edit")]
        public void EditWorkPlace(int id, [FromBody] WorkPlace workPlace)
        {
            if (workPlace.Id == id)
            {
                _repository.Entry(workPlace).State = EntityState.Modified;

                _repository.SaveChanges();
            }
        }

        [HttpDelete]
        [Route("delete")]
        public void DeleteWorkPlace(int id)
        {
            var workPlace = _repository.WorkPlaces.Find(id);

            if (workPlace is not null)
            {
                _repository.WorkPlaces.Remove(workPlace);

                _repository.SaveChanges();
            }
        }
    }

}