using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Standards.Core.Models.Departments;
using Standards.Infrastructure.Data;

namespace Standards.Controllers
{
    [Route("api/departments")]
    [ApiController]
    public class DepartmentsController(ApplicationDbContext repository) : ControllerBase
    {
        [HttpGet]
        [Route("list")]
        public IActionResult GetDepartments()
        {
            var rooms = repository.Rooms.ToList();
            var sectors = repository.Sectors.ToList();
            sectors.ForEach(s => s.Rooms.ToList().AddRange(rooms.FindAll(r => r.Sector.Id == s.Id)));
            var departments = repository.Departments.ToList();
            departments.ForEach(d => d.Sectors.ToList().AddRange(sectors.FindAll(s => s.Department.Id == d.Id)));

            return Ok(departments);
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetDepartment(int id = 0)
        {
            if (id == 0) return BadRequest();

            var rooms = repository.Rooms.ToList();
            var sectors = repository.Sectors.ToList();
            sectors.ForEach(s => s.Rooms.ToList().AddRange(rooms.FindAll(r => r.Sector.Id == s.Id)));

            var department = repository.Departments.FirstOrDefault(d => d.Id == id);

            if (department is null)
            {
                return NotFound($"Not found department with id: {id}");
            }

            department.Sectors = sectors.FindAll(s => s.Department.Id == department.Id);

            return Ok(department);
        }

        [HttpPost]
        [Route("add")]
        public void CreateDepartment([FromBody] Department department)
        {
            repository.Add(new Department
            {
                Name = department.Name,
                ShortName = department.ShortName,
                Sectors = department.Sectors,
                Comments = department.Comments
            });

            repository.SaveChanges();
        }

        [HttpPut]
        [Route("edit")]
        public void EditDepartment(int id, [FromBody] Department department)
        {
            if (department.Id != id) return;
            
            repository.Entry(department).State = EntityState.Modified;

            repository.SaveChanges();
        }

        [HttpDelete]
        [Route("delete")]
        public void DeleteDepartment(int id)
        {
            var department = repository.Departments.Find(id);

            if (department is null) return;
            
            repository.Departments.Remove(department);

            repository.SaveChanges();
        }
    }
}
