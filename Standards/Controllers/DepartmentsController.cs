using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Standards.Core.Models.Departments;
using Standards.Data;

namespace Standards.Controllers
{
    [Route("api/departments")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        readonly ApplicationDbContext _repository;

        public DepartmentsController(ApplicationDbContext repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("list")]
        public IActionResult GetDepartments()
        {
            var rooms = _repository.Rooms.ToList();
            var sectors = _repository.Sectors.ToList();
            sectors.ForEach(s => s.Rooms.ToList().AddRange(rooms.FindAll(r => r.SectorId == s.Id)));
            var departments = _repository.Departments.ToList();
            departments.ForEach(d => d.Sectors.ToList().AddRange(sectors.FindAll(s => s.DepartmentId == d.Id)));

            return Ok(departments);
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetDepartment(int id = 0)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var rooms = _repository.Rooms.ToList();
            var sectors = _repository.Sectors.ToList();
            sectors.ForEach(s => s.Rooms.ToList().AddRange(rooms.FindAll(r => r.SectorId == s.Id)));

            var department = _repository.Departments.FirstOrDefault(d => d.Id == id);

            if (department is null)
            {
                return NotFound($"Not found department with id: {id}");
            }

            department.Sectors = sectors.FindAll(s => s.DepartmentId == department.Id);

            return Ok(department);
        }

        [HttpPost]
        [Route("add")]
        public void CreateDepartment([FromBody] Department department)
        {
            _repository.Add(new Department
            {
                Name = department.Name,
                ShortName = department.ShortName,
                Sectors = department.Sectors,
                Comments = department.Comments
            });

            _repository.SaveChanges();
        }

        [HttpPut]
        [Route("edit")]
        public void EditDepartment(int id, [FromBody] Department department)
        {
            if (department.Id == id)
            {
                _repository.Entry(department).State = EntityState.Modified;

                _repository.SaveChanges();
            }
        }

        [HttpDelete]
        [Route("delete")]
        public void DeleteDepartment(int id)
        {
            var department = _repository.Departments.Find(id);

            if (department is not null)
            {
                _repository.Departments.Remove(department);

                _repository.SaveChanges();
            }
        }
    }
}
