using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Standards.Core.Models.Persons;
using Standards.Infrastructure.Data;

namespace Standards.Controllers
{
    [Route("api/persons/categories")]
    [ApiController]
    public class CategoriesController(ApplicationDbContext repository) : ControllerBase
    {
        [HttpGet]
        [Route("list")]
        public IActionResult GetCategories()
        {
            var categories = repository.Categories.ToList();

            return Ok(categories);
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetCategory(int id = 0)
        {
            if (id == 0) return BadRequest();

            var category = repository.Categories.FirstOrDefault(category => category.Id == id);

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
            repository.Add(new Category
            {
                Name = category.Name,
                Comments = category.Comments
            });

            repository.SaveChanges();
        }

        [HttpPut]
        [Route("edit")]
        public void EditCategory(int id, [FromBody] Category category)
        {
            if (category.Id != id) return;
            
            repository.Entry(category).State = EntityState.Modified;

            repository.SaveChanges();
        }

        [HttpDelete]
        [Route("delete")]
        public void DeleteCategory(int id)
        {
            var category = repository.Categories.Find(id);

            if (category is null) return;
            
            repository.Categories.Remove(category);

            repository.SaveChanges();
        }
    }
}