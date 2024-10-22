using MediatR;
using Microsoft.AspNetCore.Mvc;
using Standards.Core.CQRS.Common.GenericCRUD;
using Standards.Core.CQRS.Departments;
using Standards.Core.Models.Departments;
using Standards.Core.Models.DTOs;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Filter.Implementations;

namespace Standards.Controllers
{
    [Route("api/departments")]
    [ApiController]
    public class DepartmentsController(IRepository repository, ISender sender) : ControllerBase
    {
        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> GetDepartments()
        {
            var query = new GetAll.Query();

            var result = await sender.Send(query);

            return Ok(result);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetDepartment(int id)
        {
            var query = new GetById<Department>.Query(id);

            var result = await sender.Send(query);

            return Ok(result);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentDto department)
        {
            var query = new Create.Query(department);

            var result = await sender.Send(query);

            return Ok(result);
        }

        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> EditDepartment([FromBody] DepartmentDto department)
        {
            var query = new Edit.Query(department);
            
            var result = await sender.Send(query);

            return Ok(result);
        }

        [HttpDelete]
        [Route("delete/{id:int}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var query = new Delete<Department>.Query(id);

            var result = await sender.Send(query);

            return Ok(result);
        }

        [HttpPost]
        [Route("filter")]
        public async Task<IActionResult> GetDepartmentsByFilter([FromBody] QueryParameters parameters)
        {
            var query = new GetFiltered<Department>.Query(parameters);

            var result = await sender.Send(query);

            return Ok(result);
        }
    }
}
