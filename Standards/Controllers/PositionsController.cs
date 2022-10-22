using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Standards.Data;
using Standards.Models.Persons;

namespace Standards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        readonly ApplicationDbContext _repository;

        public PositionsController(ApplicationDbContext repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("list")]
        public IActionResult GetPositions()
        {
            var positions = _repository.Positions.ToList();

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

            var position = _repository.Positions.FirstOrDefault(p => p.Id == id);

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
            _repository.Add(new Position
            {
                Name = position.Name,
                Comments = position.Comments
            });

            _repository.SaveChanges();
        }

        [HttpPut]
        [Route("edit")]
        public void EditPosition(int id, [FromBody] Position position)
        {
            if (position.Id == id)
            {
                _repository.Entry(position).State = EntityState.Modified;

                _repository.SaveChanges();
            }
        }

        [HttpDelete]
        [Route("delete")]
        public void DeletePosition(int id)
        {
            var position = _repository.Positions.Find(id);

            if (position is not null)
            {
                _repository.Positions.Remove(position);

                _repository.SaveChanges();
            }
        }
    }
}
