using Standards.Core.Models.Housings;
using Standards.Core.Models.Persons;

namespace Standards.Core.Models.Departments
{
    public class Sector : BaseEntity
    {
        public Department Department { get; set; }
        public IList<Room> Rooms { get; set; } = new List<Room>();
        public IList<WorkPlace> WorkPlaces { get; set; } = new List<WorkPlace>();
        public IList<Person> Persons { get; set; } = new List<Person>();
    }
}
