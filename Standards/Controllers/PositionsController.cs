using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Standards.Core.Models.Persons;
using Standards.Infrastructure.Data;

namespace Standards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController(ApplicationDbContext repository) : ControllerBase
    {
        [HttpGet]
        [Route("list")]
        public IActionResult GetPositions()
        {
            var positions = repository.Positions.ToList();

            return Ok(positions);
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetPosition(int id = 0)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var position = repository.Positions.FirstOrDefault(p => p.Id == id);

            if (position is null)
            {
                return NotFound($"Not found position with id: {id}");
            }

            return Ok(position);
        }

        [HttpPost]
        [Route("add")]
        public void CreatePosition([FromBody] Position position)
        {
            repository.Add(new Position
            {
                Name = position.Name,
                Comments = position.Comments
            });

            repository.SaveChanges();
        }

        [HttpPut]
        [Route("edit")]
        public void EditPosition(int id, [FromBody] Position position)
        {
            if (position.Id != id) return;
            
            repository.Entry(position).State = EntityState.Modified;

            repository.SaveChanges();
        }

        [HttpDelete]
        [Route("delete")]
        public void DeletePosition(int id)
        {
            var position = repository.Positions.Find(id);

            if (position is not null)
            {
                repository.Positions.Remove(position);

                repository.SaveChanges();
            }
        }
    }
}
