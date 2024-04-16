using Standards.Core.Models.Departments;
using Standards.Core.Models.Persons;

namespace Standards.Core.Models.Housings
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int HousingId { get; set; }
        public int Floor { get; set; }
        public int Length { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public IList<WorkPlace> WorkPlaces { get; set; } = new List<WorkPlace>();
        public IList<Person> Persons { get; set; } = new List<Person>();
        public int SectorId { get; set; }
        public string Comments { get; set; } = null!;
    }
}
