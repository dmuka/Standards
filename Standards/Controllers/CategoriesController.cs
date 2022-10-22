using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Standards.Data;
using Standards.Models.Persons;

namespace Standards.Controllers
{
    [Route("api/persons/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        readonly ApplicationDbContext _repository;

        public CategoriesController(ApplicationDbContext repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("list")]
        public IActionResult GetCategories()
        {
            var categories = _repository.Categories.ToList();

            return Ok(categories);
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetCategory(int id = 0)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var category = _repository.Categories.FirstOrDefault(c => c.Id == id);

            if (category is null)
            {
                return NotFound($"Not found category with id: {id}");
            }

            return Ok(category);
        }

        [HttpPost]
        [Route("add")]
        public void CreateCategory([FromBody] Category category)
        {
            _repository.Add(new Category
            {
                Name = category.Name,
                Comments = category.Comments
            });

            _repository.SaveChanges();
        }

        [HttpPut]
        [Route("edit")]
        public void EditCategory(int id, [FromBody] Category category)
        {
            if (category.Id == id)
            {
                _repository.Entry(category).State = EntityState.Modified;

                _repository.SaveChanges();
            }
        }

        [HttpDelete]
        [Route("delete")]
        public void DeleteCategory(int id)
        {
            var category = _repository.Categories.Find(id);

            if (category is not null)
            {
                _repository.Categories.Remove(category);

                _repository.SaveChanges();
            }
        }
    }
}