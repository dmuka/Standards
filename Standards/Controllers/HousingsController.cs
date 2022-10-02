using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Standards.Data;
using Standards.Models;

namespace Standards.Controllers
{
    [Route("housings")]
    [ApiController]
    public class HousingsController : ControllerBase
    {
        readonly ApplicationDbContext _repository;

        public HousingsController(ApplicationDbContext repository)
        {
            _repository = repository;
        }
        
        [HttpGet]
        [Route("list")]
        public IEnumerable<Housing> GetHousings()
        {
            var result = _repository.Housings.ToList();
            
            return result;
        }

        [HttpGet]
        [Route("")]
        public Housing GetHousing(int id)
        {
            return _repository.Housings.First(h => h.Id == id);
        }

        [HttpPost]
        [Route("add")]
        public void CreateHousing([FromBody]Housing housing)
        {
            _repository.Add(new Housing
            {
                Name = housing.Name,
                ShortName = housing.ShortName,
                Address = housing.Address,
                FloorsCount = housing.FloorsCount,
                Departments = housing.Departments,
            });

            _repository.SaveChanges();
        }

        [HttpPut]
        [Route("edit")]
        public void EditHousing(int id, [FromBody]Housing housing)
        {
            if (housing.Id == id)
            {
                _repository.Entry(housing).State = EntityState.Modified;

                _repository.SaveChanges();
            }
        }

        [HttpDelete]
        [Route("delete")]
        public void DeleteHousing(int id)
        {
            var housing = _repository.Housings.Find(id);

            if (housing is not null)
            {
                _repository.Housings.Remove(housing);

                _repository.SaveChanges();
            }
        }
    }
}
