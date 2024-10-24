using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Departments;
using Standards.Core.Models.Interfaces;
using Standards.Core.Models.Persons;

namespace Standards.Core.Models.Housings
{
    public class Room : BaseEntity, IEntity<int>
    {
        public Housing Housing { get; set; }
        public int Floor { get; set; }
        public double Length { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public IList<Workplace> WorkPlaces { get; set; } = new List<Workplace>();
        public IList<Person> Persons { get; set; } = new List<Person>();
        public Sector Sector { get; set; }

        public static string GetCacheKey()
        {
            return Cache.Rooms;
        }
    }
}
